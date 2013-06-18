using UnityEngine;
using System.Collections;

public class Variables : MonoBehaviour
{
    public static GameObject Selected;

    // Use this for initialization
    void Start()
    {
        
    }
    
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Variables.Selected = null;
        }
    }
}
