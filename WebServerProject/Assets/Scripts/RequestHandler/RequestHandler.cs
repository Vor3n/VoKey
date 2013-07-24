using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Vokey;
using VokeySharedEntities;

/// <summary>
/// A requestHandler is an object that handles an incoming request by generating a response and returning this to a HttpClient.
/// </summary>
public class RequestHandler : Thisiswhytheinternetexists.WebCore.RequestHandler
{
    private List<string> _handlableCommandsList;

    protected VokeySession session = null;

    protected HttpListenerContext context;

    /// <summary>
    /// Constructor for a RequestHandler
    /// </summary>
    /// <param name="hlc">The HttpListenerContext to handle</param>
    /// <param name="commands">The commands that this handler is able to process.</param>
    public RequestHandler(HttpListenerContext hlc, string[] commands) : base (hlc, commands)
    {
        if (hlc != null)
        {
            context = hlc;
            Headers = GetHeadersFromRequest(hlc.Request);
            string sessId = null;
            if (Headers != null && Headers.ContainsKey("Session"))
            {
                Headers.TryGetValue("Session", out sessId);
                session = AssetServer.getInstance().getVokeySession(sessId);

                if (session != null)
                {
                    UnityEngine.Debug.Log("Client authenticated using session " + session.SessionHash);
                    session.heartBeat();
                }
            }
            else
            {
                UnityEngine.Debug.Log("No session provided with reuest");
            }
        }


        if (_handlableCommandsList == null)
        {
            _handlableCommandsList = new List<string>();
            _handlableCommandsList.AddRange(commands);
        }


    }


}
