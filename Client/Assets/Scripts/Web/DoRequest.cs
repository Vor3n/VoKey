using UnityEngine;
using System.Collections;

public class DoRequest : MonoBehaviour {
	
	private WWW www;
	private string url;
	public bool isDone = false;
	public string response;
	
	public DoRequest(string suffix)
	{
		url = GlobalSettings.serverURL + suffix;
		Debug.Log(url);
		
	}

	public IEnumerator Request()
	{
		isDone = false;
		Debug.Log("STARTING REQUEST");
		float elapsedTime = 0.0f;
		Debug.Log("SENDING THING");
		
		byte[] b = {0x10, 0x20};
		Hashtable htbl = new Hashtable();
		htbl.Add("Session", GlobalSettings.SessionID);
		htbl.Add("Content-Type", "text/html");
		
		www = new WWW(url, b, htbl);
		while(!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 1.9f) break;
			yield return www;
		}
		isDone = www.isDone;
		response = www.text;
		Debug.Log("2e request done: " + response);
    }
}
