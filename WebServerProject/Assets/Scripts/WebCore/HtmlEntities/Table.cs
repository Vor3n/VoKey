using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.HtmlEntities
{
    public class Table : HtmlDocumentElementBase
    {
        private List<TableRow> rows;

        public Table()
        {
            rows = new List<TableRow>();
            setElementTags();
        }

        public void addRow(TableRow r)
        {
            rows.Add(r);
        }

        public void setBorderWidth(int width)
        {
            if (parameters.ContainsKey("border")) parameters.Remove("border");
            parameters.Add("border", "" + width);
        }

        public override void setElementTags()
        {
            elementStart = "<table%ARGS%>";
            elementEnd = "</table>";
        }

        public override string getHtmlRepresentation()
        {
            string replacementArgs = "";
            foreach(KeyValuePair<string, string> kvp in parameters){
                Console.WriteLine(kvp.Key);
                replacementArgs += " " + kvp.Key + "=\"" + kvp.Value + "\"";
            }

            string output = base.elementStart.Replace("%ARGS%", replacementArgs);
            foreach (TableRow tr in rows)
            {
                output += tr.getHtmlRepresentation();
            }
            output += "\r\n" + elementEnd;
            return output;
        }
    }
}
