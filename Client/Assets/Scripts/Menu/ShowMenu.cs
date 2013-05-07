using UnityEngine;
using System.Collections;

public class ShowMenu : MonoBehaviour {
	bool paused = false;
	private float savedTimeScale;
	GameObject panel2;

	// Use this for initialization
	void Start () {
		panel2  = GameObject.Find("LoginPanel");
  		NGUITools.SetActive(panel2,false);
	
	}
	
	
	void PauseGame() {
	    savedTimeScale = Time.timeScale;
	    Time.timeScale = 0;
  		NGUITools.SetActive(panel2,true);
		paused = true;
	    //AudioListener.pause = true;
		
	}
	
	void UnPauseGame() {
	    Time.timeScale = savedTimeScale;
  		NGUITools.SetActive(panel2,false);
		paused = false;
	    //AudioListener.pause = false;
	}
	
	void LateUpdate () { 
		if (Input.GetKeyDown("escape")) 
		{
	        if (paused)
				UnPauseGame(); 
			else
				PauseGame();
	    }
	}
}
