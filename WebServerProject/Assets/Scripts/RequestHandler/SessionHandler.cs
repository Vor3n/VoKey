using UnityEngine;
using System.Collections;
using System.Net;
using System.Collections.Generic;
using Vokey;
using GuiTest;
using AssemblyCSharp;
using System;
using VokeySharedEntities;

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
                        User u = AssetServer.TryLogin(username, password);
                        if (u != null)
                        {
                            string s = AssetServer.getInstance().CreateVokeySession(u);
                            UnityEngine.Debug.Log("Session created: " + s);
                            HttpFunctions.addSessionHeaderToResponse(context, s);
                            HttpFunctions.addHeaderToResponse(context, "UserType", u.type.ToString());
                            HttpFunctions.sendTextResponse(context, "OK");
                        }
                        else
                        {
                            HttpFunctions.sendTextResponse(context, "JE LOGIN IS INVALIDE", 403);
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
                    v = AssetServer.getInstance().getSession(sessionHash);
                }
                catch { }

                if (v != null && v.isValid)
                {
                    v.Invalidate();
                    HttpFunctions.sendStandardResponse(context, "OK", 200);
                }
                else
                {
                    throw new Exception("Tried to invalidate a session that is already invalid.");
                }
                break;
            case "session":
            case "sessions":
                throw new Exception("Viewing sessions is not yet implemented!");
        }
    }
}
