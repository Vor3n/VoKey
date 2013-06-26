using System;
using System.Collections.Generic;

namespace WebCore.HtmlEntities
{
	public class FormButton : HtmlDocumentElementBase
	{
		public FormButton (string content, string onclick)
		{
			InnerText = content;
			OnClick = onclick;
		}
		
		public string OnClick {
			get { return parameters ["onClick"]; } 
			set { parameters ["onClick"] = value; }
		}
		
		public override void setElementTags ()
		{
			elementStart = "<button %ARGS%>";
			elementEnd = "</button>";
		}
		
		public override string getHtmlRepresentation()
        {
    		string replacementArgs = "";
            foreach(KeyValuePair<string, string> kvp in parameters){
                replacementArgs += " " + kvp.Key + "=\"" + kvp.Value + "\"";
            }
            return elementStart.Replace("%ARGS%", replacementArgs) + InnerText + elementEnd;
        }
	}
}

