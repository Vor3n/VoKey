using UnityEngine;
using System.Collections;

public class InitializeGlobalSettings : MonoBehaviour {
	
	public string ServerURL;
	
	// Use this for initialization
	void Start () {
		GlobalSettings.serverURL = ServerURL;
		Messenger.AddListener<string> (VokeyMessage.LOGIN_OK, LoginHandler);
		Messenger.AddListener<string> (VokeyMessage.LOGIN_FAIL, LoginFailHandler);
	}
	
	void LoginHandler(string sessionId){
		GlobalSettings.SessionID = sessionId;
		//DO STUFF
		//LIKE
		//SET THE HOUSE TO FOCUS ON
		//BUT ONLY
		//IF I AM A STUDENT
		//HAVE A NICE DAY
		//AND
		//THIS SCRIPT SHOULDN'T BE HERE.
		//I'M SORRY.
		Application.LoadLevel("StreetTest");
	}
	
	void LoginFailHandler(string message){
		Debug.Log ("Login failure: " + message);
	}
	
}
