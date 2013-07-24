using UnityEngine;
using System.Collections;
using GuiTest;
using System;

public static class GlobalSettings{
	
	public static string serverURL
	{
		get
		{
			return PlayerPrefs.GetString("serverURL");
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
    public static VokeyUser.VokeyUserType UserType
	{
		get
		{
            VokeyUser.VokeyUserType type = (VokeyUser.VokeyUserType)Enum.Parse(typeof(VokeyUser.VokeyUserType), PlayerPrefs.GetString("usertype"));
			return type;
		}
		set
		{
			if(value != null)
				PlayerPrefs.SetString("usertype", value.ToString());
		}
	}
}
