using UnityEngine;
using System.Collections;

public class InitializeGlobalSettings : MonoBehaviour {
	
	public string ServerURL;
	
	// Use this for initialization
	void Start () {
		GlobalSettings.serverURL = ServerURL;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
