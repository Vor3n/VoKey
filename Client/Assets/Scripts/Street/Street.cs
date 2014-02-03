using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VokeySharedEntities;

public class Street : MonoBehaviour {
    public static List<VokeySharedEntities.Street> Streets = new List<VokeySharedEntities.Street>();
    public static List<Room> CurrentRooms = null;
    public static Assignment CurrentAssignment = null;
    public static Vector3 StartingCoordinates = new Vector3(1793, 1, 75);
    public static Vector3 RoadCoordinates = new Vector3(1720, 1, 1000);
    public static int HouseIncrement = 50, StreetIncrement = 300;
    public static UIPanel ListRoot;
    public static bool Loaded = false;

	// Use this for initialization
	void Start () {
        Debug.Log("Street::Start");
        ListRoot = ((GameObject)GameObject.Find("RoomListPanel")).GetComponent<UIPanel>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void CreateRoomList(List<Room> roomList)
    {
        CurrentRooms = new List<Room>(roomList);
        GameObject listObject = (GameObject)GameObject.Find("RoomList");
        UIPopupList list = listObject.GetComponent<UIPopupList>();

        // Remove street colliders to avoid triggering house onclick through the menu
        GameObject[] objects = (GameObject[])GameObject.FindSceneObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
        {
            if (go.name == "RoomList" || go.name == "RoomListButton") continue;

            MeshCollider m = go.GetComponent<MeshCollider>();
            if (m != null)
            {
                m.enabled = false;
            }

            BoxCollider b = go.GetComponent<BoxCollider>();
            if (b != null)
            {
                b.enabled = false;
            }
        }

        // Clear menu
        list.items.Clear();

        // Read Rooms belonging to student
        foreach (Room room in roomList)
        {
            list.items.Add(room.name);
        }

        list.highlightColor = new Color(97f / 255f, 162f / 255f, 255f / 255f, 255f / 255f);
        list.selection = "Choose room..";


        ListRoot.alpha = 1;
    }
}
