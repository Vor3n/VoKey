using UnityEngine;
using System.Collections;
using UWK;
using UnityEditor;

public class LoadWebPage : MonoBehaviour{
	UWKView view;
	int width;
	int height;
	int x;
	int y;
	float transparency;
	string page;
	string pagename;

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
			view = UWKCore.CreateView(pagename,GlobalSettings.serverURL + page , width, height );
			SetProperties();
		}
		catch
		{
			Debug.Log("Couldn't create view. were all values assigned?");
		}
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
	public static void OnSayHello (object sender, BridgeEventArgs args)
	{
		Debug.Log("ONSAYHELLO");
		#if UNITY_EDITOR
			if (args.Args.Length == 3 && args.Args[0] == "1" && args.Args[1] == "Testing123" && args.Args[2] == "45678")
				EditorUtility.DisplayDialog ("Hello!", "The UWebKit JavaScript Bridge is alive and well!", "Awesome");
			else
				EditorUtility.DisplayDialog ("Hello!", "The UWebKit JavaScript Bridge callback was invoked, but args were wrong!", "Ok");
		#endif
	}
	
	// Example delegate called as a callback from Javascript on the page
	public static void OnSwitchCommand (object sender, BridgeEventArgs args)
	{
		Debug.Log("SwitchCommand");
		switch(args.Args[0]){
		case "RoomEdit":
			Application.LoadLevel("MainMenu");
			break;
		case "ShowStudentsBack":
			GameObject teachmenu = GameObject.Find("TeacherMainMenu");
			teachmenu.GetComponent<UIPanel>().alpha = 1f;
			GameObject studentpanel = GameObject.Find("ShowStudentsPanel");
			studentpanel.GetComponent<UIPanel>().alpha = 0f;
			GameObject StudentList = GameObject.Find("EditTownButton");
			StudentList.GetComponent<EditTown>().DoClear();
			break;
		case "ShowUser":
			break;
		case "ShowClass":
			break;
		case "ShowAssignments":
			break;
		case "OpenRoomEditor":
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
	
	static bool props = false;
	public static void SetProperties ()
	{
		
		if (props)
			return;
		
		props = true;
		
		// Bind Javascript callback to Unity.SayHello js function
		Bridge.BindCallback ("Unity", "SayHello", OnSayHello); //OnSwitchCommand	
		Bridge.BindCallback ("Unity", "SwitchCommand", OnSwitchCommand);
	}
	
	
}
