using UnityEngine;
using System.Collections;

public class MouseOvers : MonoBehaviour {

	// Use this for initialization
	
	Texture InitialMaterial;
	public Texture OverMaterial;
	public string Level;
	
	void Start () {
		InitialMaterial = guiTexture.texture;
	}
	
	
	
	
 	void OnMouseUpAsButton()
 	{
		if(Level != "QUITAPP"){
		
  				Application.LoadLevel(Level);
		}else{
			Application.Quit();	
		}
 	}
	
	void OnMouseEnter(){
		guiTexture.texture = OverMaterial;
	}
	
	void OnMouseExit(){
		guiTexture.texture = InitialMaterial;
	}
}
