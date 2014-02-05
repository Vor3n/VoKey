using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using VokeySharedEntities;
using System.Xml;
public enum RoomMode { Editor, Game }
public class RoomManager : MonoBehaviour
{

    public Guid RoomID;
    AssetBundleManager abm;
    public string url = GlobalSettings.serverURL;
    Room TheRoom;

    public RoomMode OpenAs;
    // Use this for initialization
    void Start()
    {
		UnityEngine.Object[] objects = FindObjectsOfType(typeof(Rigidbody));
        foreach (UnityEngine.Object obj in objects)
        {
			Rigidbody temp = (Rigidbody) obj;
			temp.useGravity = false;
			temp.isKinematic = true;
		}
		RoomID=GameObject.Find("GameController").GetComponent<GameControllerScript>().RoomToOpen;
		url = GameObject.Find("GameController").GetComponent<GameControllerScript>().ServerURL;
		abm = GameObject.Find("GameController").GetComponent<AssetBundleManager>();
        LoadRoom(RoomID);
    }



    /// <summary>
    /// Loads the room.
    /// And places the items in the scene along with the required scripts and properties depending on the OpenAs property.
    /// </summary>
    /// <param name='id'>
    /// The Guid of the room.
    /// </param>
    public void LoadRoom(Guid id)
    {
        if (OpenAs == RoomMode.Editor)
        {
            ClearItemsFromRoom();
        }

        TheRoom = RetrieveRoom();

        List<string> itemsToFind = new List<string>();
		Debug.Log(abm);
        foreach (FindableObject FO in TheRoom.containedObjects)
        {
			
            GameObject g = (GameObject)GameObject.Instantiate(abm.RetrieveObject(FO.GameObjectId), FO.position,Quaternion.identity);
            g.transform.localScale = FO.scale;
            if (OpenAs == RoomMode.Editor)
            {
                g.AddComponent<Rigidbody>().useGravity = false;
                g.AddComponent<SelectMovable>();
                g.AddComponent<MeshCollider>();
                 g.AddComponent<BoxCollider>().size = new Vector3(1/FO.scale.x,1/FO.scale.y,1/FO.scale.z);
                g.rigidbody.freezeRotation = true;
                g.name = FO.GameObjectId;
            }
            else
            {
                g.AddComponent<Rigidbody>().useGravity = false;
                g.AddComponent<BoxCollider>().size = new Vector3(1/FO.scale.x,1/FO.scale.y,1/FO.scale.z);// Attach a collider to respond to clicking... still looking for a better solution than this.
                g.AddComponent<Animation>().animatePhysics = false;
                AnimationClip anim = Resources.Load("FindableClicked") as AnimationClip;
                g.animation.AddClip(anim, "FindableClicked");               
                FindableAnimation fa = g.AddComponent<FindableAnimation>();
                fa.findable = true;
				fa.ObjectName = abm.RetrieveObjectName(FO.GameObjectId);
                g.name = FO.GameObjectId;
				SayItemName tts = g.AddComponent<SayItemName>();
				tts.ItemName = abm.RetrieveObjectName(FO.GameObjectId);
                itemsToFind.Add(abm.RetrieveObjectName(FO.GameObjectId));//Make sure it appears in the list to be found.
            }
        }

        if (OpenAs == RoomMode.Game)
        {
            GameObject go = GameObject.Find("ItemList");
            go.GetComponent<CreateObjectiveList>().Init(itemsToFind.ToArray());
        }
    }
    /// <summary>
    /// Sends a serialized version of the current room to the webservice where it will be saved.
    /// </summary>
    public void SaveRoom()
    {
        ClearItemsFromRoom();//Clear the current items because else they will be duplicated
        TheRoom.containedObjects = new System.Collections.Generic.List<VokeySharedEntities.FindableObject>();
        UnityEngine.Object[] objects = FindObjectsOfType(typeof(SelectMovable));
        foreach (UnityEngine.Object obj in objects)
        {
			//Add each item to the room for serialization
            SelectMovable SM = (SelectMovable)obj;
            TheRoom.AddGameObject(SM.gameObject, SM.transform.position, SM.transform.localScale);

        }
		
		//Serialize the room object to xml and send it to the server
        string xml = TheRoom.ToXml();
        byte[] roomInfo = System.Text.Encoding.UTF8.GetBytes(xml);
        //float elapsedTime = 0.0f;
        WWW www;
        //Debug.Log("SENDING FORM");

        www = new WWW(url + "room/create", roomInfo);
        while (!www.isDone)
        {
           

        }
        


    }

    /// <summary>
    /// Clears all the items from room.
    /// </summary>
    void ClearItemsFromRoom()
    {
        try
        {
            UnityEngine.Object[] objects = FindObjectsOfType(typeof(SelectMovable));
            foreach (UnityEngine.Object obj in objects)
            {
                SelectMovable SM = (SelectMovable)obj;
                Destroy(SM.gameObject);//Destroy each object with component SelectMovable

            }
        }
        catch { }
    }

    /// <summary>
    /// Retrieves the room.
    /// </summary>
    /// <returns>
    /// The room.
    /// </returns>
    public Room RetrieveRoom()
    {
        // Use global room guid TODO
        WWW www = new WWW(url + "town/");
		Debug.Log(url + "town/");
        while (!www.isDone)
        {

        }
		XmlDocument xml = new XmlDocument();
		xml.LoadXml(www.text);
		string room = xml.SelectSingleNode("//Room[@Id='"+RoomID+"']").OuterXml;
		//Debug.Log(room);
        return MySerializerOfItems.FromXml<Room>(room);
    }


}
