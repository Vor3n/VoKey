using UnityEngine;
using System.Collections;
using Vokey;
using VokeySharedEntities;
using System.Collections.Generic;
using GuiTest;

public class ServerScript : MonoBehaviour
{

    public string StartStopButtonText
    {
        get
        {
            return (ws.Running) ? "Stop Webserver" : "Start Webserver";
        }
    }

    private AssetServer ws;
    // Use this for initialization
    void Start()
    {
        ws = new AssetServer();
        ws.LogMessage += HandleLogMessage;
        ws.Start();
    }

    void HandleLogMessage(string obj)
    {
        Debug.Log(obj);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 120, 20), StartStopButtonText))
            if (ws.Running)
                ws.Stop();
            else
                ws.Start();

        if (GUI.Button(new Rect(130, 10, 130, 20), "Ik doe niks"))
        {
			UnityEngine.Debug.Log ("KNOPJEDRUK");
			ws.writeUserData();
        }
    }
}
