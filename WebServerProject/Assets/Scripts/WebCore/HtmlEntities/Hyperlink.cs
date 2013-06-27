using System;
using System.Collections.Generic;

namespace WebCore.HtmlEntities
{
	public class Hyperlink : HtmlDocumentElementBase
	{
		private string innerText;
		public Hyperlink (string url, string content)
		{
			InnerText = content;
			Source = url;
		}
		
		public Hyperlink (string url, string onClick, string content)
		{
			OnClick = onClick;
			InnerText = content;
			Source = url;
		}
		
		public Hyperlink(string url, params HtmlDocumentElementBase[] objects){
			Source = url;
			InnerElements.AddRange(objects);
		}
		
		public Hyperlink(string url, string onClick, params HtmlDocumentElementBase[] objects){
			OnClick = onClick;
			Source = url;
			InnerElements.AddRange(objects);
		}
		
		public string Source {
			get { return parameters ["href"]; } 
			set { parameters ["href"] = value; }
		}
		
		public string OnClick {
			get { return parameters ["onClick"]; } 
			set { parameters ["onClick"] = value; }
		}
		
        public override void setElementTags()
        {
            elementStart = "<a %ARGS%>%CONTENT%";
            elementEnd = "</a>";
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

