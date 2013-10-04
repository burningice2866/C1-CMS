using System.Web;

namespace Composite.AspNet
{
    public abstract class BrowserOverrideStore
    {
        public abstract string GetOverriddenUserAgent(HttpContext httpContext);
        public abstract void SetOverriddenUserAgent(HttpContext httpContext, string userAgent);
    }
}
