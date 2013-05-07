using UnityEngine;
using System.Collections;

public class SelectMovable : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUpAsButton()
    {
        Variables.Selected = gameObject;
        //Gizmo.MoveToSelected(Variables.Selected);
        Debug.Log("M-Selected: " + gameObject.name);
    }

    void OnTouch()
    {
        Variables.Selected = gameObject;
        Debug.Log("T-Selected: " + gameObject.name);
    }
}
