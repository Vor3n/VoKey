using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreetHouse : MonoBehaviour
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

        // Get List of Rooms for Student
        // TODO

        // Create Room List
        Street.CreateRoomList(null);
    }
}
