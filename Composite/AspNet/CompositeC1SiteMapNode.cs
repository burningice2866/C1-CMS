using System;
using System.Globalization;
using System.Web;
using Composite.Core.Routing;
using Composite.Data.Types;

namespace Composite.AspNet
{
    /// <exclude />
    [Obsolete("Use CmsPageSiteMapNode directly instead")]
    public abstract class CompositeC1SiteMapNode : SiteMapNode
    {
        /// <exclude />
        public abstract CultureInfo Culture { get; protected set; }

        /// <exclude />
        public abstract int? Priority { get; protected set; }

        /// <exclude />
        public abstract IPage Page { get; protected set; }

        /// <exclude />
        public abstract int Depth { get; protected set; }

        /// <exclude />
        public abstract DateTime LastModified { get; protected set; }

        /// <exclude />
        public abstract SiteMapNodeChangeFrequency? ChangeFrequency { get; protected set; }

        /// <exclude />
        public abstract string DocumentTitle { get; protected set; }

        /// <exclude />
        protected CompositeC1SiteMapNode(SiteMapProvider provider, IPage page) : base(provider, page.Id.ToString(), PageUrls.BuildUrl(page), page.MenuTitle, page.Description) { }


        /// <exclude />
        public bool Equals(CompositeC1SiteMapNode obj)
        {
            return Key == obj.Key && Culture.Equals(obj.Culture);
        }

        /// <exclude />
        public override bool Equals(object obj)
        {
            var pageSiteMapNode = obj as CompositeC1SiteMapNode;
            if (pageSiteMapNode != null)
            {
                return Equals(pageSiteMapNode);
            }

            return base.Equals(obj);
        }

        /// <exclude />
        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ Culture.GetHashCode();
        }

        /// <exclude />
        public static bool operator ==(CompositeC1SiteMapNode a, CompositeC1SiteMapNode b)
        {
            if (Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if ((object)a == null || (object)b == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <exclude />
        public static bool operator !=(CompositeC1SiteMapNode a, CompositeC1SiteMapNode b)
        {
            return !(a == b);
        }
    }
}
