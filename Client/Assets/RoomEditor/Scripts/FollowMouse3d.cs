using UnityEngine;
using System.Collections;

public class FollowMouse3d : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 yolo = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y,0));
		this.transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y,0));
		//Debug.Log("x: " + yolo.x + ",y: " +  + yolo.y + ",z: " +  + yolo.z);
	}
}
