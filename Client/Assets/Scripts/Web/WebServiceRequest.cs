using UnityEngine;
using System.Collections;
using VokeySharedEntities;
using System.Collections.Generic;

public class WebServiceRequest{
	
	private WWW www;
	private string url;
	public bool isDone = false;
	public string response;
	
	public WebServiceRequest(string suffix)
	{
		url = GlobalSettings.serverURL + suffix;
		Debug.Log(url);
		
	}

	public IEnumerator Request()
	{
		isDone = false;
		Debug.Log("STARTING REQUEST");
		//kfloat elapsedTime = 0.0f;
		Debug.Log("SENDING THING");
		
		byte[] b = {0x10, 0x20};
		Hashtable htbl = new Hashtable();
		htbl.Add("Session", GlobalSettings.SessionID);
		htbl.Add("Content-Type", "text/html");
		
		www = new WWW(url, b, htbl);
		while(!www.isDone)
		{
			//elapsedTime += Time.deltaTime;
			//if (elapsedTime >= 4.0f) break;
		}
		isDone = www.isDone;
		response = www.text;
		Messenger.Broadcast (VokeyMessage.REQUEST_COMPLETE, response);
		Debug.Log("2e request done: " + response);
		yield return www;
    }
}
