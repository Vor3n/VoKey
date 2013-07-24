using UnityEngine;
using System.Collections;
using GuiTest;
using VokeySharedEntities;
using System.Collections.Generic;

public class GameControllerScript : MonoBehaviour {
	
	public List<Town> towns;
    public System.Guid RoomToOpen;
	public string RoomGUID;
	public string TownGUID;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		try
		{
			GameObject serverlabel = GameObject.Find("ServerLabel");
			UILabel UIserverlabel = (UILabel)serverlabel.GetComponent("UILabel");
			UIserverlabel.text = GlobalSettings.serverURL;	
		}
		catch
		{}
        Messenger.AddListener<string, VokeyUser.VokeyUserType>(VokeyMessage.LOGIN_OK, LoginHandler);
		Messenger.AddListener<string> (VokeyMessage.LOGIN_FAIL, LoginFailHandler);
		Messenger.AddListener<string> (VokeyMessage.REQUEST_COMPLETE, RequestCompleteHandler);
	}
	
	void LoginHandler(string sessionId, VokeyUser.VokeyUserType usertype){
		GlobalSettings.SessionID = sessionId;
		GlobalSettings.UserType = usertype;
		switch(GlobalSettings.UserType)
		{
			//student
            case VokeyUser.VokeyUserType.Student:
				Application.LoadLevel("StreetTest");
			break;
			//docent
            case VokeyUser.VokeyUserType.Teacher:
				Application.LoadLevel("TeacherMenu");
			break;
			default:
			break;
		}
		//GetAssetBundlesFromServer();
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
		WebServiceRequest r = new WebServiceRequest("rooms");
		StartCoroutine(r.Request());
	}
	
}
