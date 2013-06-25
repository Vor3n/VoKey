using UnityEngine;
using System.Collections;
using GuiTest;

public class GameControllerScript : MonoBehaviour {
	
	public string ServerURL;
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		GlobalSettings.serverURL = ServerURL;
		Messenger.AddListener<string, User.UserType> (VokeyMessage.LOGIN_OK, LoginHandler);
		Messenger.AddListener<string> (VokeyMessage.LOGIN_FAIL, LoginFailHandler);
		Messenger.AddListener<string> (VokeyMessage.REQUEST_COMPLETE, RequestCompleteHandler);
	}
	
	void LoginHandler(string sessionId, User.UserType usertype){
		GlobalSettings.SessionID = sessionId;
		GlobalSettings.UserType = usertype;
		switch(GlobalSettings.UserType)
		{
			//student
			case User.UserType.Student:
				Application.LoadLevel("StreetTest");
			break;
			//docent
			case User.UserType.Teacher:
				Application.LoadLevel("TeachMenu");
			break;
			default:
			break;
		}
		
		//DO STUFF
		//LIKE
		//SET THE HOUSE TO FOCUS ON
		//BUT ONLY
		//IF I AM A STUDENT
		//HAVE A NICE DAY
		//AND
		//THIS SCRIPT SHOULDN'T BE HERE.
		//I'M SORRY.
		//GetAssetBundlesFromServer();
	}
	
	void LoginFailHandler(string message){
		Debug.Log ("Login failure: " + message);
	}
	
	void RequestCompleteHandler(string message){
		Debug.Log("REQUEST COMPLETE: " + message);
	}
	
	void GetAssetBundlesFromServer()
	{
		DoRequest r = new DoRequest("rooms");
		StartCoroutine(r.Request());
		float elapsedTime = 0.0f;
		while (!r.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 2.0f) break;
		}
	}
	
}
