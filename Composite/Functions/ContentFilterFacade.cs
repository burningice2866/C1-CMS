using System;
using System.Collections.Generic;
using System.Linq;

using Composite.Core.Xml;

namespace Composite.Functions
{
    public static class ContentFilterFacade
    {
        private static readonly List<IContentFilter> _contentFilters;

        static ContentFilterFacade()
        {
            _contentFilters = new List<IContentFilter>();

            var asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asms)
            {
                try
                {
                    var types = asm.GetTypes()
                        .Where(t => typeof(IContentFilter).IsAssignableFrom(t) && !t.IsInterface)
                        .Select(t => (IContentFilter)Activator.CreateInstance(t));

                    _contentFilters.AddRange(types);
                }
                catch { }
            }
        }

        public static XhtmlDocument FilterContent(XhtmlDocument doc)
        {
            return FilterContent(doc, String.Empty);
        }

        public static XhtmlDocument FilterContent(XhtmlDocument doc, string id)
        {
            if (doc == null)
            {
                return null;
            }

            foreach (var filter in _contentFilters)
            {
                filter.Filter(doc, id);
            }

            return doc;
        }
    }
}
