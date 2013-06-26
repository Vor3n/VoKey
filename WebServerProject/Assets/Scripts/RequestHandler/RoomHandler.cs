using AssemblyCSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

    public override void handleComplexRequest(string action)
    {
        Room r = null;
        if (splitArrayFromHandlableAction(context.Request.Url.ToString())[1] == "create")
        {
            if (true /*session != null && session.IsTeacher && session.isValid*/)
            {
                UnityEngine.Debug.Log(Content);
                try
                {
                    AssetServer.getInstance().TownList[0].educationalStreets[0].houses[0].rooms[0] = MySerializerOfItems.FromXml<Room>(Content);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.Log(e.StackTrace);
                }
                UnityEngine.Debug.Log("I'm still alive");
                HttpFunctions.sendStandardResponse(context, "OK", 200);
            }
            else
            {
                HttpFunctions.sendStandardResponse(context, "YOU HAVE NO RIGHTS TO CREATE A TOWN", 403);
            }
        }
    }
}