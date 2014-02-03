using UnityEngine;
using System.Collections;

public class FindableAnimation : MonoBehaviour
{
	public string ObjectName;
	public bool findable = false;
	
	private bool found = false;
	private bool inAnimPosition = false;
	private bool moveToAnimPosition = false;
	private bool soundAvailable = false;
	
	Vector3 endPoint = new Vector3 (-5, 3, -6);
	float duration = 1.0f;
	
	private Vector3 startPoint;
	private float startTime;
	
	// Use this for initialization
	void Start ()
	{
			endPoint = new Vector3 (-5, 3, -6);
			found = false; 
	 		inAnimPosition = false;
 			moveToAnimPosition = false;
	 		soundAvailable = false;
		
		if ( audio != null ){
			if ( audio.clip != null ) { 
				soundAvailable = true;
			}
			else{
				Debug.LogWarning("There is no audio clip attached to the AudioSource on Findable: " + this.name);
			}
		}
		else{
			Debug.LogWarning("There is no AudioSource component attached on the Finable : " + this.name);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if ( moveToAnimPosition){
			transform.position = Vector3.Lerp (startPoint, endPoint, (Time.time - startTime) / duration);
			if ( transform.position == endPoint){
				Debug.Log ("DONE MOVING");
				moveToAnimPosition = false;
				inAnimPosition = true;
				
				if ( soundAvailable ){
					audio.Play();
				}
			}
		}
		if ( readyToAnimate()){
			animation.Play("FindableClicked");
			inAnimPosition = false;
		}
	}
	
	void OnMouseUpAsButton ()
	{
		if ( findable ){
			GameObject ItemList = GameObject.Find("ItemList");
		if(ItemList.GetComponent<CreateObjectiveList>().DoesItemExist(ObjectName)){
			UnityEngine.Object.Destroy(this.GetComponent<Rigidbody>());
			//start moving the object to the startpoint of the animation
			startPoint = transform.position;
			startTime = Time.time;
			//start moving
			moveToAnimPosition = true;
			}
		}
	}
	
	void MoveAnimationDone ()
	{
		GameObject ItemList = GameObject.Find("ItemList");
		ItemList.GetComponent<CreateObjectiveList>().RemoveItem(ObjectName);
		Destroy (gameObject);
	}
	
	private bool readyToAnimate(){
		if ( inAnimPosition ){
			
			if ( soundAvailable ){
				if ( !audio.isPlaying ){
					return true;
				}
			}
			else{
				return true;
			}
		}
		return false; // false if above uncommented
	}
}
