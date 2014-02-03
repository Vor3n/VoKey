using UnityEngine;
using System.Collections;
using GuiTest;
using System;

public static class GlobalSettings{
	
	public static string serverURL
	{
		get
		{
			String url = "";
			GameControllerScript.InvokeOnMain(() => url = PlayerPrefs.GetString("serverURL"));
			
			url = PlayerPrefs.GetString("serverURL");
			// Wait here for answer from main thread?!?!
			
			if(url == null)
			{
				Debug.Log("url was null");
				url = "";	
			} else {
				Debug.Log("URL: " + url);	
			}
			return url;
		}
		set
		{
			if (value != null)
				PlayerPrefs.SetString("serverURL", value);
		}
	}
	public static string SessionID
	{
		get
		{
			return PlayerPrefs.GetString("SessionID");
		}
		set
		{
			if (value != null)
				PlayerPrefs.SetString("SessionID", value);
		}
	}
	public static User.UserType UserType
	{
		get
		{
			User.UserType type = (User.UserType)Enum.Parse(typeof(User.UserType), PlayerPrefs.GetString("usertype"));
			return type;
		}
		set
		{
			if(value != null)
				PlayerPrefs.SetString("usertype", value.ToString());
		}
	}
}
