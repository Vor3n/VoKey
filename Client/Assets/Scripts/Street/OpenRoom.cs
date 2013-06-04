using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenRoom : MonoBehaviour
{
    Color[] InitialColors;
    public string Owner;
    public bool ShowLabel
    {
        get
        {
            return showLabel;
        }
        set
        {
            showLabel = value;
            Debug.Log("ShowLabel Changed to:[" + value + "]");
        }
    }
    private bool showLabel = false;
    List<string> menuItems = new List<string>();
    public GameObject ButtonPrefab;
    public UIAtlas Atlas;
    private UIPanel listRoot;
    private UIPanel labelRoot;

    // Use this for initialization
    void Start()
    {
        listRoot = ((GameObject)GameObject.Find("Root")).GetComponent<UIPanel>();
        labelRoot = ((GameObject)GameObject.Find("LabelRoot")).GetComponent<UIPanel>();
        InitialColors = new Color[gameObject.renderer.materials.Length];

        labelRoot.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Restore Meshcolliders
            foreach (GameObject go in Street.houseList)
            {
                MeshCollider mc = go.GetComponent<MeshCollider>();
                if (mc == null)
                {
                    go.AddComponent<MeshCollider>();
                }
            }

            listRoot.alpha = 0;
            labelRoot.alpha = 0;
        }
    }

    void OnMouseEnter()
    {
        UILabel label = ((GameObject)GameObject.Find("OwnerLabel")).GetComponent<UILabel>();
        label.text = Owner;
        labelRoot.alpha = 1;

        for (int i = 0; i < gameObject.renderer.materials.Length; i++)
        {
            InitialColors[i] = gameObject.renderer.materials[i].color;
            gameObject.renderer.materials[i].color += Color.white;
        }
    }

    void OnMouseExit()
    {
        if (listRoot.alpha == 0)
        {
            labelRoot.alpha = 0;
        }

        for (int i = 0; i < gameObject.renderer.materials.Length; i++)
        {
            gameObject.renderer.materials[i].color = InitialColors[i];
        }
    }

    void OnMouseUpAsButton()
    {
        // Get List of Rooms for Student
        // TODO

        // Create Room List
        CreateRoomList(null);
    }

    void CreateRoomList(List<Room> roomList)
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
        list.selection = "Please choose a room..";

        
        listRoot.alpha = 1;
    }
}
