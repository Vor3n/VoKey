using UnityEngine;
using System.Collections;

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
}
