using UnityEngine;
using System.Collections;

public class LoadStreet : MonoBehaviour
{
    public GameObject HousePrefab;

    // Use this for initialization
    void Start()
    {
        CreateSingleStreet(Street.StartingCoordinates);

        // onhover for the houses, each shows a name from array, when array is empty, further houses are 'free'
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateSingleStreet(Vector3 StartingCoordinates)
    {
        HousePrefab.transform.localScale = new Vector3(130, 130, 130);
        Quaternion rot = Quaternion.Euler(new Vector3(-90, -90, 0));

        for (int i = 0; i < Street.studentNames.Count; i++)
        {
            GameObject house = (GameObject)Instantiate(HousePrefab, StartingCoordinates + new Vector3(0, 0, i * Street.Increment), rot);
            house.name = Street.studentNames[i] + "'s House";

            // Assign a student name to the house
            house.AddComponent<MeshCollider>();
            house.AddComponent<OpenRoom>();
            house.GetComponent<OpenRoom>().Room = Street.studentNames[i];
        }
    }
}
