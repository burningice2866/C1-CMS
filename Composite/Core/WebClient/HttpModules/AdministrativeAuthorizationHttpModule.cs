﻿using Composite.C1Console.Security;
using Composite.Core.Application;
using Composite.Core.Configuration;
using Composite.Core.IO;
using Composite.Core.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;


namespace Composite.Core.WebClient.HttpModules
{
    /// <summary>
    ///  Http Module that ensures that only authenticated users can access /Composite/* files not explicitly allowed to everyone.
    ///  Also ensure that HTTPS rules are enforced.
    /// </summary>
    internal class AdministrativeAuthorizationHttpModule : IHttpModule
    {
        private static readonly List<string> _allAllowedPaths = new List<string>();
        private static string _adminRootPath;
        private static string _loginPagePath;
        private static object _lock = new object();
        private static bool _allowC1ConsoleRequests;
        private static bool _forceHttps = true;
        private static bool _allowFallbackToHttp = true;
        private static int? _customHttpsPortNumber;

        private const string webauthorizationRelativeConfigPath = "~/Composite/webauthorization.config";
        private const string c1ConsoleAccessRelativeConfigPath = "~/App_Data/Composite/Configuration/C1ConsoleAccess.xml";
        private const string unsecureRedirectRelativePath = "~/Composite/unsecure.aspx";
        private const string loginPagePathAttributeName = "loginPagePath";
        private const string allowElementName = "allow";
        private const string allow_pathAttributeName = "path";

        private const string c1ConsoleRequestsNotAllowedHtmlTemplate = @"<!DOCTYPE html>
<html lang='en'>
    <head>
        <title>Access Denied</title>
    </head>
    <body>
        <h1>Access Denied</h1>
        <p>Administrative access has been disabled on this site.</p>
        <p style='display:none'>IE Padding, preventing it from showing a generic error page: {0}</p>
    </body>
</html>";

        static AdministrativeAuthorizationHttpModule()
        {
            if (C1Directory.Exists(HostingEnvironment.MapPath(UrlUtils.AdminRootPath)))
            {
                LoadConfiguration();
            }
            else
            {
                _allowC1ConsoleRequests = false;
            }
        }



        public void Dispose()
        {
        }



        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += context_AuthenticateRequest;
        }



        private bool AlwaysAllowUnsecured(string requestPath)
        {
            string fileName = Path.GetFileName(requestPath);
            return fileName.StartsWith("unsecure") ||
                fileName == "blank.png" ||
                fileName == "box.png" ||
                fileName == "button.png" ||
                fileName == "startcomposite.png" ||
                fileName == "default.css.aspx";

        }



        private void context_AuthenticateRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            HttpContext context = application.Context;

            string currentPath = context.Request.Path.ToLowerInvariant();

            if (_adminRootPath == null || !currentPath.StartsWith(_adminRootPath))
            {
                return;
            }

            if (!_allowC1ConsoleRequests)
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "text/html";
                string iePadding = new String('!', 512);
                context.Response.Write(string.Format(c1ConsoleRequestsNotAllowedHtmlTemplate, iePadding));
                context.Response.End();
            }

            // https check
            if (_forceHttps && context.Request.Url.Scheme != "https")
            {
                if (!AlwaysAllowUnsecured(context.Request.Url.LocalPath) && !UserOptedOutOfHttps(context))
                {
                    context.Response.Redirect(string.Format("{0}?fallback={1}&httpsport={2}", unsecureRedirectRelativePath, _allowFallbackToHttp.ToString().ToLower(), _customHttpsPortNumber));
                }
            }

            // access check
            if (currentPath.Length > _adminRootPath.Length && !UserValidationFacade.IsLoggedIn())
            {
                if (!_allAllowedPaths.Any(p => currentPath.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
                {
                    Log.LogWarning("Authorization", "DENIED {0} access to {1}", context.Request.UserHostAddress, currentPath);
                    string redirectUrl = string.Format("{0}?ReturnUrl={1}", _loginPagePath, HttpUtility.UrlEncode(context.Request.Url.PathAndQuery, Encoding.UTF8));
                    context.Response.Redirect(redirectUrl, true);
                }
            }

            // On authenticated request make sure these resources gets compiled / launched. 
            if (ApplicationOnlineHandlerFacade.IsApplicationOnline && GlobalInitializerFacade.SystemCoreInitialized && !GlobalInitializerFacade.SystemCoreInitializing && SystemSetupFacade.IsSystemFirstTimeInitialized)
            {
                BrowserRender.EnsureReadyness();
                BuildManagerHelper.InitializeControlPreLoading();
            }
        }



        private bool UserOptedOutOfHttps(HttpContext context)
        {
            if (!_allowFallbackToHttp)
            {
                return false;
            }

            HttpCookie cookie = context.Request.Cookies["avoidc1consolehttps"];
            return cookie != null && cookie.Value == "true";
        }



        private static void LoadConfiguration()
        {
            lock (_lock)
            {
                _adminRootPath = UrlUtils.AdminRootPath.ToLowerInvariant();

                if (!_adminRootPath.EndsWith("/"))
                {
                    _adminRootPath = string.Format("{0}/", _adminRootPath);
                }
                    
                LoadAllowedPaths();
                _allowC1ConsoleRequests = true;
                LoadC1ConsoleAccessConfig();
            }
        }



        private static void LoadC1ConsoleAccessConfig()
        {
            // defaults - keeping these if config file is missing or fucked up somehow
            _forceHttps = false;
            _allowFallbackToHttp = true;
            _customHttpsPortNumber = null;

            string c1ConsoleAccessConfigPath = HostingEnvironment.MapPath(c1ConsoleAccessRelativeConfigPath);

            if (C1File.Exists(c1ConsoleAccessConfigPath))
            {
                try
                {
                    XDocument accessDoc = XDocumentUtils.Load(c1ConsoleAccessConfigPath);
                    _allowC1ConsoleRequests = _allowC1ConsoleRequests && (bool)accessDoc.Root.Attribute("enabled");

                    XElement protocolElement = accessDoc.Root.Element("ClientProtocol");
                    _forceHttps = (bool)protocolElement.Attribute("forceHttps");
                    _allowFallbackToHttp = (bool)protocolElement.Attribute("allowFallbackToHttp");

                    var customHttpsPortNumberAttrib = protocolElement.Attribute("customHttpsPortNumber");

                    if (customHttpsPortNumberAttrib != null && customHttpsPortNumberAttrib.Value.Length > 0)
                    {
                        _customHttpsPortNumber = (int)customHttpsPortNumberAttrib;
                    }
                }
                catch (Exception ex)
                {
                    Log.LogError("Authorization", "Problem parsing '{0}'. Will use defaults and allow normal access. Error was '{1}'", c1ConsoleAccessRelativeConfigPath, ex.Message);
                }
            }
        }



        private static void LoadAllowedPaths()
        {
            _allAllowedPaths.Clear();

            string webauthorizationConfigPath = HostingEnvironment.MapPath(webauthorizationRelativeConfigPath);

            if (!C1File.Exists(webauthorizationConfigPath))
            {
                Log.LogInformation("AdministrativeAuthorizationHttpModule ", "File '{0}' not found - all access to the ~/Composite folder will be blocked", webauthorizationConfigPath);
                return;
            }

            XDocument webauthorizationConfigDocument = XDocumentUtils.Load(webauthorizationConfigPath);

            XAttribute loginPagePathAttribute = Verify.ResultNotNull(webauthorizationConfigDocument.Root.Attribute("loginPagePath"), "Missing '{0}' attribute on '{1}' root element", loginPagePathAttributeName, webauthorizationRelativeConfigPath);
            string relativeLoginPagePath = Verify.StringNotIsNullOrWhiteSpace(loginPagePathAttribute.Value, "Unexpected empty '{0}' attribute on '{1}' root element", loginPagePathAttributeName, webauthorizationRelativeConfigPath);

            _loginPagePath = UrlUtils.ResolveAdminUrl(relativeLoginPagePath);

            foreach (XElement allowElement in webauthorizationConfigDocument.Root.Elements(allowElementName))
            {
                XAttribute relativePathAttribute = Verify.ResultNotNull(allowElement.Attribute(allow_pathAttributeName), "Missing '{0}' attribute on '{1}' element in '{2}'.", allow_pathAttributeName, allowElement, webauthorizationRelativeConfigPath);
                string relativePath = Verify.StringNotIsNullOrWhiteSpace(relativePathAttribute.Value, "Empty '{0}' attribute on '{1}' element in '{2}'.", allow_pathAttributeName, allowElement, webauthorizationRelativeConfigPath);

                string fullPath = UrlUtils.ResolveAdminUrl(relativePath).ToLowerInvariant();
                _allAllowedPaths.Add(fullPath);
            }
        }
    }
}