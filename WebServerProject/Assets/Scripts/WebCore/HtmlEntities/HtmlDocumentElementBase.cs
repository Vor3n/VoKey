using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.HtmlEntities
{
    public abstract class HtmlDocumentElementBase
    {
        internal string elementStart;
        internal string elementEnd;
        /// <summary>
        /// Dictionary of the parameters that this element has.
        /// </summary>
        internal Dictionary<string, string> parameters;
        
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

		protected HtmlDocumentElementBase ()
		{
			parameters = new Dictionary<string, string>	();
		}

        public virtual string getHtmlRepresentation()
        {
            return elementStart + elementEnd;
        }
    }

}
