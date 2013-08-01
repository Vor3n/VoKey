using UnityEngine;
using System.Collections;
using System.Net;
using System.Collections.Generic;
using Vokey;
using GuiTest;
using AssemblyCSharp;
using System;
using VokeySharedEntities;
using Thisiswhytheinternetexists.WebCore;

public class SessionHandler : RequestHandler {
    private static string[] acceptableCommands = { "login", "logout", "session", "sessions" };

    public SessionHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {

    }

    public override void handleSimpleRequest(string command)
    {
        switch (command)
        {
            case "login":
                if (IsFormRequest)
                {
                    Dictionary<string, string> formData = GetFormDataFromRequest(context.Request);
                    if (formData.ContainsKey("username") && formData.ContainsKey("password"))
                    {
                        string username = null, password = null;
                        formData.TryGetValue("username", out username);
                        formData.TryGetValue("password", out password);
                        VokeyUser u = AssetServer.TryVokeyLogin(username, password);
                        if (u != null)
                        {
                            string s = AssetServer.getInstance().CreateVokeySession(u);
                            UnityEngine.Debug.Log("Session created: " + s);
                            HttpFunctions.addSessionHeaderToResponse(context, s);
                            HttpFunctions.addHeaderToResponse(context.Response, "UserType", u.vtype.ToString());
                            HttpFunctions.sendTextResponse(context, "OK");
                        }
                        else
                        {
                            HttpFunctions.sendTextResponse(context, "INVALID CREDENTIALS", 403);
                        }
                    }
                }
                throw new Exception("Login called but no form data provided.");
            case "logout":
                string sessionHash = null;
                GetFormDataFromRequest(context.Request).TryGetValue("session", out sessionHash);
                VokeySession v = null;
                try
                {
                    v = AssetServer.getInstance().getVokeySession(sessionHash);
                }
                catch { }

                if (v != null && v.isValid)
                {
                    v.Invalidate();
                    HttpFunctions.sendStandardResponse(context, "OK", 200);
                }
                else
                {
                    HttpFunctions.sendTextResponse(context, "INVALID SESSION ID", 403);
                }
                break;
            case "session":
            case "sessions":
                throw new Exception("Viewing sessions is not yet implemented!");
        }
    }
}
