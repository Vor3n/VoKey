using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	public bool playing;
	private bool paused = false;
	private float savedTimeScale;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//savedTimeScale = Time.timeScale;
	}
	
	void LateUpdate () {
		if (Input.GetKeyDown("escape")) 
		{
			if (!paused && playing)
			{
				savedTimeScale = Time.timeScale;
                //Debug.Log("Timescale: " + Time.timeScale + ", " + savedTimeScale);
	   			Time.timeScale = 0;
				AudioListener.pause = true;
				GameObject PausePanel = GameObject.Find("PausePanel");
				PausePanel.GetComponent<UIPanel>().alpha = 1f;
				paused = true;
			}
			else
			{
				Time.timeScale = savedTimeScale;
                //Time.timeScale = 1;
                //Debug.Log("Timescale: " + Time.timeScale + ", " + savedTimeScale);
	    		AudioListener.pause = false;
				GameObject PausePanel = GameObject.Find("PausePanel");
				PausePanel.GetComponent<UIPanel>().alpha = 0f;
				paused = false;
			}
	    }
	}
}
