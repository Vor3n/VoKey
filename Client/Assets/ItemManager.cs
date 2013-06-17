using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class ItemManager : MonoBehaviour {
	public InitializeItem Item;
	public UIGrid Grid;
	
	public Dictionary<string, string> ItemList;
	List<GameObject> Children;
	
	// Use this for initialization
	void Start () {
		Children = new List<GameObject>();
		ItemList = new Dictionary<string, string>();
		ItemList.Add("Harv", "1111");
		ItemList.Add("Barf", "1111");
		ItemList.Add("Clardf", "1111");
		ItemList.Add("Narf", "1111");
		ItemList.Add("Blarf", "1111");
		ItemList.Add("Garf", "1111");
		ItemList.Add("Iarf", "1111");
		ItemList.Add("Rarf", "1111");
		ItemList.Add("Quarf", "1111");
		ItemList.Add("Parf", "1111");
	
		CreateItems(ItemList);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void CreateItems(Dictionary<string,string> Items){
		
		Children.ForEach(child => Destroy(child));

		foreach( KeyValuePair<string,string> IT in Items){
		GameObject obj = (GameObject) Instantiate(Item.gameObject);	
			obj.transform.parent = Grid.gameObject.transform;
			obj.transform.localScale = new Vector3(1,1,1);
			obj.SetActive(true);
			obj.GetComponent<InitializeItem>().InitialiseItem(IT.Key,IT.Value);
			obj.name = IT.Key;
			Children.Add(obj);
		}
		
		Grid.repositionNow = true;
	}
	
	public void OnSubmit(string Result){
		Debug.Log(Result);
		Debug.Log ("Empty:" +(Result == ""));
		Search(Result);
		
	}
	
	public void Search(string search){
		if(search != ""){
			Dictionary<string, string> NewList = new Dictionary<string, string>();
			foreach( KeyValuePair<string,string> IT in ItemList){
				if (IT.Key.Contains(search)){
					
				 NewList.Add(IT.Key, IT.Value);	
				}
		
			
			}
			CreateItems(NewList);
		}else{
			CreateItems(ItemList);			
		}
		Grid.repositionNow = true;
		
	}
}