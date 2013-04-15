using UnityEngine;
using System.Collections;

public class StartTextClicked : MonoBehaviour {

	// Use this for initialization
	void Start () {
		guiText.material.color = Color.cyan;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public string level;
	void OnMouseUpAsButton()
	{
		Application.LoadLevel(level);
	}
}
