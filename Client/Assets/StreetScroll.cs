using UnityEngine;
using System.Collections;

public class StreetScroll : MonoBehaviour
{
    public enum ScrollDirection
    {
        Left,
        Right
    }

    public ScrollDirection Direction;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    void OnMouseEnter()
    {
        // Show highlight
        gameObject.renderer.material.color += Color.white;

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
}
