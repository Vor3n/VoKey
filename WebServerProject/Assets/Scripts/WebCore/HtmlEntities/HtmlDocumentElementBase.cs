using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.HtmlEntities
{
    public abstract class HtmlDocumentElementBase
    {
        internal string elementStart;
        internal string elementEnd;
        internal string InnerText {
        	get; set;
        }
        /// <summary>
        /// Dictionary of the parameters that this element has.
        /// </summary>
        internal Dictionary<string, string> parameters;
        public List<HtmlDocumentElementBase> InnerElements;
        
        public virtual string ElementId {
			get {
				string s = "";
				if (parameters.ContainsKey ("id")) {
						parameters.TryGetValue ("id", out s);
				}
				return s;
			}
			set {
		            if (parameters.ContainsKey("id")) parameters.Remove("id");
			parameters.Add("id", value);
			}
        }
        
        public virtual string ElementClass {
			get {
				string s = "";
				if (parameters.ContainsKey ("class")) {
						parameters.TryGetValue ("class", out s);
				}
				return s;
			}
			set {
            if (parameters.ContainsKey("class")) parameters.Remove("class");
				parameters.Add("class", value);
			}
        }
        
		public int Width {
			get { return int.Parse(parameters["width"]); } 
			set { parameters["width"] = "" + value;  }
		}
				
		public int Height {
			get { return int.Parse(parameters["height"]); } 
			set { parameters["height"] = "" + value; }
		}
		
		public string Style {
			get {
				string s = "";
				if (parameters.ContainsKey ("style")) parameters.TryGetValue ("style", out s);
				return s;
			}
			set {
				parameters["style"] = value;
			}
		}
		protected HtmlDocumentElementBase ()
		{
			setElementTags();
			parameters = new Dictionary<string, string>	();
			InnerElements = new List<HtmlDocumentElementBase>();
		}

		public virtual void setElementTags()
        {
            throw new Exception("ElementTags not set!");
        }

        public virtual string getHtmlRepresentation()
        {
            return elementStart + elementEnd;
        }
    }

}
