using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.HtmlEntities
{
    public class TableCell : HtmlDocumentElementBase
    {
        public string Content
        {
            get;
            set;
        }

        public override void setElementTags()
        {
            base.elementStart = "<td%REPLACE%>";
            base.elementEnd = "</td>\r\n";
        }

        public TableCell()
        {
        }

        public TableCell(string content)
        {
            Content = content;
        }

        public TableCell(string content, string cssId)
        {
            Content = content;
            ElementId = cssId;
        }

        public TableCell(HtmlDocumentElementBase content)
        {
            Content = content.getHtmlRepresentation();
        }

        public TableCell(HtmlDocumentElementBase content, string cssId)
        {
            Content = content.getHtmlRepresentation();
            ElementId = cssId;
        }
        
        public override string getHtmlRepresentation()
        {
            string replacementArgs = "" + (String.IsNullOrEmpty(ElementId) ? "" : " " + ElementId);
            replacementArgs += (String.IsNullOrEmpty(ElementClass) ? "" : " " + ElementClass);
            return elementStart.Replace("%REPLACE%", replacementArgs) + Content + elementEnd;
        }
    }
}
