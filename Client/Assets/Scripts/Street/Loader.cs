using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject go = GameObject.Find("GameController");
        if (go != null)
        {
            if (go.GetComponent<Street>() == null)
            {
                go.AddComponent<Street>();
            }
            if (go.GetComponent<LoadStreet>() == null)
            {
                LoadStreet ls = go.AddComponent<LoadStreet>();
                ls.HousePrefab = Resources.Load("Buildings/House/StreetHousePrefab") as GameObject;
                ls.Atlas = Resources.Load("Atlases/Fantasy/Fantasy Atlas") as UIAtlas;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
