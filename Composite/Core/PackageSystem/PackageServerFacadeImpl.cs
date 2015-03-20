﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using Composite.Core.PackageSystem.Foundation;
using Composite.Core.PackageSystem.WebServiceClient;


namespace Composite.Core.PackageSystem
{
    internal sealed class PackageServerFacadeImpl : IPackageServerFacade
    {
        private readonly PackageServerFacadeImplCache _packageServerFacadeImplCache = new PackageServerFacadeImplCache();


        public ServerUrlValidationResult ValidateServerUrl(string packageServerUrl)
        {
            try
            {
                var basicHttpBinding = new BasicHttpBinding { MaxReceivedMessageSize = int.MaxValue };
                basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;

                var client = new PackagesSoapClient(basicHttpBinding, new EndpointAddress(string.Format("https://{0}", packageServerUrl)));

                client.IsOperational();
                return ServerUrlValidationResult.Https;
            }
            catch (Exception)
            {
            }

            try
            {
                var basicHttpBinding = new BasicHttpBinding { MaxReceivedMessageSize = int.MaxValue };
                var client = new PackagesSoapClient(basicHttpBinding, new EndpointAddress(string.Format("http://{0}", packageServerUrl)));

                client.IsOperational();
                return ServerUrlValidationResult.Http;
            }
            catch (Exception)
            {
            }

            return ServerUrlValidationResult.Invalid;
        }



        public IEnumerable<PackageDescription> GetPackageDescriptions(string packageServerUrl, Guid installationId, CultureInfo userCulture)
        {
            List<PackageDescription> packageDescriptions = _packageServerFacadeImplCache.GetCachedPackageDescription(packageServerUrl, installationId, userCulture);
            if (packageDescriptions != null) return packageDescriptions;

            PackageDescriptor[] packageDescriptors = null;
            try
            {
                PackagesSoapClient client = CreateClient(packageServerUrl);

                packageDescriptors = client.GetPackageList(installationId, userCulture.ToString());
            }
            catch (Exception ex)
            {
                Log.LogError("PackageServerFacade", ex);
            }

            packageDescriptions = new List<PackageDescription>();
            if (packageDescriptors != null)
            {
                foreach (PackageDescriptor packageDescriptor in packageDescriptors)
                {
                    if (ValidatePackageDescriptor(packageDescriptor))
                    {
                        packageDescriptions.Add(new PackageDescription
                        {
                            PackageFileDownloadUrl = packageDescriptor.PackageFileDownloadUrl,
                            PackageVersion = packageDescriptor.PackageVersion,
                            Description = packageDescriptor.Description,
                            EulaId = packageDescriptor.EulaId,
                            GroupName = packageDescriptor.GroupName,
                            Id = packageDescriptor.Id,
                            InstallationRequireLicenseFileUpdate = packageDescriptor.InstallationRequireLicenseFileUpdate,
                            IsFree = packageDescriptor.IsFree,
                            IsTrial = packageDescriptor.IsTrial,
                            LicenseRuleId = packageDescriptor.LicenseId,
                            MaxCompositeVersionSupported = packageDescriptor.MaxCompositeVersionSupported,
                            MinCompositeVersionSupported = packageDescriptor.MinCompositeVersionSupported,
                            Name = packageDescriptor.Name,
                            PriceAmmount = packageDescriptor.PriceAmmount,
                            PriceCurrency = packageDescriptor.PriceCurrency,
                            ReadMoreUrl = packageDescriptor.ReadMoreUrl,
                            TechicalDetails = packageDescriptor.TechicalDetails,
                            TrialPeriodDays = packageDescriptor.TrialPeriodDays,
                            UpgradeAgreementMandatory = packageDescriptor.UpgradeAgreementMandatory,
                            Vendor = packageDescriptor.Author
                        });
                    }
                }

                _packageServerFacadeImplCache.AddCachedPackageDescription(packageServerUrl, installationId, userCulture, packageDescriptions);
            }            

            return packageDescriptions;
        }



        public string GetEulaText(string packageServerUrl, Guid eulaId, CultureInfo userCulture)
        {
            PackagesSoapClient client = CreateClient(packageServerUrl);

            string eulaText = client.GetEulaText(eulaId, userCulture.ToString());

            return eulaText;
        }



        public Stream GetInstallFileStream(string packageFileDownloadUrl)
        {
            Log.LogVerbose("PackageServerFacade", string.Format("Downloading file: {0}", packageFileDownloadUrl));

            var client = new System.Net.WebClient();
            return client.OpenRead(packageFileDownloadUrl);
        }



        public void RegisterPackageInstallationCompletion(string packageServerUrl, Guid installationId, Guid packageId, string localUserName, string localUserIp)
        {
            PackagesSoapClient client = CreateClient(packageServerUrl);

            client.RegisterPackageInstallationCompletion(installationId, packageId, localUserName, localUserIp);
        }



        public void RegisterPackageInstallationFailure(string packageServerUrl, Guid installationId, Guid packageId, string localUserName, string localUserIp, string exceptionString)
        {
            PackagesSoapClient client = CreateClient(packageServerUrl);

            client.RegisterPackageInstallationFailure(installationId, packageId, localUserName, localUserIp, exceptionString);
        }



        public void RegisterPackageUninstall(string packageServerUrl, Guid installationId, Guid packageId, string localUserName, string localUserIp)
        {
            PackagesSoapClient client = CreateClient(packageServerUrl);

            client.RegisterPackageUninstall(installationId, packageId, localUserName, localUserIp);
        }



        public void ClearCache()
        {
            _packageServerFacadeImplCache.Clear();
        }



        private bool ValidatePackageDescriptor(PackageDescriptor packageDescriptor)
        {
            string newVersion;
            if (!VersionStringHelper.ValidateVersion(packageDescriptor.PackageVersion, out newVersion))
            {
                Log.LogWarning("PackageServerFacade", string.Format("The package '{0}' ({1}) did not validate and is skipped", packageDescriptor.Name, packageDescriptor.Id));
                return false;
            }

            packageDescriptor.PackageVersion = newVersion;

            if (!VersionStringHelper.ValidateVersion(packageDescriptor.MinCompositeVersionSupported, out newVersion))
            {
                Log.LogWarning("PackageServerFacade", string.Format("The package '{0}' ({1}) did not validate and is skipped", packageDescriptor.Name, packageDescriptor.Id));
                return false;
            }

            packageDescriptor.MinCompositeVersionSupported = newVersion;

            if (!VersionStringHelper.ValidateVersion(packageDescriptor.MaxCompositeVersionSupported, out newVersion))
            {
                Log.LogWarning("PackageServerFacade", string.Format("The package '{0}' ({1}) did not validate and is skipped", packageDescriptor.Name, packageDescriptor.Id));
                return false;
            }

            packageDescriptor.MaxCompositeVersionSupported = newVersion;

            return true;
        }
      


        private PackagesSoapClient CreateClient(string packageServerUrl)
        {
            var timeout = TimeSpan.FromMinutes(RuntimeInformation.IsDebugBuild ? 2 : 1);

            var basicHttpBinding = new BasicHttpBinding
            {
                CloseTimeout = timeout,
                OpenTimeout = timeout,
                ReceiveTimeout = timeout,
                SendTimeout = timeout
            };

            if (packageServerUrl.StartsWith("https"))
            {
                basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
            }

            basicHttpBinding.MaxReceivedMessageSize = int.MaxValue;

            return new PackagesSoapClient(basicHttpBinding, new EndpointAddress(packageServerUrl));
        }
    }
}
