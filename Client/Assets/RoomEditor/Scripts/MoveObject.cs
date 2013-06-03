using UnityEngine;
using System.Collections;

public enum MoveDirection
{
	X ,
	Y,
	Z
}

public class MoveObject : MonoBehaviour
{
	public MoveDirection Axis;
	public float MoveSpeed = 10f;
	private float oldPos;
	Vector3 screenPoint;
	public Vector3 MoveDir;
	public Vector3 offset;
	/// <summary>
	/// Gets the position of the object that the Gizmo is attached to
	/// </summary>
	/// <value>
	/// The object position.
	/// </value>
	public Vector3 ObjectPosition {
		get {
			return Variables.Selected.transform.position;
		}
		set{
		}
	}
	
	/// <summary>
	/// The difference
	/// </summary>
	public Vector3 Difference;
	public Vector3 DesiredPosition;
	
	/// <summary>
	/// The real mouse position. This variable is assigned to by Input.mousePosition.
	/// </summary>
	 Vector3 realMousePosition;
	/// <summary>
	/// Gets or sets the mouse position with a fixed z-value.
	/// </summary>
	/// <value>
	/// The mouse position.
	/// </value>
	public Vector3 mousePosition {
		get {
			realMousePosition = Input.mousePosition;
			return new Vector3 (realMousePosition.x, realMousePosition.y, screenPoint.z);
		}
		set{
		}
	}
	
	/// <summary>
	/// Gets or sets the W pmouse position.
	/// </summary>
	/// <value>
	/// Mouse WorldPoint
	/// </value>
	public Vector3 RealmousePosition {
		get {
			
			return Camera.main.ScreenToWorldPoint (mousePosition);
		}
		set{
		}
	}
	// Use this for initialization
	void Start ()
	{
	
	}

	void Update ()
	{	
	}
	
	void OnCollisionEnter (Collision c)
	{
		Debug.Log ("BoemIsHo");
			
	}
	
	void OnMouseDown ()
	{
	Debug.Log("MOUSEDOWN");
		//screenPoint = Camera.main.WorldToScreenPoint(Variables.Selected.transform.position);
     	mousePosition = Input.mousePosition;
		screenPoint = Camera.main.ScreenToWorldPoint (mousePosition);
		offset = Variables.Selected.transform.position - Camera.main.ScreenToWorldPoint (mousePosition);
	}
	
	
     
	void OnMouseUp ()
	{
		SelectMovable other = (SelectMovable) Variables.Selected.GetComponent(typeof(SelectMovable));
		//screenPoint = Camera.main.WorldToScreenPoint(Variables.Selected.transform.position);
		Variables.Selected.rigidbody.Sleep ();
  		other.DesiredPosition = Variables.Selected.transform.position;
		other.desiredPositionChanged = false;
	}

	void OnMouseDrag ()
	{
		Vector3 curScreenPoint = mousePosition;
     	
		
		DesiredPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
		//Debug.Log("xdes: " + DesiredPosition.x + "	
		Difference = DesiredPosition - ObjectPosition;
		SelectMovable other = (SelectMovable) Variables.Selected.GetComponent(typeof(SelectMovable));
		if (MoveDirection.Y == Axis) {
			
			other.DesiredPosition = (new Vector3(ObjectPosition.x, -DesiredPosition.y, ObjectPosition.z)); 
		} else if (MoveDirection.Z == Axis) {
			
			other.DesiredPosition = (new Vector3(ObjectPosition.x, ObjectPosition.y, -DesiredPosition.z));
		} else {
		
			other.DesiredPosition = (new Vector3(-DesiredPosition.x, ObjectPosition.y, ObjectPosition.z));
		}	
		
		
	}
		

    
}
