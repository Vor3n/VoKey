using UnityEngine;
using System.Collections;
using System;
using VokeySharedEntities;
using System.Xml;
public class RoomManager : MonoBehaviour {
	
	public Guid RoomID;
	public string url;
	Room TheRoom;
	// Use this for initialization
	void Start () {
	TheRoom = new Room();
	RoomID = TheRoom.id;
	}
	
	// Update is called once per frame

	
	
	public void LoadRoom(Guid id){
		ClearItemsFromRoom();
		
		TheRoom = RetrieveRoom();
		
		foreach(FindableObject FO in TheRoom.containedObjects){
			//Do stuff that adds the objects to the room...
		}
	}
	
	public void SaveRoom(){
		
		 TheRoom.containedObjects = new System.Collections.Generic.List<VokeySharedEntities.FindableObject>();
		UnityEngine.Object[] objects = FindObjectsOfType(typeof(SelectMovable));
		foreach( UnityEngine.Object obj in objects){
			SelectMovable SM = (SelectMovable) obj;
			TheRoom.AddGameObject(SM.gameObject,SM.transform.position,SM.transform.localScale);
			
		}
		
		string xml = Room.ToXml(TheRoom);
		byte[] roomInfo = System.Text.Encoding.UTF8.GetBytes (xml);
		float elapsedTime = 0.0f;
		WWW www;
		Debug.Log("SENDING FORM");
		
		www = new WWW(url + "/room/" +RoomID,roomInfo);
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
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 1.9f) break;
			
		}
		
		XmlDocument xml = new XmlDocument();
		xml.LoadXml(www.text);
		
		return Room.FromXml(xml);
		
	}
	
	
}