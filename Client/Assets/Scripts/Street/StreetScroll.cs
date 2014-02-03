using UnityEngine;
using System.Collections;

public class StreetScroll : MonoBehaviour
{
    public enum ScrollDirection
    {
        Left,
        Right
    }

    Color originalColor;
    UISprite sprite;
    bool Moving = false;
    Vector3 TerrainSize;

    public ScrollDirection Direction;

    // Use this for initialization
    void Start()
    {
        // Initialize the boundaries
        Terrain terra = ((GameObject)GameObject.Find("Earth")).GetComponent<Terrain>();
        TerrainSize = new Vector3(terra.terrainData.size.x, terra.terrainData.size.y, terra.terrainData.size.z);

        sprite = gameObject.GetComponent<UISprite>();
    }

    // Update is called once per frame
    void Update()
    {
		Debug.Log("is moving: " + Moving);
        if (Moving)
        {
            MoveCamera();
        }
    }

    void OnHover(bool isOver)
    {
		Debug.Log("OnHover");
        if (isOver)
        {
            if (sprite != null)
            {
                sprite.alpha = 0.3f;

                Moving = true;
            }
        }
        else
        {
            sprite.alpha = 1f;

            Moving = false;
        }
    }

    void MoveCamera()
    {
		Debug.Log("Moving camera on street");
        Camera cam = ((GameObject)GameObject.Find("Main Camera")).GetComponent<Camera>();
		
        switch (Direction)
        {
            case ScrollDirection.Left:
                if(cam.transform.position.z < TerrainSize.z - 1650)//275)
                {
                    cam.transform.position += new Vector3(0, 0, 5);
                }
                break;
            case ScrollDirection.Right:
                if (cam.transform.position.z > 225)
                {
                    cam.transform.position += new Vector3(0, 0, -5);
                }
                break;
        }
    }
}
