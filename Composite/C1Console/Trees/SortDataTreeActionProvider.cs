using System.Collections.Generic;
using System.Xml.Linq;

using Composite.C1Console.Trees.Foundation;

namespace Composite.C1Console.Trees
{
    public class SortDataTreeActionProvider : ITreeActionProvider
    {
        private const string Icon = "customurlaction-defaulticon";

        public XNamespace Namespace => XNamespace.Get("https://bitbucket.org/burningice/compositec1contrib");

        public XName Name => Namespace + "SortAction";

        public ActionNode BuildNode(XElement element, Tree tree)
        {
            var actionNode = new CustomUrlActionNode();
            ActionNodeCreatorFactory.InitializeWithCommonValue(element, tree, actionNode, Icon);

            var type = element.Attribute("Type");

            var url = $"~/Composite/InstalledPackages/CompositeC1Contrib.Sorting/Sort.aspx?type={type.Value}";

            if (element.HasElements)
            {
                var filter = element.Element(TreeMarkupConstants.Namespace + "Filter");
                if (filter != null)
                {
                    var filterField = filter.Attribute("Field");
                    var filterValue = filter.Attribute("Value");

                    if (filterField != null && filterValue != null)
                    {
                        url += $"&amp;filter={filterField.Value}%3D${filterValue.Value}";
                    }
                }
            }

            actionNode.Url = url;

            actionNode.ViewLabel = actionNode.Label;
            actionNode.ViewToolTip = actionNode.ToolTip;
            actionNode.ViewIcon = FactoryHelper.GetIcon(Icon);
            actionNode.ViewType = CustomUrlActionNodeViewType.DocumentView;
            actionNode.PostParameters = new Dictionary<string, string>();

            return actionNode;
        }
    }
}
