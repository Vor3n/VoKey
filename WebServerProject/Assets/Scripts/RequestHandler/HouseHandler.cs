using UnityEngine;
using System.Collections;
using System.Net;
using Vokey;
using VokeySharedEntities;
using AssemblyCSharp;
using Thisiswhytheinternetexists.WebCore;

public class HouseHandler : RequestHandler
{
    private static string[] acceptableCommands = { "house" };
    
    private Street streetToProcess = null;

    public HouseHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {
		
    }
    
    /// <summary>
    /// Sets the street to process (in case of a redirected handler).
    /// </summary>
    /// <param name="s">S.</param>
    public void setStreet (Street s)
		{
		streetToProcess = s;
		}
    
    public override void handleSimpleRequest (string action)
	{
		AssetServer instance = AssetServer.getInstance();
        if (session != null)
        {
            if (session.IsStudent)
            {
            	if(streetToProcess != null)
                	HttpFunctions.returnXmlStringToHttpClient(context, streetToProcess.houses.ToXml());
                	else
                	throw new System.Exception("No street specified");
            }
            else
            {
                HttpFunctions.returnXmlStringToHttpClient(context, instance.TownList.ToXml());
                return;
            }
        }
        else
        {
        	if(streetToProcess != null)
            	HttpFunctions.returnXmlStringToHttpClient(context, streetToProcess.houses.ToXml());
            	else throw new System.Exception("No street specified");
            return;
        }
	}
	
	public override void handleComplexRequest (string action)
		{
			AssetServer instance = AssetServer.getInstance ();
			House h = null;
			string[] arguments = splitArrayFromHandlableAction (context.Request.Url.ToString());
			try {
				h = streetToProcess.getHouse (new System.Guid(arguments[1]));
			} catch {
			}

			if (h != null) {
				RoomHandler rh = new RoomHandler(context);
				rh.setHouseToProcess(h);
				rh.handleRequest();
			}
		}
}