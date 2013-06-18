using UnityEngine;
using System.Collections;

public class HighLightArrow : MonoBehaviour {

    Color InitialColor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
        InitialColor = gameObject.renderer.material.color;
        gameObject.renderer.material.color += Color.white;
    }

    void OnMouseOver()
    {
        //gameObject.renderer.material.color += Color.white;
    }

    void OnMouseExit()
    {
        gameObject.renderer.material.color = InitialColor;
    }
}
