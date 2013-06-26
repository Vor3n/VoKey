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
            Debug.Log("ShowLabel Changed to:[" + value + "]");
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
            // Restore Meshcolliders
            foreach (GameObject go in Street.HouseList)
            {
                MeshCollider mc = go.GetComponent<MeshCollider>();
                if (mc == null)
                {
                    go.AddComponent<MeshCollider>();
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
        Debug.Log("Creating Menu");

        // Create Room List
        Street.CreateRoomList(House.rooms);
    }

    
}
