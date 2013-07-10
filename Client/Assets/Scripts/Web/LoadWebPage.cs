using UnityEngine;
using System.Collections;
using UWK;
using VokeySharedEntities;
using System.Text;
using GuiTest;

public class LoadWebPage : MonoBehaviour{
	UWKView view;
	int width;
	int height;
	int x;
	int y;
	float transparency;
	static string page;
	string pagename;
	LoadWebPage lpw;

	public void DoStart (int _x, int _y, int _width, int _height, float _transparency,string _pagename, string urlsuffix) {
		
		x = _x;
		y = _y;
		width = _width;
		height = _height;
		transparency = _transparency;
		pagename = _pagename;
		page = urlsuffix;
		Debug.Log("DoStart Called");
		try
		{
			view = UWKCore.CreateView(pagename, GlobalSettings.serverURL + page , width, height );
			view.LoadURL(GlobalSettings.serverURL + page);
			SetProperties();
		}
		catch
		{
			Debug.Log("Couldn't create view. were all values assigned?");
		}
	}
	
	void updateviewpage(string suffix)
	{
		view.LoadURL(GlobalSettings.serverURL + suffix);
	}
	void setviewpage(string url)
	{
		view.LoadURL(url);	
	}
	
	void OnGUI()
	{
		try
		{
			view.OnWebGUI(x,y,width,height,transparency);
		}
		catch
		{
			Debug.Log("Something went wrong while trying OnWebGUI.");
		}
	}
	
	

	
	// Example delegate called as a callback from Javascript on the page
	public static void OnSwitchCommand (object sender, BridgeEventArgs args)
	{
		LoadWebPage lwp = GameObject.Find("EditTownButton").GetComponent<LoadWebPage>();
		Debug.Log("SwitchCommand: "+ args.Args[0]);
		switch(args.Args[0]){
		case "EditRoom":
			GameObject.Find("GameController").GetComponent<GameControllerScript>().RoomToOpen = new System.Guid(args.Args[1]);
			Debug.Log("ROOM GUID PASSED ON:\n"+GameObject.Find("GameController").GetComponent<GameControllerScript>().RoomToOpen);
			GameObject.Find("GameController").GetComponent<GameControllerScript>().TownGUID = args.Args[2];
			Application.LoadLevel("EditorFirstTest");
			break;
		case "CloseWindow":
			GameObject.Find("EditTownButton").GetComponent<EditTown>().DoClear();
			break;
		case "ShowUser":
			lwp.updateviewpage("dynamic/edituser/"+args.Args[1]);
			Debug.Log("EDITUSER: "+args.Args[1]);
			break;
		case "ShowClass":
			break;
		case "ShowAssignments":
			break;
		case "OpenRoomEditor":
			break;
		case "ShowTown":
			lwp.setviewpage(GlobalSettings.serverURL + page);
			break;
		case "UpdateTown":
			lwp.startUpdateTown("XML",args.Args[1]);
			break;
		case "EditUser":
			lwp.updateviewpage("dynamic/edituser/"+args.Args[1]);
			Debug.Log("EDITUSER: "+args.Args[1]);
			break;
		case "DeleteUser":
			Debug.Log(args.Args[0] + "\n" + args.Args[1] + "\n" + args.Args[2]);
			//town guid arg1
			//user guid arg2
			Town t = lwp.getTown(args.Args[1]);
			User u = t.getUser(new System.Guid(args.Args[2]));
			Debug.Log("FULLNAME: "+u.FullName);
			lwp.startUpdateTown(t.id.ToString()+"/user/delete", u.ToXml());
			

			//http://ws:9090/town/{guid}/user/{guid}/delete
			//refresh page
			//view = UWKCore.CreateView(pagename,GlobalSettings.serverURL + page , width, height );
			break;
		case "Log":
			Debug.Log(args.Args[1]);
			break;
		}
		
		/*#if UNITY_EDITOR
			if (args.Args.Length == 3 && args.Args[0] == "1" && args.Args[1] == "Testing123" && args.Args[2] == "45678")
				EditorUtility.DisplayDialog ("Hello!", "The UWebKit JavaScript Bridge is alive and well!", "Awesome");
			else
				EditorUtility.DisplayDialog ("Hello!", "The UWebKit JavaScript Bridge callback was invoked, but args were wrong!", "Ok");
		#endif*/
	}
	
	void startUpdateTown(string URLSuffix, string objectxml)
	{
		StartCoroutine(DoTownRequest(objectxml, URLSuffix));
	}
	
	void startUpdateUser(string objectxml, string userGUID)
	{
		StartCoroutine(DoTownRequest(objectxml, ""));
	}
	
	static bool props = false;
	public static void SetProperties ()
	{
		
		if (props)
			return;
		
		props = true;
		
		// Bind Javascript callback to Unity.SayHello js function	
		Bridge.BindCallback ("Unity", "SwitchCommand", OnSwitchCommand);
	}
	
	public Town getTown(string GUID)
	{
		foreach(Town t in GameObject.Find("GameController").GetComponent<GameControllerScript>().towns)
		{
			if (t.id == new System.Guid(GUID))
			{
				Debug.Log("FOUND TOWN");
				return t;
			}
		}
		Debug.Log("DID NOT FIND TOWN");
		return null;
	}
	
	
	public IEnumerator DoTownRequest(string objectxml, string URLSuffix)
	{

		bool isDone = false;
		Debug.Log("STARTING REQUEST");
		float elapsedTime = 0.0f;
		Debug.Log("SENDING THING");
		string url = GlobalSettings.serverURL + "town/" +URLSuffix;

		byte[] b = Encoding.UTF8.GetBytes(objectxml);
		Hashtable htbl = new Hashtable();
		htbl.Add("Session", GlobalSettings.SessionID);
		htbl.Add("Content-Type", "text/html");
		
		WWW www = new WWW(url, b, htbl);
		while(!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 2.0f) break;
			yield return www;
		}
		isDone = www.isDone;
		string response = www.text;
		Debug.Log("DOTOWNREQUESTRESPONSE: "+response);
		GameObject TownList = GameObject.Find("TownList");
		TownList.GetComponent<TeacherMenuTowns>().DoClear();
		TownList.GetComponent<TeacherMenuTowns>().DoStart();
		view.LoadURL(view.URL);
    }
	
}
