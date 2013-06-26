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
        // Get the list
        GameObject listObject = (GameObject)GameObject.Find("RoomList");
        UIPopupList list = listObject.GetComponent<UIPopupList>();

        Debug.Log("HAI: " + list.selection); // Find out which index we currently have selected
    }
}
