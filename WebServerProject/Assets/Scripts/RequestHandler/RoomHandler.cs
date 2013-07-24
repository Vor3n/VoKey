using AssemblyCSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Thisiswhytheinternetexists.WebCore;
using Vokey;
/// <summary>
/// This handler handles Favicons.
/// </summary>
using VokeySharedEntities;


public class RoomHandler : RequestHandler
{
    private static string[] acceptableCommands = { "room" };

    public RoomHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {

    }
    private House theHouseToProcess = null;
    
    /// <summary>
    /// Sets the house to process (in case of handler redirection).
    /// </summary>
    /// <param name="h">The house to process.</param>
    public void setHouseToProcess(House h){
    	theHouseToProcess = h;
    }

        public override void handleSimpleRequest(string action)
        {
            if (theHouseToProcess == null)
            {
                throw new Exception("No house specified to get the rooms from!<br />");
            }
            HttpFunctions.returnXmlStringToHttpClient(context, theHouseToProcess.rooms.ToXml());
        }
        
        

    public override void handleComplexRequest (string action)
		{
				Room r = null;
				AssetServer instance = AssetServer.getInstance ();
				House h = null;
				string[] arguments = splitArrayFromHandlableAction (context.Request.Url.ToString());
				if (h != null) {
				try {
					r = theHouseToProcess.getRoom(new Guid(arguments[1]));
						} catch {
					}
					if (r != null) HttpFunctions.returnXmlStringToHttpClient(context, r.ToXml());
				}
				
				switch (splitArrayFromHandlableAction (context.Request.Url.ToString()) [1]) {
				case "create":
						if (true /*session != null && session.IsTeacher && session.isValid*/) {
								if (theHouseToProcess != null) {
										theHouseToProcess.rooms.Add (MySerializerOfItems.FromXml<Room> (Content));
										HttpFunctions.sendStandardResponse (context, "OK", 200);
										return;
								} else {
																try {
										AssetServer.getInstance ().TownList [0].residentialStreets [0].houses [0].rooms [0] = MySerializerOfItems.FromXml<Room> (Content);
								} catch (Exception e) {
										UnityEngine.Debug.Log (e.StackTrace);
								}
								}

								HttpFunctions.sendStandardResponse (context, "OK", 200);
						} else {
								HttpFunctions.sendStandardResponse (context, "YOU HAVE NO RIGHTS TO CREATE A TOWN", 403);
						}
						break;
					case "hoer":
						HttpFunctions.returnXmlStringToHttpClient(context, AssetServer.getInstance().getStreet (new Guid("d3a94e14-b5d6-425e-9a3c-31ca0603d383")).getHouse(new Guid("e4897c17-84b8-4da4-8570-0e05b4c161ce")).rooms[0].ToXml());
						break;
		 default:
		throw new Exception("The requesthandler could not handle action " + splitArrayFromHandlableAction (context.Request.Url.ToString())[1]);
				}
    }
}