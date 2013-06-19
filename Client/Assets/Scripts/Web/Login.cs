using UnityEngine;
using System.Collections;

public class Login{
	
	private WWWForm form;
	private WWW www;
	private string username, password;
	public bool isDone = false;
	public string response;
	
	
	public Login(string un, string pw)
	{
		username = un;
		password = pw;
	}
	
	public IEnumerator startLogin()
	{
		Debug.Log ("Starting Login");
		isDone = false;
		float elapsedTime = 0.0f;
		form = new WWWForm();
        form.AddField("username", username);
		form.AddField("password",password);
		www = new WWW(GlobalSettings.serverURL + "login", form);
		
		while(!www.isDone){
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 1.9f) {
				Messenger.Broadcast (VokeyMessage.LOGIN_FAIL, "Server did not respond in a timely fashion. Elapsed time: " + elapsedTime + "seconds");
				break;
			}
			yield return www;
			if (!string.IsNullOrEmpty(www.error))
            	Messenger.Broadcast (VokeyMessage.LOGIN_FAIL, www.error);
		}
		
		isDone = www.isDone;
		string output;
		
		if (www.responseHeaders.TryGetValue("SESSION", out output))
		{
			Messenger.Broadcast (VokeyMessage.LOGIN_OK, output);
		} else {
			Messenger.Broadcast (VokeyMessage.LOGIN_FAIL, "We got no session in the reply. User probably entered an incorrect username and/or password.");
		}
    }
}


























