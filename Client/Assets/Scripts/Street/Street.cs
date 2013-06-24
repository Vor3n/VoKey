using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VokeySharedEntities;

public class Street : MonoBehaviour {
    public static List<string> studentNames = new List<string>();
    public static List<GameObject> houseList = new List<GameObject>();
    public static Vector3 StartingCoordinates = new Vector3(1793, 1, 805);
    public static int Increment = 59, Space = 300; //56
    public static UIPanel ListRoot;

    static List<string> menuItems = new List<string>();

	// Use this for initialization
	void Start () {
        studentNames.Add("Duncan");
        studentNames.Add("Felix");
        studentNames.Add("Pascal");
        studentNames.Add("Dylan");
        studentNames.Add("Roy");

        ListRoot = ((GameObject)GameObject.Find("MenuRoot")).GetComponent<UIPanel>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void CreateRoomList(List<Room> roomList)
    {
        GameObject listObject = (GameObject)GameObject.Find("RoomList");
        UIPopupList list = listObject.GetComponent<UIPopupList>();

        // Remove street colliders to avoid triggering house onclick through the menu
        foreach (GameObject go in Street.houseList)
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

        menuItems.Add("Kitchen");
        menuItems.Add("Bedroom");
        menuItems.Add("Living Room");
        menuItems.Add("Hall");

        foreach (string item in menuItems)
        {
            list.items.Add(item);
        }

        // Read Rooms belonging to student
        /*
        foreach (Room room in roomList)
        {
            list.items.Add(room.name);
        }*/

        list.highlightColor = Color.blue + Color.cyan;
        list.selection = "Choose room..";


        ListRoot.alpha = 1;
    }
}
