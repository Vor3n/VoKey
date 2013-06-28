using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System;
public class ItemManager : MonoBehaviour {
	public InitializeItem Item;
	public UIGrid Grid;
	public string URL;
	AssetBundleManager abm;
	
	public Dictionary<string, KeyValuePair<string, Guid>> ItemList;
	List<GameObject> Children;
	
	// Use this for initialization
	void Start () {
		Children = new List<GameObject>();
		abm = GameObject.Find("GameController").GetComponent<AssetBundleManager>();
		ItemList = new Dictionary<string, KeyValuePair<string, Guid>>();
		
		LoadFromBundles();
	
   		
	}
	
	/// <summary>
	/// Gets all items from the VokeyAssetbundles.
	/// Note: this function will be called by the AssetbundleManager upon finishing.
	/// </summary>
	public void LoadFromBundles(){
		foreach(VokeyAssetBundle VAB in abm.Bundles.Values){
			foreach(VokeyAsset VA in VAB.objects){
				ItemList.Add(VA.hashString+"", new KeyValuePair<string, Guid>( VA.name,VAB.modelId));	
			}
		}
		CreateItems(ItemList);
		
	}
	
	public void CreateItems(Dictionary<string,KeyValuePair<string, Guid>> Items){
		
		Children.ForEach(child => Destroy(child));

		foreach( KeyValuePair<string,KeyValuePair<string, Guid>> IT in Items){
		GameObject obj = (GameObject) Instantiate(Item.gameObject);	
			obj.transform.parent = Grid.gameObject.transform;
			obj.transform.localScale = new Vector3(1,1,1);
			obj.SetActive(true);
			obj.GetComponent<InitializeItem>().InitialiseItem(IT.Value.Key,IT.Key, IT.Value.Value);
			obj.name = IT.Value.Key;
			Children.Add(obj);
		}
		try{
			this.gameObject.GetComponent<SpringPanel>().enabled = true;
			this.gameObject.GetComponent<SpringPanel>().target = new Vector3(0,-13,0);
		}
		catch{}
		Grid.repositionNow = true;
	}
	
	public void OnSubmit(string Result){
		Debug.Log(Result);
		//Debug.Log ("Empty:" +(Result == ""));
		Search(Result);
		
	}
	
	
	
	public void Search(string search){
		if(search != ""){
			Dictionary<string, KeyValuePair<string, Guid>> NewList = new Dictionary<string, KeyValuePair<string, Guid>>();
			foreach( KeyValuePair<string,KeyValuePair<string, Guid>> IT in ItemList){
				if (IT.Value.Key.Contains(search)){					
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