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

        public void setElementTags()
        {
            base.elementStart = "<td%REPLACE%>";
            base.elementEnd = "</td>\r\n";
        }

        public TableCell()
        {
            setElementTags();
        }

        public TableCell(string content)
        {
            setElementTags();
            Content = content;
        }

        public TableCell(string content, string cssId)
        {
            setElementTags();
            Content = content;
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
