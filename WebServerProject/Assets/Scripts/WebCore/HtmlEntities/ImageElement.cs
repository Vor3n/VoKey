using System;
using System.Collections.Generic;

namespace WebCore.HtmlEntities
{
		public class ImageElement : HtmlDocumentElementBase
		{
				public ImageElement (string source, int width, int height, string alt)
				{
					parameters.Add("src", source);
					Width = width;
					Height = height;
					AltText = alt;
				}
				
				public string Source {
					get { return parameters ["src"]; } 
					set { parameters ["src"] = value; }
				}
				
				public string AltText {
					get { return parameters ["alt"]; } 
					set { parameters ["alt"] = value; }
				}
				
		        public override void setElementTags()
		        {
		            elementStart = "<img%ARGS%";
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

