using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VokeySharedEntities;

public class TeacherMenuTowns : MonoBehaviour {
	public InitializeTownItem Item;
	public UIGrid Grid;
	
	public Dictionary<string, string> ItemList;
	List<GameObject> Children;
	public GameObject selectedItem;
	GameObject previouslySelected;
	Color colorItem;
	
	// Use this for initialization
	public void DoStart () {
		Children = new List<GameObject>();
		ItemList = new Dictionary<string, string>();
		CreateItems(ItemList);
		StartCoroutine(RetrieveItems());
	}
	
	public void DoClear()
	{
		Children.ForEach(child => Destroy(child));
	}
	
	void Update()
	{
		if (previouslySelected != selectedItem)
		{
			try
			{
				previouslySelected.GetComponent<InitializeTownItem>().clearSelected();
			}
			catch
			{}
			previouslySelected = selectedItem;
			selectedItem.GetComponent<InitializeTownItem>().setSelected();
		}
	}
	
	void CreateItems(Dictionary<string,string> Items){
		
		Children.ForEach(child => Destroy(child));
		
		foreach( KeyValuePair<string,string> IT in Items.OrderBy(key => key.Key)){
			GameObject obj = (GameObject) Instantiate(Item.gameObject);	
			obj.transform.parent = Grid.gameObject.transform;
			obj.transform.localScale = new Vector3(1,1,1);
			obj.SetActive(true);
			obj.GetComponent<InitializeTownItem>().InitialiseItem(IT.Value,IT.Key);
			obj.name = IT.Value;
			Children.Add(obj);
		}
		try{
			this.gameObject.GetComponent<SpringPanel>().enabled = true;
			this.gameObject.GetComponent<SpringPanel>().target = new Vector3(-136.70f,107.86f,0);
		}
		catch
		{
			this.gameObject.AddComponent<SpringPanel>();
			this.gameObject.GetComponent<SpringPanel>().enabled = true;
			this.gameObject.GetComponent<SpringPanel>().target = new Vector3(-136.7016f,107.8612f,0);
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
		
		GameObject.Find("GameController").GetComponent<GameControllerScript>().towns = MySerializerOfLists.FromXml<Town>(www.text);
		XmlNodeList List =  xml.SelectNodes("//ArrayOfTown/Town");
		foreach( XmlNode XN in List){
			Debug.Log("" + XN.Attributes["Name"].Value);
			ItemList.Add(XN.Attributes["Name"].Value, XN.Attributes["ClassroomName"].Value);
		}
		CreateItems(ItemList);
	}
}