using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VokeySharedEntities;
using System.ComponentModel;
using System.Threading;

public class LoadStreet : MonoBehaviour
{
    public GameObject HousePrefab;
    public UIAtlas Atlas;
    private static bool IsDone = false;
    private Town town = null;
    private string result = "";

    // Use this for initialization
    void Start()
    {
        Debug.Log("LoadStreet::Start");

        Street.Streets.Clear();

        Vector3 roadPosition = Street.RoadCoordinates;
        Vector3 streetPosition = Street.StartingCoordinates;

        // Get town object
        town = GetVokeyObject<Town>("town");
        if (town != null)
        {
            foreach (VokeySharedEntities.Street s in town.streets)
            {
                Debug.Log("Street: " + s.name + ", ID: " + s.id + ", Houses: " + s.houses.Count);
                VokeySharedEntities.Street street = GetVokeyObject<VokeySharedEntities.Street>("town/" + town.id + "/street/" + s.id);
                Street.Streets.Add(street);
            }

            // foreach street contained in town object
            for (int i = 0; i < Street.Streets.Count; i++)
            {
                // Create road
                GameObject road = GameObject.CreatePrimitive(PrimitiveType.Plane);
                road.transform.position = roadPosition;
                road.transform.localScale = new Vector3(5, 1, 200);
                road.name = "Road";
                MeshRenderer mr = road.GetComponent<MeshRenderer>();
                mr.material = Resources.Load("Materials/Concrete_Block_8x8_Gray_") as Material;

                // Create the streets
                CreateSingleStreet(streetPosition, Street.Streets[i].houses);
                streetPosition.x -= Street.StreetIncrement;
                roadPosition.x -= Street.StreetIncrement;
            }
        }

        // Request current users assignments
        AssignmentList assignments = GetVokeyObject<AssignmentList>("assignment");

        // Scale and position Sprite as background
        UISprite backgroundSprite = ((GameObject)GameObject.Find("AssignmentListBackground")).GetComponent<UISprite>();
        backgroundSprite.transform.localScale = new Vector3(300, assignments.TodoAssignments.Count * 45 + 45);

        // Create and position labels (assignments)
        for (int i = 0; i < assignments.TodoAssignments.Count; i++)
        {
            GameObject Anchor = (GameObject)GameObject.Find("AssignmentAnchor");
            // Create the button
            GameObject assignment = new GameObject("assignment");
            assignment.AddComponent<BoxCollider>();
            assignment.AddComponent<UIButton>();
            assignment.AddComponent<UIButtonScale>();
            assignment.AddComponent<UIButtonOffset>();
            assignment.AddComponent<UIButtonSound>();
            AssignmentClicked clicked = assignment.AddComponent<AssignmentClicked>();
            clicked.assignment = assignments.TodoAssignments[i];
            assignment.transform.parent = Anchor.transform;
            assignment.layer = Anchor.layer;
            assignment.transform.localScale = new Vector3(1, 1, 1);
            assignment.transform.position = new Vector3(42, -60 - (i * 45), 1); // -15
            assignment.transform.localPosition = assignment.transform.position;

            // Create the label
            GameObject lbl = new GameObject("assignment-label");
            lbl.layer = Anchor.layer;
            lbl.AddComponent<UILabel>();

            UILabel label = lbl.GetComponent<UILabel>();
            label.transform.parent = assignment.transform;
            label.pivot = UIWidget.Pivot.TopLeft;
            label.transform.position = new Vector3(0, 0, 1);
            label.transform.localPosition = label.transform.position;
            label.transform.localScale = new Vector3(28, 28, 1);
            label.depth = 1;
            label.text = assignments.TodoAssignments[i].name;
            label.font = ((GameObject)Resources.Load("Atlases/Fantasy/Fantasy Font - Normal")).GetComponent<UIFont>();
            /*
            // Create the sprite
            GameObject sprt = new GameObject("assignment-sprite");
            sprt.AddComponent<UISprite>();

            UISprite sprite = sprt.GetComponent<UISprite>();
            sprite.transform.parent = assignment.transform;
            sprite.pivot = UIWidget.Pivot.TopLeft;
            sprite.transform.position = new Vector3(42, -15 - (i * 45), 1);
            sprite.transform.localPosition = sprite.transform.position;
            sprite.transform.localScale = new Vector3(label.transform. */

            // Set up the box collider
            BoxCollider box = assignment.GetComponent<BoxCollider>();
            box.center = new Vector3(140, -15, 0); // -15
            box.size = new Vector3(280, 28, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public T GetVokeyObject<T>(string r)
    {
        T vokeyObject = default(T);

        GetObject(r);

        while (!IsDone)
        {

        }

        if (result != null && result != string.Empty)
        {
            vokeyObject = MySerializerOfItems.FromXml<T>(result);
            result = null;
        }
        else
        {
            // Error
            Debug.Log("Result is empty / null");
        }

        return vokeyObject;
    }

    public void GetObject(string request)
    {
        byte[] b = { 0x10, 0x20 };
        Hashtable hash = new Hashtable();
        hash.Add("Session", GlobalSettings.SessionID);
        hash.Add("Content-Type", "text/html");
        WWW response = new WWW(GlobalSettings.serverURL + request, b, hash);
        while (!response.isDone)
        {
            if (!string.IsNullOrEmpty(response.error))
            {
                //Messenger.Broadcast(VokeyMessage.REQUEST_FAIL, response.error);
                return;
            }
        }

        result = response.text;
        IsDone = response.isDone;
    }

    void CreateSingleStreet(Vector3 StartingCoordinates, List<House> Houses)
    {
        Quaternion rot = Quaternion.Euler(new Vector3(-90, -90, 0));
        for (int i = 0; i < Houses.Count; i++)
        {
            GameObject house = (GameObject)Instantiate(HousePrefab, StartingCoordinates + new Vector3(0, 0, i * Street.HouseIncrement), rot);
            house.name = Houses[i].name;

            // Assign a student name to the house
            house.AddComponent<MeshCollider>();
            house.AddComponent<StreetHouse>();
            house.GetComponent<StreetHouse>().Atlas = Atlas;
            house.GetComponent<StreetHouse>().House = Houses[i];
        }
    }

    /// <summary>
    /// Changes the color of a house part.
    /// </summary>
    /// <param name='house'>
    /// The house to change the color of.
    /// </param>
    /// <param name='newColor'>
    /// The new color of the house part.
    /// The RGB values have to be between 1.0f and 255.0f and/or normalized between 0,0f and 1,0f
    /// </param>
    /// <param name='housePart'>
    /// The part of the house to recolor. Part names are : 'Wall', 'Door', 'Roof', 'Windows', 'Venster'
    /// </param>
    void ChangeHousePartColor(GameObject house, string housePart, Color newColor)
    {
        Color normalizedColor = NormalizeColor(newColor);
        if (housePart.ToLower().Equals("windows"))
        { // we want to have the windows always the same alfa value
            normalizedColor.a = 100.0f / 255.0f;
        }
        // iterate through the materials until the right material has been found
        for (int iMat = 0; iMat < house.renderer.materials.Length; iMat++)
        {
            if (house.renderer.materials[iMat].name.ToLower().StartsWith(housePart.ToLower()))
            {
                house.renderer.materials[iMat].SetColor("_Color", normalizedColor);
            }
        }
    }

    /// <summary>
    /// Normalizes the color. ( a value between 0.0f and 1.0f )
    /// </summary>
    /// <returns>
    /// The Normalized color.
    /// </returns>
    /// <param name='color'>
    /// A color.
    /// </param>
    private Color NormalizeColor(Color color)
    {
        Color newColor = new Color(color.r, color.g, color.b, color.a);
        while (newColor.r > 1.0f || newColor.g > 1.0f || newColor.b > 1.0f)
        {
            if (newColor.r > 1.0f)
            {
                newColor.r = newColor.r / 255.0f;
            }
            if (newColor.g > 1.0f)
            {
                newColor.g = newColor.g / 255.0f;
            }
            if (newColor.b > 1.0f)
            {
                newColor.b = newColor.b / 255.0f;
            }
        }
        Debug.Log("Normalized RGB: " + newColor.r + "," + newColor.g + "," + newColor.b);
        return newColor;
    }

}
