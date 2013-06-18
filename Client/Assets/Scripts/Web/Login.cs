using UnityEngine;
using System.Collections;

public class Login {
	
	private WWWForm form;
	private WWW www;
	private string username, password;
	public bool isDone = false;
	public string response;
	
	
	public Login(string un, string pw)
	{
		Debug.Log("public login");
		username = un;
		password = pw;
	}
	
	
	public IEnumerator startLogin()
	{
		float elapsedTime = 0.0f;
		form = new WWWForm();
        form.AddField("username", username);
		form.AddField("password",password);
		www = new WWW(GlobalSettings.serverURL + "login", form);
		while(!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 1.9f) break;
			yield return www;
		}
		isDone = www.isDone;
		//response = www.responseHeaders;
		string output;
		if (www.responseHeaders.TryGetValue("SESSION", out output))
		{
			GlobalSettings.SessionID = output;
			Debug.Log("SESSIONID: " + GlobalSettings.SessionID);
		}
    }
}


























