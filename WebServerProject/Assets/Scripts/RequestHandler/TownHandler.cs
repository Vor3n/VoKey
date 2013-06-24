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
            	UnityEngine.Debug.Log("Still Alive");
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
							if (splitArrayFromHandlableAction (context.Request.Url.ToString()) [2] == "street") {
									StreetHandler s = new StreetHandler (context);
									s.setTown (t);
									s.handleRequest ();
							}
					} else {
						HttpFunctions.returnXmlStringToHttpClient(context, t.ToXml ());
					}
				
				} else throw new Exception ("Specified town not found");
		
		}
    }

