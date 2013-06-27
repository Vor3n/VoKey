using System;
using System.Collections.Generic;

namespace WebCore.HtmlEntities
{
	public class CssLinkElement : HtmlDocumentElementBase
	{
		public CssLinkElement (string source)
		{
			setElementTags();
			parameters.Add("href", source);
			parameters.Add("rel", "stylesheet");
			parameters.Add("type", "text/css");
		}
		
		public string Source {
			get { return parameters ["href"]; } 
			set { parameters ["href"] = value; }
		}
		
        public override void setElementTags()
        {
            elementStart = "<link %ARGS%";
            elementEnd = "/>";
        }
        
		public override string getHtmlRepresentation()
        {
    		string replacementArgs = "";
            foreach(KeyValuePair<string, string> kvp in parameters){
                Console.WriteLine(kvp.Key);
                replacementArgs += " " + kvp.Key + "=\"" + kvp.Value + "\"";
            }
            return elementStart.Replace("%ARGS%", replacementArgs) + elementEnd;
        }
	}
}

