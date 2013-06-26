using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class TeacherMenuStudents : MonoBehaviour {
	public InitializeItem Item;
	public UIGrid Grid;
	
	public Dictionary<string, string> ItemList;
	List<GameObject> Children;
	
	// Use this for initialization
	public void DoStart () {
		Debug.Log("do start called");
		Children = new List<GameObject>();
		ItemList = new Dictionary<string, string>();
		CreateItems(ItemList);
		StartCoroutine(RetrieveItems());
	}
	
	public void DoClear()
	{
		Children.ForEach(child => Destroy(child));
	}
	
	void CreateItems(Dictionary<string,string> Items){
		
		Children.ForEach(child => Destroy(child));
		
		foreach( KeyValuePair<string,string> IT in Items.OrderBy(key=> key.Value)){
		GameObject obj = (GameObject) Instantiate(Item.gameObject);	
			obj.transform.parent = Grid.gameObject.transform;
			obj.transform.localScale = new Vector3(1,1,1);
			obj.SetActive(true);
			obj.GetComponent<InitializeItem>().InitialiseItem(IT.Key,IT.Value);
			obj.name = IT.Key;
			Children.Add(obj);
			Debug.Log(IT.Value);
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
	
	public IEnumerator RetrieveItems(){
		float elapsedTime = 0.0f;
		Debug.Log("SENDING FORM");
		
		byte[] b = {0x10, 0x20};
		Hashtable htbl = new Hashtable();
		htbl.Add("Session", GlobalSettings.SessionID);
		htbl.Add("Content-Type", "text/html");
		string url = GlobalSettings.serverURL + "town";
		WWW www;
		www = new WWW(url, b, htbl);
		
		while(!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 1.9f) break;
			yield return www;
		}	
		XmlDocument xml = new XmlDocument();
		xml.LoadXml(www.text);
		XmlNodeList List =  xml.SelectNodes("//ArrayOfTown/Town/Pupils/User");
		foreach( XmlNode XN in List){
			Debug.Log("name: " + XN["FullName"].InnerText);
			ItemList.Add(XN["FullName"].InnerText, XN.Attributes["UserGuid"].Value);
		}
		CreateItems(ItemList);
	}
}