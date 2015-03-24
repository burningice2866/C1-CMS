using System.Xml.Linq;

namespace Composite.C1Console.Trees
{
    public interface ITreeActionProvider
    {
        XNamespace Namespace { get; }
        XName Name { get; }
        ActionNode BuildNode(XElement element, Tree tree);
    }
}
