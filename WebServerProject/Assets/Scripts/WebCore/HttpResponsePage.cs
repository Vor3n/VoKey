using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore
{
    public class HttpResponsePage
    {
        private const string html0_htmlHeadStart = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n";
        private const string html1_htmlCssEntry = "";
        private const string html1_htmlJavascriptEntry = "<script src=\"%REPLACE%\"></script>\r\n";
        private const string html1_htmlMetaNameDescEntry = "<meta name=\"%RELACENAME%\" content=\"%REPLACEVALUE%\">\r\n";
        private const string html1_htmlMetaCharSetEntry = "<meta charset=\"%REPLACEME%\">\r\n";
        private const string html1_htmlMetaHeaderEntry = "<meta http-equiv=\"%REPLACENAME%\" content=\"%REPLACEVALUE%\">\r\n";
        private const string html1_titleEntry = "<title>%REPLACE%</title>\r\n";
        private const string html2_headEnd = "</head>\r\n";
        private const string html3_bodyStart = "<body%REPLACEMEONLOAD%>";
        private const string html5_bodyEnd = "</body>\r\n</html>";

        private const string html_util_rn = "\r\n";

        private List<WebCore.HtmlEntities.HtmlDocumentElementBase> contentList;
        private List<WebCore.HtmlEntities.HtmlDocumentElementBase> headList;

        private string _title;

        public HttpResponsePage(string title)
        {
            _title = title;
            contentList = new List<HtmlEntities.HtmlDocumentElementBase>();
            headList = new List<HtmlEntities.HtmlDocumentElementBase>();
        }
		
		public void AddElementToHead(WebCore.HtmlEntities.HtmlDocumentElementBase content)
        {
            headList.Add(content);
        }
		
        public void AddElementToBody(WebCore.HtmlEntities.HtmlDocumentElementBase content)
        {
            contentList.Add(content);
        }

        public string getHtmlRepresentation()
        {
            string replacedTitle = html1_titleEntry.Replace("%REPLACE%", _title);
            string replacedBodyArgs = "";
            string replacedBody = html3_bodyStart.Replace("%REPLACEMEONLOAD%", replacedBodyArgs);
            string output = html0_htmlHeadStart + replacedTitle;
            foreach(WebCore.HtmlEntities.HtmlDocumentElementBase o in headList){
                output += o.getHtmlRepresentation();
            }
             output += html2_headEnd + replacedBody + html_util_rn;

            //Add elements to body
            foreach(WebCore.HtmlEntities.HtmlDocumentElementBase o in contentList){
                output += o.getHtmlRepresentation();
            }
               output += html5_bodyEnd;

            return output;
        }
    }
}
