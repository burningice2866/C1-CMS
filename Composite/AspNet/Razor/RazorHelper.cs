﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.WebPages;
using System.Xml;
using System.Xml.Linq;
using Composite.Core.Types;
using Composite.Core.Xml;
using Composite.Functions;

namespace Composite.AspNet.Razor
{
    /// <summary>
    /// Exposes method to execute razor pages.
    /// </summary>
    public static class RazorHelper
    {
        internal static readonly string PageContext_FunctionContextContainer = "C1.FunctionContextContainer";
        private static readonly string ExecutionLock_ItemsKey = "__razor_execute_lock__";
        private static readonly object _lock = new object();

        /// <summary>
        /// Executes the razor page.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="setParameters">Delegate to set the parameters.</param>
        /// <param name="resultType">The type of the result.</param>
        /// <param name="functionContextContainer">The function context container</param>
        /// <returns></returns>
        public static object ExecuteRazorPage(
            string virtualPath,
            Action<WebPageBase> setParameters,
            Type resultType,
            FunctionContextContainer functionContextContainer)
        {

            string output;

            WebPageBase webPage = null;
            try
            {
                webPage = WebPageBase.CreateInstanceFromVirtualPath(virtualPath);
                var startPage = StartPage.GetStartPage(webPage, "_PageStart", new[] { "cshtml" });

                object requestLock = null;
                HttpContextBase httpContext;

                HttpContext currentContext = HttpContext.Current;
                if (currentContext == null)
                {
                    httpContext = NoHttpRazorContext.GetDotNetSpecificVersion();
                }
                else
                {
                    httpContext = new HttpContextWrapper(currentContext);

                    requestLock = GetRazorExecutionLock(currentContext);
                }

                var pageContext = new WebPageContext(httpContext, webPage, startPage);
                if (functionContextContainer != null)
                {
                    pageContext.PageData.Add(PageContext_FunctionContextContainer, functionContextContainer);
                }

                if (setParameters != null)
                {
                    setParameters(webPage);
                }

                var sb = new StringBuilder();
                using (var writer = new StringWriter(sb))
                {
                    bool lockTaken = false;
                    try
                    {
                        if (requestLock != null)
                        {
                            Monitor.Enter(requestLock, ref lockTaken);
                        }

                        webPage.ExecutePageHierarchy(pageContext, writer);
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            Monitor.Exit(requestLock);
                        }
                    }
                }

                output = sb.ToString().Trim();
            }
            finally
            {
                if (webPage is IDisposable)
                {
                    (webPage as IDisposable).Dispose();
                }
            }


            if (resultType == typeof(XhtmlDocument))
            {
                if (output == "") return new XhtmlDocument();

                try
                {
                    return OutputToXhtmlDocument(output);
                }
                catch (XmlException ex)
                {
                    string[] codeLines = output.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None);

                    XhtmlErrorFormatter.EmbedSouceCodeInformation(ex, codeLines, ex.LineNumber);

                    throw;
                }
            }

            return ValueTypeConverter.Convert(output, resultType);
        }

        private static object GetRazorExecutionLock(HttpContext currentContext)
        {
            object requestLock = currentContext.Items[ExecutionLock_ItemsKey];

            if (requestLock == null)
            {
                lock (_lock)
                {
                    requestLock = currentContext.Items[ExecutionLock_ItemsKey];

                    if (requestLock == null)
                    {
                        requestLock = new object();
                        lock (currentContext.Items.SyncRoot)
                        {
                            currentContext.Items[ExecutionLock_ItemsKey] = requestLock;
                        }
                    }
                }
            }
            return requestLock;
        }

        private static XhtmlDocument OutputToXhtmlDocument(string output)
        {
            var nodes = new List<XNode>();

            using (var stringReader = new StringReader(output))
            {
                var xmlReaderSettings = new XmlReaderSettings
                {
                    IgnoreWhitespace = true,
                    DtdProcessing = DtdProcessing.Parse,
                    MaxCharactersFromEntities = 10000000,
                    XmlResolver = null,
                    ConformanceLevel = ConformanceLevel.Fragment // Allows multipe XNode-s
                };

                using (var xmlReader = XmlReader.Create(stringReader, xmlReaderSettings))
                {
                    xmlReader.MoveToContent();

                    while (!xmlReader.EOF)
                    {
                        XNode node = XNode.ReadFrom(xmlReader);
                        nodes.Add(node);
                    }
                }
            }

            if (nodes.Count == 1 && nodes[0] is XElement && (nodes[0] as XElement).Name.LocalName == "html")
            {
                return new XhtmlDocument(nodes[0] as XElement);
            }

            var document = new XhtmlDocument();
            document.Body.Add(nodes);

            return document;
        }

        /// <summary>
        /// Executes the razor page.
        /// </summary>
        /// <typeparam name="ResultType">The result type.</typeparam>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="setParameters">Delegate to set the parameters.</param>
        /// <param name="functionContextContainer">The function context container.</param>
        /// <returns></returns>
        public static ResultType ExecuteRazorPage<ResultType>(
            string virtualPath,
            Action<WebPageBase> setParameters,
            FunctionContextContainer functionContextContainer = null) where ResultType : class
        {
            return (ResultType)ExecuteRazorPage(virtualPath, setParameters, typeof(ResultType), functionContextContainer);
        }
    }


}
