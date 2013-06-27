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
	
	public Dictionary<string, KeyValuePair<string, Guid>> ItemList;
	List<GameObject> Children;
	
	// Use this for initialization
	void Start () {
		Children = new List<GameObject>();
		ItemList = new Dictionary<string, KeyValuePair<string, Guid>>();
		
	
		CreateItems(ItemList);
		  StartCoroutine(RetrieveItems(URL));
   		
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
	
	public IEnumerator RetrieveItems(string url){
		float elapsedTime = 0.0f;
		WWW www;
		//Debug.Log("SENDING FORM");
		
	www = new WWW(url + "/assetbundle" );
		while(!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 1.9f) break;
			yield return www;
		}
		//isDone = www.isDone;
		//response = www.responseHeaders;
		
		//Debug.Log("hoi"+www.text);		
		XmlDocument xml = new XmlDocument();
		xml.LoadXml(www.text);
		XmlNodeList List =  xml.SelectNodes("//ArrayOfVokeyAssetBundle/VokeyAssetBundle/VokeyAssets/VokeyAsset");
		foreach( XmlNode XN in List){
			//Debug.Log("" + XN.Attributes["Name"].Value);	
			if(XN.Attributes["Type"].Value == "UnityEngine.GameObject"){
				Guid BundleId = new Guid(XN.SelectSingleNode("../..").Attributes["ModelId"].Value);
				//Debug.Log("" + XN.Attributes["Name"].Value);
				ItemList.Add( XN.Attributes["Hash"].Value,new KeyValuePair<string, Guid>(XN.Attributes["Name"].Value,BundleId));
				
	
			}
		}
		CreateItems(ItemList);
	
	}
}