using UnityEngine;
using System.Collections;
using GuiTest;
using VokeySharedEntities;
using System.Collections.Generic;

public class GameControllerScript : MonoBehaviour {
	
	public string ServerURL;
	public List<Town> towns;
    public System.Guid RoomToOpen;
	public string RoomGUID;

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
				Application.LoadLevel("TeacherMenu");
			break;
			default:
			break;
		}
		//GetAssetBundlesFromServer();
		GetAssetBundlesFromServer();
	}
	
	void LoginFailHandler(string message){
		Debug.Log ("Login failure: " + message);
	}
	
	void RequestCompleteHandler(string message){
		Debug.Log("REQUEST COMPLETE: " + message);
	}
	
	void GetAssetBundlesFromServer()
	{
		WebServiceRequest r = new WebServiceRequest("rooms");
		StartCoroutine(r.Request());
	}
	
}
