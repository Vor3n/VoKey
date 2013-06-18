using UnityEngine;
using System.Collections;

public class LoadingAssetsScreen : MonoBehaviour {
    private GameObject progressBar;
    private GameObject progressBarFill;
    private float lastProgress = 0;
    public float Progress = 0;
    Vector3 progressBarVector;
    private float progressBarWidth;
	// Use this for initialization
	void Start () {
        progressBarFill = GameObject.Find("ProgressBarFill");
        progressBar = GameObject.Find("ProgressBarContainer");
        progressBarWidth = progressBar.transform.localScale.x;
        progressBarVector = progressBarFill.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        if (Progress != lastProgress)
        {
            SetProgressPercentage(Progress);
        }
	}

    void SetProgressPercentage(float progress)
    {
        if (progress < 0 || progress > 100) throw new UnityException("Invalid percentage specified!");
        else
        {
            float progressToSet = progress * (progressBarWidth / 100);
            progressBarFill.transform.localScale = new Vector3(progressToSet > 12 ? progressToSet : 12f, progressBarVector.y, progressBarVector.z);
            Progress = progress; 
        }
    }
}
