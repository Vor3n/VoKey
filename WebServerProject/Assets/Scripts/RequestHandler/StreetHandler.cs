using System;
using System.Net;
using VokeySharedEntities;
using System.Collections.Generic;
using Vokey;

namespace AssemblyCSharp
{
    public class StreetHandler : RequestHandler
    {
        private static string[] acceptableCommands = { "street" };

        private Town theTownToProcess = null;

        public StreetHandler(HttpListenerContext hlc)
            : base(hlc, acceptableCommands)
        {
        }

        public void setTown(Town t)
        {
            theTownToProcess = t;
        }

        public override void handleSimpleRequest(string action)
        {
            if (theTownToProcess == null)
            {
                throw new Exception("No town specified to get the streets from!<br />" + getHtmlFormattedListOfTowns());
            }
            HttpFunctions.returnXmlStringToHttpClient(context, theTownToProcess.streets.ToXml());
        }

        public override void handleComplexRequest(string action)
        {
            AssetServer instance = AssetServer.getInstance();
            Street s = null;
            if (theTownToProcess == null)
            {
                try
                {
                    s = instance.getStreet(new Guid(splitArrayFromHandlableAction(context.Request.Url.ToString())[1]));
                }
                catch
                {
                }
                throw new Exception("No town specified to get the streets from!<br />" + getHtmlFormattedListOfTowns());
            }

            s = theTownToProcess.getStreet(new Guid(splitArrayFromHandlableAction(context.Request.Url.ToString())[1]));
            if (s != null)
            {
                HttpFunctions.returnXmlStringToHttpClient(context, s.ToXml());
            }
            else
            {
                HttpFunctions.sendStandardResponse(context, "Street not found", 404);
            }
        }

        private string getHtmlFormattedListOfTowns()
        {
            string myList = "<ul>";
            string listEnd = "</ul>";
            foreach (Town t in AssetServer.getInstance().TownList)
            {
                myList += "<li><a href=\"town/" + t.id + "/street" + "\">" + t.classroomName + "</a></li>";
            }
            return myList + listEnd;
        }
    }
}


