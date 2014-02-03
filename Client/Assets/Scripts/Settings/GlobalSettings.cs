using UnityEngine;
using System.Collections;
using GuiTest;
using System;

public static class GlobalSettings{
	
	public static string serverURL
	{
		get
		{
			return PlayerPrefs.GetString("serverURL"); // execute from main thread
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
