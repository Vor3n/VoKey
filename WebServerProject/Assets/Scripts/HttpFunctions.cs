using System;
using System.Net;
using System.Text;

namespace AssemblyCSharp
{
    public static class HttpFunctions
    {
        /// <summary>
        /// Sends a standard response.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="content">Content.</param>
        public static void sendStandardResponse(HttpListenerContext request, byte[] content)
        {
            sendStandardResponse(request, content, 200);
        }

        /// <summary>
        /// Sends a standard response.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="content">Content.</param>
        public static void sendStandardResponse(HttpListenerContext request, byte[] content, int requestCode)
        {
            Console.WriteLine("sendStandardResponse start");
            request.Response.ContentLength64 = content.Length;
            request.Response.OutputStream.Write(content, 0, content.Length);
            Console.WriteLine("sendStandardResponse stop");
        }

        /// <summary>
        /// Sends a standard response.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="content">Content.</param>
        public static void sendStandardResponse(HttpListenerContext request, string text, int requestCode)
        {
            Console.WriteLine("sendStandardResponse start");
            byte[] content = Encoding.UTF8.GetBytes(text);
            request.Response.ContentLength64 = content.Length;
            request.Response.OutputStream.Write(content, 0, content.Length);
            Console.WriteLine("sendStandardResponse stop");
        }

        public static void appendSessionHeaderToResponse(HttpListenerContext hlc, string sessionHash)
        {
            hlc.Response.AppendHeader("session", sessionHash);
        }
        /// <summary>
        /// Sends a file and uses the provided content type.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="data">Data.</param>
        public static void sendFileWithContentType(HttpListenerContext request, string contentType, byte[] data)
        {
            Console.WriteLine("sendFileWithContentType start");
            request.Response.ContentType = contentType;
            request.Response.ContentLength64 = data.Length;
            request.Response.OutputStream.Write(data, 0, data.Length);
            Console.WriteLine("sendFileWithContentType stop");
        }

        /// <summary>
        /// Returns an XML String to a HTTP Client
        /// </summary>
        /// <param name="request"></param>
        /// <param name="xmlString"></param>
        public static void returnXmlStringToHttpClient(HttpListenerContext request, string xmlString)
        {
            byte[] returnBytes;
            try
            {
                returnBytes = Encoding.UTF8.GetBytes(xmlString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException());
                returnBytes = Encoding.UTF8.GetBytes("No data in response");
            }
            sendStandardResponse(request, returnBytes);
        }

        /// <summary>
        /// Handles an unknown request.
        /// </summary>
        /// <param name="request">Request.</param>
        public static void handleUnknownRequest(HttpListenerContext request, string message)
        {
            string data = "<HTML>" +
                "<HEAD><TITLE>Vokey Server</TITLE></HEAD>" +
            "<BODY>I regret to inform you that the request that you have submitted has not yet been implemented. " +
            "If any additional information was provided by the AssetServer it will be displayed below: <br />{0}</BODY></HTML>";
            string.Format(data, message);
            sendStandardResponse(request, Encoding.UTF8.GetBytes(data), 501);
        }

        /// <summary>
        /// Handles a server exception.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="e"></param>
        public static void handleServerException(HttpListenerContext request, Exception e)
        {
            string data = "<HTML>" +
                "<HEAD><TITLE>Vokey Server</TITLE></HEAD>" +
            "<BODY>I regret to inform you that your request caused the server to cry like a little girl. " +
            "Details about what went wrong between you two are the following: <br />Base Exception:<br />{0}<br /><br />Message:<br />{1}</BODY></HTML>";
            data = string.Format(data, e.GetBaseException().ToString(), e.Message);
            sendStandardResponse(request, Encoding.UTF8.GetBytes(data), 500);
        }

        /// <summary>
        /// Sends the text response.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="message">Message.</param>
        public static void sendTextResponse(HttpListenerContext request, string message)
        {
            sendTextResponse(request, message, 200);
        }

        /// <summary>
        /// Sends the text response.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="message">Message.</param>
        public static void sendTextResponse(HttpListenerContext request, string message, int errorCode)
        {
            sendStandardResponse(request, Encoding.UTF8.GetBytes(string.Format("<HTML><BODY>Vokey Server<br>" + DateTime.Now.ToShortTimeString() + " {0}<br />" + message + "</BODY></HTML>", DateTime.Now)), errorCode);
        }
    }
}

