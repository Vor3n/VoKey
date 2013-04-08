using UnityEngine;
using System.Collections;

public class CameraFreeMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public float speed = 10.0f;
	void Update () {
		Vector3 movement = Vector3.zero;
		movement.z = Input.GetAxis("Vertical");
		movement.x = Input.GetAxis("Horizontal");
//		movement.y = Input.GetButton("Jump");
		
		transform.Translate(movement * speed * Time.deltaTime, Space.Self);
	
	}
}
