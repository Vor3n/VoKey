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
		
		/// <summary>
		/// Sets the town to be handled.
		/// </summary>
		/// <param name="t">T.</param>
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

        public override void handleComplexRequest (string action)
				{
						AssetServer instance = AssetServer.getInstance ();
						Street s = null;
						string[] arguments = splitArrayFromHandlableAction (context.Request.Url.ToString());
						if (theTownToProcess == null) {
							try {
									s = instance.getStreet (new Guid(arguments[1]));
							} catch {
							}
							throw new Exception ("No town specified to get the streets from!<br />" + getHtmlFormattedListOfTowns());
						} else {
						if (arguments.Length > 2) {
								switch (arguments [2]) {
								case "house":
															try {
									s = instance.getStreet (new Guid(arguments[1]));
							} catch {
							}
									HouseHandler hh = new HouseHandler(context);
										hh.setStreet(s);
										hh.handleRequest ();
										break;
								}
						} else {
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
						}
						
           
        }

		/// <summary>
		/// Gets the html formatted list of towns.
		/// </summary>
		/// <returns>The html formatted list of towns.</returns>
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


