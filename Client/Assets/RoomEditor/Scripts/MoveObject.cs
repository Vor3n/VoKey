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
	private GameObject GizmoPlane;
	private bool gizmoActive = false;
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
		set {
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
		set {
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
		set {
		}
	}
	// Use this for initialization
	void Start ()
	{
		GizmoPlane = GameObject.Find ("GizmoPlane");
	}

	void Update ()
	{	
		if (gizmoActive) {
			RaycastHit[] hits;
			
			Ray ray = Camera.mainCamera.ScreenPointToRay (Input.mousePosition);
			
			Debug.DrawRay (ray.origin, ray.direction * 100, Color.blue);
			
			hits = Physics.RaycastAll (ray);
			
			foreach (RaycastHit hit in hits) {
				//Debug.Log("HIT: " + hit.transform.gameObject.name);
				if (hit.transform.gameObject.name.Equals ("GizmoPlane")) {
					this.DesiredPosition = new Vector3 (hit.point.x, hit.point.y, hit.point.z)+offset;
					Debug.DrawRay (ray.origin, ray.direction * hit.distance, Color.red);
					SelectMovable other = (SelectMovable)Variables.Selected.GetComponent (typeof(SelectMovable));
					if (MoveDirection.Y == Axis) {
			
						other.DesiredPosition = (new Vector3 (ObjectPosition.x, DesiredPosition.y, ObjectPosition.z)); 
					} else if (MoveDirection.Z == Axis) {
			
						other.DesiredPosition = (new Vector3 (ObjectPosition.x, ObjectPosition.y, DesiredPosition.z));
					} else {
		
						other.DesiredPosition = (new Vector3 (DesiredPosition.x, ObjectPosition.y, ObjectPosition.z));
					}
					
				}
			}
		}
	}
	
	void OnCollisionEnter (Collision c)
	{
		Debug.Log ("BoemIsHo");
			
	}
	
	void OnMouseDown ()
	{
		GizmoPlane.transform.localScale = new Vector3(10f, 10f, 10f);
		//Variables.Selected.GetComponent<Rigidbody>().isKinematic = false;
		gizmoActive = true;
		//Debug.Log ("MOUSEDOWN");
		//screenPoint = Camera.main.WorldToScreenPoint(Variables.Selected.transform.position);
		//mousePosition = Input.mousePosition;
		//screenPoint = Camera.main.ScreenToWorldPoint (mousePosition);
		//offset = Variables.Selected.transform.position - Camera.main.ScreenToWorldPoint (mousePosition);
		
		if (Axis == MoveDirection.X || Axis == MoveDirection.Y) {
			GizmoPlane.transform.rotation = Quaternion.Euler (270f, 0f, 0f);
			
			Vector3 temp = GizmoPlane.transform.position;
			GizmoPlane.transform.position = new Vector3 (0f, temp.y, this.gameObject.transform.position.z);
		} else {
			GizmoPlane.transform.rotation = Quaternion.Euler (0f, 0f, 0f);
			Vector3 temp = GizmoPlane.transform.position;
			GizmoPlane.transform.position = new Vector3 (0f, this.gameObject.transform.position.y, temp.z);
		}
		
		
		RaycastHit[] hits;
			
		Ray ray = Camera.mainCamera.ScreenPointToRay (Input.mousePosition);
			
		Debug.DrawRay (ray.origin, ray.direction * 100, Color.blue);
			
		hits = Physics.RaycastAll (ray);
			
		foreach (RaycastHit hit in hits) {
			//Debug.Log("HIT: " + hit.transform.gameObject.name);
			if (hit.transform.gameObject.name.Equals ("GizmoPlane")) {
				Debug.Log("HIT: " + hit.transform.gameObject.name);
				offset = Variables.Selected.transform.position - new Vector3 (hit.point.x, hit.point.y, hit.point.z);
				Debug.DrawRay (ray.origin, ray.direction * hit.distance, Color.red);
			}
		}
	}
     
	void OnMouseUp ()
	{
		GizmoPlane.transform.localScale = new Vector3(0f, 0f, 10f);
		gizmoActive = false;
		/*SelectMovable other = (SelectMovable)Variables.Selected.GetComponent (typeof(SelectMovable));
		//screenPoint = Camera.main.WorldToScreenPoint(Variables.Selected.transform.position);
		Variables.Selected.rigidbody.Sleep ();
		other.DesiredPosition = Variables.Selected.transform.position;
		other.desiredPositionChanged = false;*/
	}

	void OnMouseDrag ()
	{
		/*Vector3 curScreenPoint = mousePosition;
     	
		
		DesiredPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
		//Debug.Log("xdes: " + DesiredPosition.x + "	
		Difference = DesiredPosition - ObjectPosition;
		SelectMovable other = (SelectMovable)Variables.Selected.GetComponent (typeof(SelectMovable));
		if (MoveDirection.Y == Axis) {
			
			other.DesiredPosition = (new Vector3 (ObjectPosition.x, -DesiredPosition.y, ObjectPosition.z)); 
		} else if (MoveDirection.Z == Axis) {
			
			other.DesiredPosition = (new Vector3 (ObjectPosition.x, ObjectPosition.y, -DesiredPosition.z));
		} else {
		
			other.DesiredPosition = (new Vector3 (-DesiredPosition.x, ObjectPosition.y, ObjectPosition.z));
		}	
		*/
		
	}
		

    
}
