using UnityEngine;
using System.Collections;

public class ItemCounter : MonoBehaviour {
	
	public UILabel Label;
	CreateObjectiveList ItemList;
	// Use this for initialization
	void Start () {
		if(Label== null){
			Label = (UILabel)GetComponent<UILabel>();	
		}
		
		GameData.ItemsFound =0;
		ItemList = GameObject.Find("ItemList").GetComponent<CreateObjectiveList>();
		GameData.ItemsToFind =  ItemList.Objectives.Length;
		Debug.Log("Count:" + GameData.ItemsToFind); 
	}
	
	// Update is called once per frame
	void Update () {
		GameData.ItemsFound = GameData.ItemsToFind - ItemList.Objectives.Length;
		if(Label != null)
			Label.text = GameData.ItemsFound + "/" + GameData.ItemsToFind;
	}
}
