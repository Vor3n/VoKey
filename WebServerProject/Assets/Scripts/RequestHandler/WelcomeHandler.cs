using AssemblyCSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Vokey;

public class WelcomeHandler : RequestHandler
{
    private static string[] acceptableCommands = { "welcome" };
    private const string responseBegin = "Resource not found. Welcome to reality.<br />Available actions are:<br /><ul>";
    private const string responseEnd = "</ul>";
    public WelcomeHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {
        swallowsEverything = true;
    }

    public override void handleSimpleRequest(string action)
    {
        AssetServer instance = AssetServer.getInstance();
        string availableActions = "";
        foreach (string s in instance.handlers.Keys)
        {
            availableActions += "<li><a href=\"" + s + "\">View " + getPlural(s) + "</a></li>";
        }
        HttpFunctions.sendTextResponse(context, responseBegin + availableActions + responseEnd, 404);
    }

    private string getPlural(string nameOfObject)
    {
        if (!nameOfObject.EndsWith("s")) return nameOfObject + "s";
        else return nameOfObject;
    }
}
