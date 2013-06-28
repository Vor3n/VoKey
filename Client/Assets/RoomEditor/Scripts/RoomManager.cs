using UnityEngine;
using System.Collections;
using System;
using VokeySharedEntities;
using System.Xml;
public enum RoomMode { Editor, Game}
public class RoomManager : MonoBehaviour {
	
	public Guid RoomID;
	AssetBundleManager abm;
	public string url =GlobalSettings.serverURL;
	Room TheRoom;
	public RoomMode OpenAs;
	// Use this for initialization
	void Start () {
		abm = GameObject.Find("EditorController").GetComponent<AssetBundleManager>();
		TheRoom = new Room("goudvis");
		RoomID = TheRoom.id;
	}
	
	// Update is called once per frame

	
	
	public void LoadRoom(Guid id){
		ClearItemsFromRoom();
		
		TheRoom = RetrieveRoom();
		
		foreach(FindableObject FO in TheRoom.containedObjects){
			GameObject g = (GameObject) GameObject.Instantiate(abm.RetrieveObject(FO.GameObjectId),FO.position,FO.rotation);
			g.transform.localScale = FO.scale;
			
		}
	}
	
	public void SaveRoom(){
		ClearItemsFromRoom();
		 TheRoom.containedObjects = new System.Collections.Generic.List<VokeySharedEntities.FindableObject>();
		UnityEngine.Object[] objects = FindObjectsOfType(typeof(SelectMovable));
		foreach( UnityEngine.Object obj in objects){
			SelectMovable SM = (SelectMovable) obj;
			TheRoom.AddGameObject(SM.gameObject,SM.transform.position,SM.transform.localScale);
			
		}
		
		string xml = TheRoom.ToXml();
		byte[] roomInfo = System.Text.Encoding.UTF8.GetBytes (xml);
		float elapsedTime = 0.0f;
		WWW www;
		Debug.Log("SENDING FORM");
		
		www = new WWW(url + "/room/create",roomInfo);
		while(!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 1.9f) break;
			
		}
		Debug.Log(xml);
		
		
	}
	
	void ClearItemsFromRoom(){
		
		UnityEngine.Object[] objects = FindObjectsOfType(typeof(SelectMovable));
		foreach( UnityEngine.Object obj in objects){
			SelectMovable SM = (SelectMovable) obj;
			Destroy(SM.gameObject);
			
		}
	}
	
	public Room RetrieveRoom(){
		float elapsedTime = 0.0f;
		WWW www;
		Debug.Log("SENDING FORM");
		
		www = new WWW(url + "/room/" +RoomID);
		while(!www.isDone)
		{
			//elapsedTime += Time.deltaTime;
			//if (elapsedTime >= 1.9f) break;
			
		}
		
		
		
		return MySerializerOfItems.FromXml<Room>(www.text);	
	}
	
	
}
