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
            base.elementStart = "<td%ARGS%>%CONTENT%";
            base.elementEnd = "</td>\r\n";
        }

        public TableCell()
        {
        }

        public TableCell(string content)
        {
            InnerText = content;
        }
        
        public int Colspan {
			get {
				 if (!parameters.ContainsKey("colspan")) return 1;
				 else return int.Parse(parameters["colspan"]);
			}
			set {
			    parameters["colspan"] = "" + value;
			}
        }

        public TableCell(string content, string cssId)
        {
            InnerText = content;
            ElementId = cssId;
        }

        public TableCell(params HtmlDocumentElementBase[] content)
        {
        	InnerElements.AddRange(content);
        }

        public TableCell(HtmlDocumentElementBase content, string cssId)
        {
            InnerElements.Add (content);
            ElementId = cssId;
        }
        
        public override string getHtmlRepresentation ()
		{
			string replacementArgs = "";
			foreach (KeyValuePair<string, string> kvp in parameters) {
				Console.WriteLine (kvp.Key);
				replacementArgs += " " + kvp.Key + "=\"" + kvp.Value + "\"";
			}
			string theInnerContent = "";
			foreach (HtmlDocumentElementBase hdeb in InnerElements) {
				theInnerContent += hdeb.getHtmlRepresentation();
			}
			if(!string.IsNullOrEmpty(InnerText)) theInnerContent += InnerText;
            return elementStart.Replace("%ARGS%", replacementArgs).Replace("%CONTENT%", theInnerContent) + elementEnd;
        }
    }
}
