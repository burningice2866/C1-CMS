using Composite.Core.Xml;

namespace Composite.Functions
{
    public interface IContentFilter
    {
        void Filter(XhtmlDocument document, string id);
    }
}
