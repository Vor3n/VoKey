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

    }

    void FixedUpdate()
    {
        if (Moving)
        {
            MoveCamera();
        }
    }

    void OnHover(bool isOver)
    {
        if (isOver)
        {
            if (sprite != null)
            {
                sprite.alpha = 0.3f;
                //originalColor = sprite.material.color;
                //sprite.material.color += (Color.white * 2);

                Moving = true;
            }
        }
        else
        {
            sprite.alpha = 1f;
            //sprite.material.color = originalColor;

            Moving = false;
        }
    }

    void MoveCamera()
    {
        Camera cam = ((GameObject)GameObject.Find("Main Camera")).GetComponent<Camera>();
        switch (Direction)
        {
            case ScrollDirection.Left:
                if(cam.transform.position.z < TerrainSize.z - 275)
                {
                    cam.transform.position += new Vector3(0, 0, 5);
                }
                break;
            case ScrollDirection.Right:
                if (cam.transform.position.z > 275)
                {
                    cam.transform.position += new Vector3(0, 0, -5);
                }
                break;
        }
    }

    /*
        void OnMouseEnter()
        {
            // Show highlight
            UISprite sprite = gameObject.GetComponent<UISprite>();
            Debug.Log("mouse enter sprite: " + sprite);
            if (sprite != null)
            {
                sprite.material.color += Color.white;
            }

            // Start scrolling (remember to stop at bounds)
            switch (Direction)
            {
                case ScrollDirection.Left:
                    break;
                case ScrollDirection.Right:
                    break;
            }
        }

        void OnMouseExit()
        {
            // Stop scrolling

            // Remove highlight
        }
    */
}
