using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour {
	
	public string ServerURL;
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
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
		GetAssetBundlesFromServer();
	}
	
	void LoginFailHandler(string message){
		Debug.Log ("Login failure: " + message);
	}
	
	void GetAssetBundlesFromServer()
	{
		DoRequest r = new DoRequest("rooms");
		StartCoroutine(r.Request());
	}
	
}
