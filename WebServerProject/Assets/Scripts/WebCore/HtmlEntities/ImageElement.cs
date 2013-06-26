using System;
using System.Collections.Generic;

namespace WebCore.HtmlEntities
{
		public class ImageElement : HtmlDocumentElementBase
		{
				public ImageElement (string source, int width, int height, string alt)
				{
					setElementTags();
					parameters.Add("src", source);
					parameters.Add("width", "" + width);
					parameters.Add("height", "" + height);
					parameters.Add("alt", alt);
				}
				
				public int Width {
					get { return int.Parse(parameters["width"]); } 
					set { parameters["width"] = "" + value;  }
				}
				
				public int Height {
					get { return int.Parse(parameters["height"]); } 
					set { parameters["height"] = "" + value; }
				}
				
				public string Source {
					get { return parameters ["src"]; } 
					set { parameters ["src"] = value; }
				}
				
				public string AltText {
					get { return parameters ["src"]; } 
					set { parameters ["src"] = value; }
				}
				
		        private void setElementTags()
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

