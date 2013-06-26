using UnityEngine;
using System.Collections;
using System.Net;

public class HouseHandler : RequestHandler
{
    private static string[] acceptableCommands = { "house" };

    public HouseHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {
		
    }
    
    public override void handleSimpleRequest (string action)
	{
		
	}

}