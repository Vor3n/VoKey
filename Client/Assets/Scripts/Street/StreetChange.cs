using UnityEngine;
using System.Collections;

public class StreetChange : MonoBehaviour {
    public enum StreetChangeDirection
    {
        Up,
        Down
    }

    public StreetChangeDirection Direction;
    Vector3 TerrainSize;

	// Use this for initialization
	void Start () {
        // Initialize the boundaries
        Terrain terra = ((GameObject)GameObject.Find("Earth")).GetComponent<Terrain>();
        TerrainSize = new Vector3(terra.terrainData.size.x, terra.terrainData.size.y, terra.terrainData.size.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        Camera cam = ((GameObject)GameObject.Find("Main Camera")).GetComponent<Camera>();
        switch (Direction)
        {
            case StreetChangeDirection.Up:
                if (cam.transform.position.x < TerrainSize.x - 1600)
                {
                    cam.transform.position += new Vector3(300, 0, 0);
                }
                break;
            case StreetChangeDirection.Down:
                if (cam.transform.position.x > TerrainSize.x - (7 * 300))
                {
                    cam.transform.position -= new Vector3(300, 0, 0);
                }
                break;
        }
    }
}
