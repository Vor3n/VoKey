using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Vokey;
using VokeySharedEntities;

/// <summary>
/// A requestHandler is an object that handles an incoming request by generating a response and returning this to a HttpClient.
/// </summary>
public class RequestHandler {
    private List<string> _handlableCommandsList;
    /// <summary>
    /// The actions that this handler is able to process.
    /// </summary>
    public List<string> handlableCommands {
        get {
            if (_handlableCommandsList == null) _handlableCommandsList = new List<string>();
            return _handlableCommandsList;
        }
    }

    protected VokeySession session = null;

    /// <summary>
    /// Enable this for a catchall handler.
    /// </summary>
    protected bool swallowsEverything = false;

    /// <summary>
    /// Returns whether the request is a simple request.
    /// </summary>
    public bool IsSimple
    {
        get
        {
            string s = getFirstHandlableAction(context);
            string t = context.Request.Url.ToString();
            return (t.EndsWith(s) || t.EndsWith(s + "/"));
        }
    }

    public string getFirstHandlableAction(HttpListenerContext hlc)
    {
        if (splitArrayFromHandlableAction(hlc.Request.Url.ToString()).Length > 0)
            return splitArrayFromHandlableAction(hlc.Request.Url.ToString())[0];
        throw new System.Exception("The current handler could not handle the provided request.");
    }

    public string[] splitArrayFromHandlableAction(string arguments)
    {
        string[] requestPieces = arguments.Split('/');
        UnityEngine.Debug.Log("Split into " + requestPieces.Length + " pieces.");
        List<string> actionPieces = new List<string>();
        int begin = 0;
        for (; begin < requestPieces.Length; begin++)
            if (handlableCommands.Contains(requestPieces[begin]))
                break;
        for (; begin < requestPieces.Length; begin++)
            actionPieces.Add(requestPieces[begin]);
        return actionPieces.ToArray();
    }

    /// <summary>
    /// Returns whethe the request contains form data.
    /// </summary>
    public bool IsFormRequest
    {
        get
        {
            if (context.Request.ContentType != null)
                return (context.Request.ContentType.ToLower() == "application/x-www-form-urlencoded");
            else return false;
        }
    }

    protected Dictionary<string, string> Headers = null;

    protected HttpListenerContext context;

    /// <summary>
    /// Constructor for a RequestHandler
    /// </summary>
    /// <param name="hlc">The HttpListenerContext to handle</param>
    /// <param name="commands">The commands that this handler is able to process.</param>
    public RequestHandler (HttpListenerContext hlc, string[] commands)
		{
				if (hlc != null) {
						context = hlc;
						Headers = GetHeadersFromRequest (hlc.Request);
						string sessId = null;
						if (Headers != null && Headers.ContainsKey ("Session")) {
								Headers.TryGetValue ("Session", out sessId);
								session = AssetServer.getInstance ().getSession (sessId);
	            
								if (session != null) {
										UnityEngine.Debug.Log ("Client authenticated using session " + session.SessionHash);
										session.heartBeat ();
								}
						} else {
						UnityEngine.Debug.Log ("No session provided with reuest");
						}
        }
        

        if (_handlableCommandsList == null)
        {
            _handlableCommandsList = new List<string>();
            _handlableCommandsList.AddRange(commands);
        }


    }

    /// <summary>
    /// Extracts the headers from a HttpListenerRequest.
    /// </summary>
    /// <param name="hlr">The HttpListenerRequest toe xtract the headers from.</param>
    /// <returns>The headers</returns>
    public static Dictionary<string, string> GetHeadersFromRequest(HttpListenerRequest hlr)
    {
        Dictionary<string, string> _h = new Dictionary<string, string>();
        foreach (string s in hlr.Headers) _h.Add(s, hlr.Headers[s]);
        return _h;
    }

    /// <summary>
    /// Returns the form data from a HttpListenerRequest
    /// </summary>
    /// <param name="hlr">The request to extract the form data from</param>
    /// <returns>The form data</returns>
    public static Dictionary<string, string> GetFormDataFromRequest(HttpListenerRequest hlr)
    {
        Dictionary<string, string> _fd = new Dictionary<string, string>();
        System.IO.Stream body = hlr.InputStream;
        System.Text.Encoding encoding = hlr.ContentEncoding;
        System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
        string s = reader.ReadToEnd();
        string[] pairs = s.Split('&');
        for (int x = 0; x < pairs.Length; x++)
        {
            string[] item = pairs[x].Split('=');
            _fd.Add(item[0], item[1]);
        }
        return _fd;
    }

    /// <summary>
    /// Handles a simple Http Request (e.g. a request that returns a list of items: http://webservice/employees
    /// </summary>
    /// <param name="action">The action to handle, e.g. employees</param>
    public virtual void handleSimpleRequest(string action) {
        throw new System.Exception("Handling Simple Requests not implemented in this class!");
    }

    /// <summary>
    /// Handles a simple Http Request (e.g. a request that returns a list of items: http://webservice/employees
    /// </summary>
    /// <param name="action">The action to handle, e.g. employees</param>
    public virtual void handleComplexRequest(string action) {
        throw new System.Exception("Handling Complex Requests not implemented in this class!");
    }

    /// <summary>
    /// Determines what kind of request this is and how to handle it. Then handles it.
    /// </summary>
    public void handleRequest()
    {
        if (swallowsEverything) handleSimpleRequest(AssetServer.getInstance().getFirstHandlableAction(context));
        if (IsSimple) handleSimpleRequest(AssetServer.getInstance().getFirstHandlableAction(context));
        else handleComplexRequest(AssetServer.getInstance().getFirstHandlableAction(context));
    }

}
