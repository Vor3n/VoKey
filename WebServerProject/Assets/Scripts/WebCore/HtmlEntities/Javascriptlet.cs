using System;

namespace WebCore.HtmlEntities
{
		public class Javascriptlet  : HtmlDocumentElementBase
		{
			public Javascriptlet (string javascriptCode)
			{
				InnerText = javascriptCode;
			}
				
			public override void setElementTags()
	        {
	            elementStart = "<script>%CONTENT%";
	            elementEnd = "</script>";
	        }
	        
	        public override string getHtmlRepresentation ()
			{
				return elementStart.Replace("%CONTENT%", InnerText) + elementEnd;
			}
		}
}

