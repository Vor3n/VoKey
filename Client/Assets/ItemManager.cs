using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
public class ItemManager : MonoBehaviour {
	public InitializeItem Item;
	public UIGrid Grid;
	public string URL;
	
	public Dictionary<string, string> ItemList;
	List<GameObject> Children;
	
	// Use this for initialization
	void Start () {
		Children = new List<GameObject>();
		ItemList = new Dictionary<string, string>();
		
	
		CreateItems(ItemList);
		  StartCoroutine(RetrieveItems(URL));
   		
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
		try{
			this.gameObject.GetComponent<SpringPanel>().enabled = true;
			this.gameObject.GetComponent<SpringPanel>().target = new Vector3(0,-13,0);
		}
		catch{}
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
	
	public IEnumerator RetrieveItems(string url){
		float elapsedTime = 0.0f;
		WWW www;
		Debug.Log("SENDING FORM");
		
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
				Debug.Log("" + XN.Attributes["Name"].Value);
				ItemList.Add(XN.Attributes["Name"].Value, XN.Attributes["Hash"].Value);
	
			}
		}
		CreateItems(ItemList);
		//return "";
			
		/*string output;
		if (www.responseHeaders.TryGetValue("SESSION", out output))
		{
			GlobalSettings.SessionID = output;
			Debug.Log("SESSIONID: " + GlobalSettings.SessionID);
		}*/
	}
}