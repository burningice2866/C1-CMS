using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

using Composite.Data;
using Composite.Data.Types;

namespace Composite.Core.Application
{
    public static class SpecialModesFileResolver
    {
        public static string ResolveTemplate(string rootDirectory, IPage page, string template, string extension, HttpRequest request)
        {
            var dirsToTry = new List<string>
            {
                rootDirectory
            };

            using (var data = new DataConnection())
            {
                var siteId = data.SitemapNavigator.GetPageNodeById(page.Id).GetPageIds(SitemapScope.Level1).First().ToString();
                dirsToTry.Insert(0, Path.Combine(rootDirectory, siteId));

                foreach (var dir in dirsToTry)
                {
                    var specialTemplate = ResolveFileInInDirectory(dir, template, extension, request.Browser.IsMobileDevice, request.QueryString);
                    if (specialTemplate != null)
                    {
                        return specialTemplate;
                    }
                }
            }

            return null;
        }

        public static string ResolveFileInInDirectory(string directory, string file, string extension, bool isMobile, NameValueCollection qs)
        {
            var pathProvider = HostingEnvironment.VirtualPathProvider;
            var specielModes = new List<string>();

            if (isMobile)
            {
                specielModes.Add("mobile");
            }

            if (qs.AllKeys.Length > 0 && qs.Keys[0] == null && qs[0] == "print")
            {
                specielModes.Add("print");
            }

            foreach (var mode in specielModes)
            {
                var specialFile = Path.Combine(directory, String.Format("{0}_{1}{2}", file, mode, extension));
                if (pathProvider.FileExists(specialFile))
                {
                    return specialFile;
                }

                specialFile = Path.Combine(directory, mode + extension);
                if (pathProvider.FileExists(specialFile))
                {
                    return specialFile;
                }
            }

            file = Path.Combine(directory, file + extension);
            if (pathProvider.FileExists(file))
            {
                return file;
            }

            return null;
        }
    }
}
