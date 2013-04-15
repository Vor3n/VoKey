using UnityEngine;
using System.Collections;

public class GuiTextOnMouseOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private Color startcolor;
	void OnMouseEnter()
	{
		startcolor = guiText.material.color;
		guiText.material.color = Color.blue;		
	}
	void OnMouseExit()
	{
		guiText.material.color = startcolor;		
	}
}
