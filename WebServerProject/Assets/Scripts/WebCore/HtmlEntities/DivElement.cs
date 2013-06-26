using System;
using System.Collections.Generic;

namespace WebCore.HtmlEntities
{
	public class DivElement : HtmlDocumentElementBase
	{
		public DivElement (string content)
		{
			InnerText = content;
		}
	
		public DivElement (params HtmlDocumentElementBase[] objects)
		{
			InnerElements.AddRange (objects);
		}
		
		public override void setElementTags ()
		{
			elementStart = "<div %ARGS%>%CONTENT%";
			elementEnd = "</div>";
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

