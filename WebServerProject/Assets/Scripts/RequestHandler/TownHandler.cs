using System.Collections;
using System.Net;
using Vokey;
using VokeySharedEntities;
using AssemblyCSharp;
using System;

public class TownHandler : RequestHandler {
        private static string[] acceptableCommands = { "town" };

        public TownHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {
        
    }

    public override void handleSimpleRequest(string command)
    {
        AssetServer instance = AssetServer.getInstance();
        if (session != null)
        {
            if (session.IsStudent)
            {
                Town t = instance.getTown(session.User.townGuid);
                UnityEngine.Debug.Log ("Town Guid: " + t.id + " Number of streets: " + t.streets.Count);
                HttpFunctions.returnXmlStringToHttpClient(context, t.ToXml());
            }
            else
            {
                HttpFunctions.returnXmlStringToHttpClient(context, instance.TownList.ToXml());
            }
        }
        else
        {
            HttpFunctions.returnXmlStringToHttpClient(context, instance.TownList.ToXml());
        }
    }
    
    public override void handleComplexRequest (string command)
		{
				AssetServer instance = AssetServer.getInstance ();
				Town t = null;
				try {
						t = instance.getTown (new Guid(splitArrayFromHandlableAction(context.Request.Url.ToString())[1]));
				} catch {
				}
				if (t != null) {
						if (splitArrayFromHandlableAction (context.Request.Url.ToString()).Length > 2) {
								if (splitArrayFromHandlableAction (context.Request.Url.ToString()) [1] == "street" 
										|| splitArrayFromHandlableAction (context.Request.Url.ToString()) [2] == "street") {
										StreetHandler s = new StreetHandler (context);
										s.setTown (t);
										s.handleRequest ();
								}
						}else if (splitArrayFromHandlableAction (context.Request.Url.ToString()).Length > 2) {
								if (splitArrayFromHandlableAction (context.Request.Url.ToString()) [1] == "room" 
										|| splitArrayFromHandlableAction (context.Request.Url.ToString()) [2] == "room") {
										
								}
						}
						 else {
								HttpFunctions.returnXmlStringToHttpClient (context, t.ToXml ());
						}
				
				} else if (splitArrayFromHandlableAction (context.Request.Url.ToString()) [1] == "create") {
						if (session != null && session.IsTeacher && session.isValid) {
								t = MySerializerOfItems.FromXml<Town> (Content);
								instance.TownList.Add (t);
								UnityEngine.Debug.Log ("New Town: " + t.name);
								HttpFunctions.sendStandardResponse(context, "OK", 200);
						} else {
							HttpFunctions.sendStandardResponse(context, "YOU HAVE NO RIGHTS TO CREATE A TOWN", 403);
						}
				} else throw new Exception ("Specified town not found");
		
		}
    }

