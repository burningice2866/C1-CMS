﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Composite.AspNet;
using Composite.Core.Localization;
using Composite.Core.WebClient.Renderings.Page;
using Composite.Core.Xml;
using Composite.Functions;
using Composite.Plugins.PageTemplates.MasterPages.Controls.Rendering;

namespace Composite.Plugins.PageTemplates.MasterPages.Controls.Functions
{
    /// <exclude />
    [ParseChildren(false)]
    public class Markup : Control
    {
        private readonly FunctionContextContainer _functionContextContainer;

        /// <exclude />
        protected XElement InnerContent { get; set; }

        /// <exclude />
        public Markup() { }

        /// <exclude />
        public Markup(XElement content, FunctionContextContainer functionContextContainer)
        {
            if(content.Name.LocalName == "html")
            {
                InnerContent = content;
            }
            else
            {
                var document = new XhtmlDocument();
                document.Body.Add(content);

                InnerContent = document.Root;
            }

            _functionContextContainer = functionContextContainer;
        }

        /// <exclude />
        protected override void OnInit(EventArgs e)
        {
            EnsureChildControls();

            base.OnInit(e);
        }

        /// <exclude />
        protected override void CreateChildControls()
        {
            if (InnerContent == null)
            {
                ProcessInternalControls();
            }

            if (InnerContent != null)
            {
                var functionContextContainer = _functionContextContainer;
                if (functionContextContainer == null && this.NamingContainer is UserControlFunction)
                {
                    var containerFunction = this.NamingContainer as UserControlFunction;
                    functionContextContainer = containerFunction.FunctionContextContainer;
                } 
                
                if (functionContextContainer == null)
                {
                    functionContextContainer = PageRenderer.GetPageRenderFunctionContextContainer();
                } 
                var controlMapper = (IXElementToControlMapper) functionContextContainer.XEmbedableMapper;

                PageRenderer.ExecuteEmbeddedFunctions(InnerContent, functionContextContainer);

                var xhmlDocument = new XhtmlDocument(InnerContent);

                PageRenderer.NormalizeXhtmlDocument(xhmlDocument);
                PageRenderer.ResolveRelativePaths(xhmlDocument);

                if (PageRenderer.CurrentPage != null)
                {
                    PageRenderer.ResolvePageFields(xhmlDocument, PageRenderer.CurrentPage);
                }

                NormalizeAspNetForms(xhmlDocument);

                ContentFilterFacade.FilterContent(xhmlDocument, ID);

                AddNodesAsControls(xhmlDocument.Body.Nodes(), this, controlMapper);

                if (Page.Header != null)
                {
                    MergeHeadSection(xhmlDocument, Page.Header, controlMapper);
                }
            }

            base.CreateChildControls();
        }


        private void MergeHeadSection(XhtmlDocument xhtmlDocument, HtmlHead headControl, IXElementToControlMapper controlMapper)
        {
            xhtmlDocument.MergeToHeadControl(headControl, controlMapper);

            // handling custom master page head control locally - removing it if a generic meta description tag was in the document
            if (headControl.Controls.OfType<HtmlMeta>().Any(f=>f.Name=="description"))
            {
                var existingDescriptionMetaTag = headControl.Controls.OfType<DescriptionMetaTag>().FirstOrDefault();
                if (existingDescriptionMetaTag != null)
                {
                    headControl.Controls.Remove(existingDescriptionMetaTag);
                }
            }
        }

        
        private void NormalizeAspNetForms(XhtmlDocument xhtmlDocument)
        {
            // If current control is inside <form id="" runat="server"> tag all <asp:forms> tags will be removed from placeholder

            bool isInsideAspNetForm = false;

            var ansestor = this.Parent;
            while (ansestor != null)
            {
                if (ansestor is HtmlForm)
                {
                    isInsideAspNetForm = true;
                    break;
                }

                ansestor = ansestor.Parent;
            }

            if (!isInsideAspNetForm)
            {
                return;
            }

            List<XElement> aspNetFormElements = xhtmlDocument.Descendants(Namespaces.AspNetControls + "form").Reverse().ToList();

            foreach (XElement aspNetFormElement in aspNetFormElements)
            {
                aspNetFormElement.ReplaceWith(aspNetFormElement.Nodes());
            }
        }

        private void ProcessInternalControls()
        {
            string str = null;

            if (Controls.Count > 0)
            {
                var content = Controls[0] as LiteralControl;
                if (content != null)
                {
                    str = content.Text;
                }
            }

            if (!String.IsNullOrEmpty(str))
            {
                Controls.Clear();

                InnerContent = new XElement(Namespaces.Xhtml + "html",
                    new XAttribute(XNamespace.Xmlns + "f", Namespaces.Function10),
                    new XAttribute(XNamespace.Xmlns + "lang", LocalizationXmlConstants.XmlNamespace),
                        new XElement(Namespaces.Xhtml + "head"),
                        new XElement(Namespaces.Xhtml + "body", XElement.Parse(str)));
            }
        }

        private static void AddNodesAsControls(IEnumerable<XNode> nodes, Control parent, IXElementToControlMapper mapper)
        {
            foreach (var node in nodes)
            {
                var c = node.AsAspNetControl(mapper);
                parent.Controls.Add(c);
            }
        }
    }
}
