using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.WebPages;

using Composite.Data;
using Composite.Data.Types;

namespace Composite.Core.Application
{
    /// <summary>
    /// 
    /// </summary>
    public static class SpecialModesFileResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="page"></param>
        /// <param name="template"></param>
        /// <param name="extension"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string ResolveTemplate(string rootDirectory, IPage page, string template, string extension, HttpContextBase context)
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
                    var specialTemplate = ResolveFileInInDirectory(dir, template, extension, context);
                    if (specialTemplate != null)
                    {
                        return specialTemplate;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="file"></param>
        /// <param name="extension"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string ResolveFileInInDirectory(string directory, string file, string extension, HttpContextBase context)
        {
            var pathProvider = HostingEnvironment.VirtualPathProvider;
            var modes = DisplayModeProvider.Instance.GetAvailableDisplayModesForContext(context, null);

            foreach (var mode in modes)
            {
                var specialFile = Path.Combine(directory, String.Format("{0}{1}", file, extension));

                var displayInfo = mode.GetDisplayInfo(context, specialFile, pathProvider.FileExists);
                if (displayInfo != null)
                {
                    return displayInfo.FilePath;
                }
            }

            file = Path.Combine(directory, file + extension);
            
            return pathProvider.FileExists(file) ? file : null;
        }
    }
}