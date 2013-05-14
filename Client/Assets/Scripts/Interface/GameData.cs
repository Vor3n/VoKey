using UnityEngine;
using System.Collections;
public enum GameMode{ Normal, Challenge}
public class GameData : MonoBehaviour {
	public static int ItemsToFind;
	public static int ItemsFound;
	private static CreateObjectiveList ItemList;
	public static GameMode Mode;
	public static float time;
	
	public bool Ended = false;
	// Use this for initialization
	void Start () {
		ItemsFound =0;
		ItemList = (CreateObjectiveList) GameObject.Find("ItemList").GetComponent<CreateObjectiveList>();
		ItemsToFind =  ItemList.Objectives.Length;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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
		
	}
}
