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
        Vector3 roadPosition = Street.RoadCoordinates;
        Vector3 streetPosition = Street.StartingCoordinates;

        // Get town object
        town = GetVokeyObject<Town>("town");
        if (town != null)
        {
            //Debug.Log("Town: " + town.name + ", ID: " + town.id);
            int counter = 0;
            foreach (VokeySharedEntities.Street s in town.streets)
            {
                if (counter++ == 2) break;
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

        //Debug.Log("Received Object XML");
        result = response.text;
        IsDone = response.isDone;
    }

    void CreateSingleStreet(Vector3 StartingCoordinates, List<House> Houses)
    {
        //Debug.Log("Creating a Street with " + Houses.Count + " Houses");
        Quaternion rot = Quaternion.Euler(new Vector3(-90, -90, 0));
        for (int i = 0; i < Houses.Count; i++)
        {
            GameObject house = (GameObject)Instantiate(HousePrefab, StartingCoordinates + new Vector3(0, 0, i * Street.HouseIncrement), rot);
            house.name = Houses[i].name;
            //Debug.Log("House @ " + house.transform.position);

            // Assign a student name to the house
            house.AddComponent<MeshCollider>();
            house.AddComponent<StreetHouse>();
            house.GetComponent<StreetHouse>().Atlas = Atlas;
            house.GetComponent<StreetHouse>().House = Houses[i];

            Street.HouseList.Add(house);

            //ChangeHousePartColor (house, "WaLL", new Color (0.3f, 0.3f, 0.0f));
            //ChangeHousePartColor (house, "Door", new Color (1.0f, 0.5f, 0.0f));
            //ChangeHousePartColor (house, "roof", new Color (123, 0.0f, 1.0f));
            //ChangeHousePartColor (house, "windows", new Color (0.5f, 0.5f, 255, 1.0f));
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
