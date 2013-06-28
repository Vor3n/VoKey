using UnityEngine;
using System.Collections;
public enum GameMode{ Normal, Challenge}
public class GameData : MonoBehaviour {
	public static int ItemsToFind;
	public static int ItemsFound;
	private static CreateObjectiveList ItemList;
	public GameMode Mode;
	public static float time;
	
	public bool Ended = false;
	// Use this for initialization
	void Start () {
		Debug.Log("Started the game");
		ItemsFound =0;
		time=0;
		Time.timeScale =1;
		Ended= false;
		GetComponent<Timer>().Running = true;
		var GO = GameObject.Find("EndGamePanel");
		GO.GetComponent<UIPanel>().enabled = false;
		ItemList = (CreateObjectiveList) GameObject.Find("ItemList").GetComponent<CreateObjectiveList>();
		ItemsToFind =  ItemList.Objectives.Length;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		switch(Mode){
		case GameMode.Normal:
			NormalUpdate();
			
		break;
			
		case GameMode.Challenge:
			ChallengeUpdate();
			
		break;
			
		}
		
		
		
	}
	
	void NormalUpdate(){
		
		if(ItemsFound >= ItemsToFind){
			if(!Ended){
				OnEndGame();
			}
		}
	}
	
	void ChallengeUpdate(){
		
		
	}
	
	
	/// <summary>
	/// What happens when the Game ends.
	/// </summary>
	void OnEndGame(){
		Debug.Log("Game ends");
		Ended = true;
		GetComponent<Timer>().Running = false;
		var GO = GameObject.Find("EndGamePanel");
		GO.GetComponent<UIPanel>().enabled = true;
		int Seconds = (int) Mathf.RoundToInt(GameData.time);
		GameObject.Find ("TimeTaken").GetComponent<UILabel>().text = "Time " + Timer.TimeFormat(Seconds) ;
		time = 0;
		this.GetComponent<PauseMenu>().UnPause();
	}
}
