using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VokeySharedEntities;

public class Street : MonoBehaviour {
    public static List<string> studentNames = new List<string>();
    public static List<VokeySharedEntities.Street> Streets = new List<VokeySharedEntities.Street>();
    public static List<GameObject> HouseList = new List<GameObject>();
    public static Vector3 StartingCoordinates = new Vector3(1793, 1, 75);
    public static Vector3 RoadCoordinates = new Vector3(1720, 1, 1000);
    public static int HouseIncrement = 50, StreetIncrement = 300;
    public static UIPanel ListRoot;

    static List<string> menuItems = new List<string>();

	// Use this for initialization
	void Start () {
        ListRoot = ((GameObject)GameObject.Find("RoomListPanel")).GetComponent<UIPanel>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void CreateRoomList(List<Room> roomList)
    {
        GameObject listObject = (GameObject)GameObject.Find("RoomList");
        UIPopupList list = listObject.GetComponent<UIPopupList>();

        // Remove street colliders to avoid triggering house onclick through the menu
        foreach (GameObject go in Street.HouseList)
        {
            MeshCollider mc = go.GetComponent<MeshCollider>();
            if (mc != null)
            {
                Object.Destroy(mc);
            }
        }

        // Clear menu
        list.items.Clear();
        menuItems.Clear();

        // Read Rooms belonging to student
        foreach (Room room in roomList)
        {
            list.items.Add(room.name);
        }

        list.highlightColor = Color.blue + Color.cyan;
        list.selection = "Choose room..";


        ListRoot.alpha = 1;
    }
}
