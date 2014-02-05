using UnityEngine;
using System.Collections;

public class SunSimulation : MonoBehaviour {

	// Use this for initialization
	GameObject sun;
	
	void Start () {
		sun = GameObject.Find("Sun");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		sun.transform.Rotate(Vector3.up * Time.deltaTime * 8.0f);
	}
}
