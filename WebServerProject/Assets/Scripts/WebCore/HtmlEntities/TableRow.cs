using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore.HtmlEntities
{
    public class TableRow : HtmlDocumentElementBase
    {
        private List<TableCell> cellList;
        public int Count
        {
            get
            {
                return cellList.Count;
            }
        }

        public override void setElementTags()
        {
            elementStart = "<tr%REPLACE%>\r\n";
            elementEnd = "</tr>\r\n";
        }

        /// <summary>
        /// Constructor for a table row.
        /// </summary>
        public TableRow()
        {
        }

        public TableRow(params TableCell[] cells)
        {
            cellList = new List<TableCell>();
            cellList.AddRange(cells);
        }

        public TableRow(string cssId, params TableCell[] cells)
        {
            ElementClass = cssId;
            cellList = new List<TableCell>();
            cellList.AddRange(cells);
        }

        /// <summary>
        /// Adds a Cell to this TableRow
        /// </summary>
        /// <param name="cell"></param>
        public void AddCell(TableCell cell)
        {
            cellList.Add(cell);
        }

        /// <summary>
        /// Removes the Cell at this position in the TableRow.
        /// </summary>
        /// <param name="position"></param>
        public void RemoveCell(int position)
        {
            cellList.RemoveAt(position);
        }

        /// <summary>
        /// Removes the specified cell from the TableRow.
        /// </summary>
        /// <param name="cell"></param>
        public void RemoveCell(TableCell cell)
        {
            cellList.Remove(cell);
        }

        public override string getHtmlRepresentation()
        {
            string replacementArgs = "" + (String.IsNullOrEmpty(ElementId) ? "" : " id=\"" + ElementId + "\"");
            replacementArgs += (String.IsNullOrEmpty(ElementClass) ? "" : " class=\"" + ElementClass + "\"");

            string output = base.elementStart.Replace("%REPLACE%", replacementArgs);
            foreach (TableCell tc in cellList)
            {
                output += "\t" + tc.getHtmlRepresentation();
            }
            output += "\r\n" + elementEnd;
            return output;
        }
    }
}
