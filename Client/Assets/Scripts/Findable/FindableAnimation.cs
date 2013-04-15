using UnityEngine;
using System.Collections;

public class FindableAnimaton : MonoBehaviour
{
	
	public bool findable = false;
	
	private bool found = false;
	private bool readyToAnim = false;
	private bool moveToAnim = false;
	
	
	Vector3 endPoint = new Vector3 (-5, 3, -6);
	float duration = 1.0f;
	
	private Vector3 startPoint;
	private float startTime;
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if ( moveToAnim){
			transform.position = Vector3.Lerp (startPoint, endPoint, (Time.time - startTime) / duration);
			if ( transform.position == endPoint){
				Debug.Log ("DONE MOVING");
				moveToAnim = false;
				readyToAnim = true;
			}
		}
		if ( readyToAnim){
			animation.Play("myanim");
			readyToAnim = false;
		}
	}
	
	void OnMouseUpAsButton ()
	{
		if ( findable ){
			found = true;
			//start moving the object to the startpoint of the animation
			startPoint = transform.position;
			startTime = Time.time;
			//start moving
			moveToAnim = true;
		}
	}
	
	void MoveAnimationDone ()
	{
		Destroy (gameObject);
	}
}
