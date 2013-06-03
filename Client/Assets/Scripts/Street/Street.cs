using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Street : MonoBehaviour {
    public static List<string> studentNames = new List<string>();
    public static Vector3 StartingCoordinates = new Vector3(1793, 1, 805);
    public static int Increment = 59, Space = 300; //56
    public static UIPanel RoomMenuPanel;

	// Use this for initialization
	void Start () {
        studentNames.Add("Duncan");
        studentNames.Add("Felix");
        studentNames.Add("Pascal");
        studentNames.Add("Dylan");
        studentNames.Add("Roy");

        RoomMenuPanel = GameObject.Find("RoomMenu").GetComponent<UIPanel>();
        RoomMenuPanel.alpha = 0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
