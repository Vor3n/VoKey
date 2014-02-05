using UnityEngine;
using System.Collections;

public class LightSwtich : MonoBehaviour {

	// Use this for initialization
	
	private GameObject lightSwitch;
	private Light spotLightOutside;
	private Light roomLight;
	private Light spotlightLightSwitch;
	
	void Start () {
		lightSwitch = GameObject.Find("LightSwitch");
		spotLightOutside = GameObject.Find("SpotlightOutside").GetComponent<Light>();
		roomLight = GameObject.Find("Roomlight").GetComponent<Light>();
		spotlightLightSwitch = GameObject.Find("SpotlightLightSwitch").GetComponent<Light>();
	}
	
	void OnMouseUpAsButton() {
		if(spotLightOutside.enabled) spotLightOutside.enabled = false;
		else spotLightOutside.enabled = true;
		
		if(roomLight.enabled) roomLight.enabled = false;
		else roomLight.enabled = true;
		
		if(spotlightLightSwitch.enabled) spotlightLightSwitch.enabled = false;
		else spotlightLightSwitch.enabled = true;
	}
}