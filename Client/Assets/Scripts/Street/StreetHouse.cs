using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VokeySharedEntities;

public class StreetHouse : MonoBehaviour
{
    Color[] InitialColors;
    public House House;
    public bool ShowLabel
    {
        get
        {
            return showLabel;
        }
        set
        {
            showLabel = value;
            //Debug.Log("ShowLabel Changed to:[" + value + "]");
        }
    }
    private bool showLabel = false;
    
    public GameObject ButtonPrefab;
    public UIAtlas Atlas;
    
    private UIPanel labelRoot;

    // Use this for initialization
    void Start()
    {
        labelRoot = ((GameObject)GameObject.Find("OwnerLabelPanel")).GetComponent<UIPanel>();
        InitialColors = new Color[gameObject.renderer.materials.Length];

        labelRoot.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Empty current room list
            Street.CurrentRooms = null;

            // Restore Meshcolliders
            GameObject[] objects = (GameObject[])GameObject.FindSceneObjectsOfType(typeof(GameObject));
            foreach (GameObject go in objects)
            {
                if (go.name == "RoomList" || go.name == "RoomListButton") continue;

                MeshCollider m = go.GetComponent<MeshCollider>();
                if (m != null)
                {
                    m.enabled = true;
                }

                BoxCollider b = go.GetComponent<BoxCollider>();
                if (b != null)
                {
                    b.enabled = true;
                }
            }

            Street.ListRoot.alpha = 0;
            labelRoot.alpha = 0;
        }
    }

    void OnMouseEnter()
    {
        UILabel label = ((GameObject)GameObject.Find("OwnerLabel")).GetComponent<UILabel>();
        label.text = House.name;
        labelRoot.alpha = 1;

        for (int i = 0; i < gameObject.renderer.materials.Length; i++)
        {
            InitialColors[i] = gameObject.renderer.materials[i].color;
            gameObject.renderer.materials[i].color += Color.white;
        }
    }

    void OnMouseExit()
    {
        if (Street.ListRoot.alpha == 0)
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
        // Create Room List
        Street.CreateRoomList(House.rooms);
    }

    
}
