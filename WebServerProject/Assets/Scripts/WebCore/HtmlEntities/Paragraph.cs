using System;
using System.Collections.Generic;

namespace WebCore.HtmlEntities
{
	public class Paragraph : HtmlDocumentElementBase
	{
		public Paragraph (string text)
		{
			InnerText = text;
		}
		
		public override void setElementTags()
        {
            elementStart = "<p %ARGS%>%CONTENT%";
            elementEnd = "</p>";
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

