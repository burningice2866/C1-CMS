using System;
using System.Web;
using Composite.Core.Extensions;
using Composite.Core.Logging;
using Composite.C1Console.Security.Foundation.PluginFacades;
using Composite.C1Console.Security.Plugins.LoginProvider;
using System.Collections.Generic;


namespace Composite.C1Console.Security
{
    /// <summary>    
    /// </summary>
    /// <exclude />
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)] 
    public static class UserValidationFacade
    {
        private static object _lock = new object();


        /// <exclude />
        public enum ValidationType 
        {
            /// <exclude />
            None = 0,

            /// <exclude />
            Form = 1,

            /// <exclude />
            Windows = 2
        };



        /// <exclude />
        public static ValidationType GetValidationType()
        {
            if (LoginProviderPluginFacade.CheckType<IFormLoginProvider>())
            {
                return ValidationType.Form;
            }
            else if (LoginProviderPluginFacade.CheckType<IWindowsLoginProvider>())
            {
                return ValidationType.Windows;
            }

            throw new InvalidOperationException(string.Format("Validation plugin '{0}' does not implement a known validation interface", LoginProviderPluginFacade.GetValidationPluginType()));
        }



        /// <exclude />
        public static IEnumerable<string> AllUsernames
        {
            get
            {
                return LoginProviderPluginFacade.AllUsernames;
            }
        }


        /// <exclude />
        public static bool CanSetUserPassword
        {
            get
            {
                return LoginProviderPluginFacade.CanSetUserPassword;
            }
        }


        /// <summary>
        /// Validates and persists a form based login
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>True if the user was validated</returns>
        public static bool FormValidateUser(string userName, string password)
        {
            LoginResult loginResult = LoginProviderPluginFacade.FormValidateUser(userName, password);

            if (loginResult == LoginResult.UserDoesNotExist)
            {
                lock (_lock)
                {
                    loginResult = LoginProviderPluginFacade.FormValidateUser(userName, password);

                    if (loginResult == LoginResult.UserDoesNotExist)
                    {
                        if (AdministratorAutoCreator.CanBeAutoCreated(userName))
                        {
                            AdministratorAutoCreator.AutoCreatedAdministrator(userName, password, "");

                            loginResult = LoginProviderPluginFacade.FormValidateUser(userName, password);
                        }
                    }
                }
            }

            if (loginResult == LoginResult.Success)
            {
                LoggingService.LogVerbose("UserValidation", String.Format("The user: [{0}], has been validated and accepted. {1}", userName, GetIpInformation()), LoggingService.Category.Audit);
                PersistUsernameInSessionDataProvider(userName);
            }
            else if(loginResult == LoginResult.IncorrectPassword)
            {
                LogLoginFailed(userName, "Incorrect password.");
            }
            else if (loginResult == LoginResult.UserDoesNotExist)
            {
                LogLoginFailed(userName, "User does not exist.");
            }
            else if (loginResult == LoginResult.PolicyViolated)
            {
                LogLoginFailed(userName, "Login policy violated.");
            }

            return loginResult == LoginResult.Success;
        }

        private static void LogLoginFailed(string userName, string message)
        {
            LoggingService.LogWarning("UserValidation", 
                                      "Login as [{0}] failed. {1} {2}".FormatWith(userName, message, GetIpInformation()),
                                      LoggingService.Category.Audit);
        }


        private static string GetIpInformation()
        {
            var httpContext = HttpContext.Current;
            if(httpContext == null)
            {
                return string.Empty;
            }

            string ipaddress = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                  
            if (String.IsNullOrWhiteSpace(ipaddress))
            {
                ipaddress = httpContext.Request.ServerVariables["REMOTE_ADDR"]; 
            }

            if (String.IsNullOrWhiteSpace(ipaddress))
            {
                return string.Empty;
            }

            return " IP address: " + ipaddress;
        }


        /// <exclude />
        public static bool FormValidateUserWithoutLogin(string userName, string password)
        {
            return LoginProviderPluginFacade.FormValidateUser(userName, password) == LoginResult.Success;
        }



        /// <exclude />
        public static void FormSetUserPassword(string userName, string password)
        {
            LoginProviderPluginFacade.FormSetUserPassword(userName, password);
        }


        /// <summary>
        /// Validates and persists a windows based login. Actual Windows username / password validation should precede this call.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="domainName"></param>
        /// <returns>True if the user was validated</returns>
        public static bool WindowsValidateUser(string userName, string domainName)
        {
            bool userIsValidated = LoginProviderPluginFacade.WindowsValidateUser(userName, domainName);


            if (userIsValidated)
            {
                PersistUsernameInSessionDataProvider(userName);
            }

            return userIsValidated;
        }


        /// <summary>
        /// Flushes the username from the login session
        /// </summary>
        public static void Logout()
        {
            LoginSessionStorePluginFacade.FlushUsername();
        }



        /// <exclude />
        public static bool IsLoggedIn()
        {
            return (string.IsNullOrEmpty(LoginSessionStorePluginFacade.StoredUsername) == false);
        }



        /// <exclude />
        public static UserToken GetUserToken()
        {
            if (IsLoggedIn() == false) throw new InvalidOperationException("No user has been logged in");

            return new UserToken(LoginSessionStorePluginFacade.StoredUsername);
        }



        /// <exclude />
        public static string GetUsername()
        {
            if (IsLoggedIn() == false) throw new InvalidOperationException("No user has been logged in");

            return LoginSessionStorePluginFacade.StoredUsername;
        }



        private static void PersistUsernameInSessionDataProvider(string userName)
        {
            LoginSessionStorePluginFacade.StoreUsername(userName, false);
        }
    }
}