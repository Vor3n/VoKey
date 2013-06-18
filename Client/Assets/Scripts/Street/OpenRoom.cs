using UnityEngine;
using System.Collections;

public class OpenRoom : MonoBehaviour
{
    int i = 0;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        Debug.Log("HAI: " + i++);
    }
}
