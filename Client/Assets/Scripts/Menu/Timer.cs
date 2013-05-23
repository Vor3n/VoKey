using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
	
	public UILabel Label;
	public bool TimeUp = false;
	public string TimeString = "";
	public bool Running = false;
	// Use this for initialization
	void StartTimer ()
	{
		Running = true;
	}
	
	void StopTimer ()
	{
		Running = false;
	}
	
	void Start ()
	{
		if (Label == null) {
			
			Label = (UILabel)GetComponent<UILabel> ();	
		}
		
	}
     
	void Update ()
	{
		if (Running) {
			if (TimeUp) {
				GameData.time += Time.deltaTime;
			} else {
				GameData.time -= Time.deltaTime;	
			}
		
			int Seconds = (int)Mathf.RoundToInt (GameData.time);
			TimeString = TimeFormat (Seconds);
		}
		if(Label != null)
		Label.text = TimeString;
		
	}
	
	public static string TimeFormat (int seconds)
	{
		
		int min = 0;
		int sec = seconds;
		int hrs = 0;
		if (seconds > 59) {
			min = seconds / 60;
			sec = seconds % 60;
		}
		if (min > 59) {
			hrs = min / 60;
			min = min % 60;
		}
		return string.Format ("{0:00}:{1:00}:{2:00}", hrs, min, sec);
		
	}
	
}
