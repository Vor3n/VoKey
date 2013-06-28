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
		abm = GameObject.Find("GameController").GetComponent<AssetBundleManager>();
     
            
            LoadRoom(Guid.Empty);
        
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
                g.transform.position = new Vector3(0, 6.5f, -5f);
                g.rigidbody.freezeRotation = true;
                g.name = FO.GameObjectId;
            }
            else
            {
                g.AddComponent<Rigidbody>().useGravity = false;
                g.AddComponent<BoxCollider>().size = new Vector3(1/FO.scale.x,1/FO.scale.y,1/FO.scale.z);
                g.AddComponent<Animation>().animatePhysics = false;
                AnimationClip anim = Resources.Load("FindableClicked") as AnimationClip;
                g.animation.AddClip(anim, "FindableClicked");
                g.AddComponent<Animator>();
                FindableAnimation fa = g.AddComponent<FindableAnimation>();
                fa.findable = true;
				fa.ObjectName = abm.RetrieveObjectName(FO.GameObjectId);
                g.name = FO.GameObjectId;
                //fa.Name = FO.GameObjectId;

                itemsToFind.Add(abm.RetrieveObjectName(FO.GameObjectId));
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
        ClearItemsFromRoom();
        TheRoom.containedObjects = new System.Collections.Generic.List<VokeySharedEntities.FindableObject>();
        UnityEngine.Object[] objects = FindObjectsOfType(typeof(SelectMovable));
        foreach (UnityEngine.Object obj in objects)
        {
            SelectMovable SM = (SelectMovable)obj;
            TheRoom.AddGameObject(SM.gameObject, SM.transform.position, SM.transform.localScale);

        }

        string xml = TheRoom.ToXml();
        byte[] roomInfo = System.Text.Encoding.UTF8.GetBytes(xml);
        float elapsedTime = 0.0f;
        WWW www;
        Debug.Log("SENDING FORM");

        www = new WWW(url + "room/create", roomInfo);
        while (!www.isDone)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 1.9f) break;

        }
        Debug.Log(xml);


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
                Destroy(SM.gameObject);

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
        WWW www = new WWW(url + "room/hoer");

        while (!www.isDone)
        {

        }
		
        return MySerializerOfItems.FromXml<Room>(www.text);
    }


}
