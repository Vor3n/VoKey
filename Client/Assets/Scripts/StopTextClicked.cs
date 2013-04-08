using UnityEngine;
using System.Collections;
using System;

public class StopTextClicked : MonoBehaviour {

	// Use this for initialization
	void Start () {
		guiText.material.color = Color.cyan;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseUpAsButton()
	{
		Application.Quit();
	}
}
