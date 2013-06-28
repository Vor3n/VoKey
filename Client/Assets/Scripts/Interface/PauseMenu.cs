using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	public bool playing;
	public bool Paused = false;
	public float SavedTimeScale;
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
			if (!Paused && playing)
			{
                Pause();
			}
			else
			{
                UnPause();
			}
	    }
	}

    public void Pause()
    {
        SavedTimeScale = Time.timeScale;
        //Debug.Log("Timescale: " + Time.timeScale + ", " + savedTimeScale);
        Time.timeScale = 0;
        AudioListener.pause = true;
        GameObject PausePanel = GameObject.Find("PausePanel");
        PausePanel.GetComponent<UIPanel>().alpha = 1f;
        Paused = true;
    }

    public void UnPause()
    {
        Time.timeScale = SavedTimeScale;
        //Time.timeScale = 1;
        //Debug.Log("Timescale: " + Time.timeScale + ", " + savedTimeScale);
        AudioListener.pause = false;
        GameObject PausePanel = GameObject.Find("PausePanel");
        PausePanel.GetComponent<UIPanel>().alpha = 0f;
        Paused = false;
    }
}
