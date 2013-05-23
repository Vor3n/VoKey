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
	private Vector3 realMousePosition;
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
		//screenPoint = Camera.main.WorldToScreenPoint(Variables.Selected.transform.position);
     	mousePosition = Input.mousePosition;
		screenPoint = Camera.main.ScreenToWorldPoint (mousePosition);
		offset = Variables.Selected.transform.position - Camera.main.ScreenToWorldPoint (mousePosition);
	}
     
	void OnMouseUp ()
	{
		//screenPoint = Camera.main.WorldToScreenPoint(Variables.Selected.transform.position);
		Variables.Selected.rigidbody.Sleep ();
  
	}

	void OnMouseDrag ()
	{
		Vector3 curScreenPoint = mousePosition;
     	
		DesiredPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
		Difference = DesiredPosition - ObjectPosition;
		SelectMovable other = (SelectMovable) Variables.Selected.GetComponent(typeof(SelectMovable));
		if (MoveDirection.Y == Axis) {
			//Variables.Selected.transform.position = new Vector3(this.transform.position.x,  curPosition.y  ,this.transform.position.z);
			//Variables.Selected.rigidbody.AddRelativeForce (new Vector3 (0, -Difference.y, 0)); //
			other.DesiredPosition = (new Vector3(ObjectPosition.x, DesiredPosition.y, ObjectPosition.z)); 
		} else if (MoveDirection.Z == Axis) {
			//Variables.Selected.transform.position = new Vector3(this.transform.position.x,   this.transform.position.y, curPosition.y);	
			//Variables.Selected.rigidbody.AddRelativeForce (new Vector3 (0, 0, -Difference.z));
			other.DesiredPosition = (new Vector3(ObjectPosition.x, ObjectPosition.y, DesiredPosition.z));
		} else {
			//Variables.Selected.transform.position = new Vector3(curPosition.x,  this.transform.position.y  ,this.transform.position.z);	
			//Variables.Selected.rigidbody.AddRelativeForce (new Vector3 (-Difference.x, 0, 0));
			other.DesiredPosition = (new Vector3(DesiredPosition.x, ObjectPosition.y, ObjectPosition.z));
		}	
		
		
	}
		
	/*
		//Debug.Log("DRAG");
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
     
    	Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		MoveDir =  screenPoint -Camera.main.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		
		
		float speed = MoveSpeed;
		if(MoveDirection.Y == Axis){
			//Variables.Selected.transform.position = new Vector3(this.transform.position.x,  curPosition.y  ,this.transform.position.z);
			if(MoveDir.y >0){
					speed = -1*speed;
			}
			  Variables.Selected.rigidbody.AddRelativeForce(new Vector3(0,speed,0));
		}else if (MoveDirection.Z == Axis){
			if(MoveDir.z <0){
					speed = -1*speed;
			}
			//Variables.Selected.transform.position = new Vector3(this.transform.position.x,   this.transform.position.y, curPosition.y);	
			
			Variables.Selected.rigidbody.AddRelativeForce(new Vector3(0,0,speed));
		}else{
			if(MoveDir.x <0){
					speed = -1*speed;
			}
			//Variables.Selected.transform.position = new Vector3(curPosition.x,  this.transform.position.y  ,this.transform.position.z);	
			 
			Variables.Selected.rigidbody.AddRelativeForce(new Vector3(speed,0,0));
		}	
		
		screenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	
	
	// Update is called once per frame
	/*void OnMouseDown(){
		//Debug.Log("Click");
		if(MoveDirection.Y == Axis){
					//Move Y Axis
				 	//Variables.Selected.transform.position = new Vector3(this.transform.position.x,  Input.mousePosition.y * MoveSpeed  ,this.transform.position.z);			
					oldPos = Input.mousePosition.y;
				}else if (MoveDirection.Z == Axis){
				// Move Z Axis
				 	//Variables.Selected.transform.position = new Vector3(this.transform.position.x, this.transform.position.y,Input.mousePosition.y* MoveSpeed );
					oldPos = Input.mousePosition.y;
				
				}else{
				//Move X Axis
				 //Variables.Selected.transform.position = new Vector3(this.transform.position.x +((Input.mousePosition.x-oldPos) * MoveSpeed),this.transform.position.y, this.transform.position.z);
					oldPos = Input.mousePosition.x;
				}
	}
	
	
	void  OnMouseDrag () {
		if (Variables.Selected != null)			
        {
					if(MoveDirection.Y == Axis){
					//Move Y Axis
				 	Variables.Selected.transform.position = new Vector3(this.transform.position.x,  Input.mousePosition.y * MoveSpeed  ,this.transform.position.z);			
					oldPos = Input.mousePosition.y;
				}else if (MoveDirection.Z == Axis){
				// Move Z Axis
				 	Variables.Selected.transform.position = new Vector3(this.transform.position.x, this.transform.position.y,Input.mousePosition.y* MoveSpeed );
					oldPos = Input.mousePosition.y;
				
				}else{
				//Move X Axis
				 Variables.Selected.transform.position = new Vector3(this.transform.position.x +((Input.mousePosition.x-oldPos) * MoveSpeed),this.transform.position.y, this.transform.position.z);
					oldPos = Input.mousePosition.x;
				}
			
		}
		
	}*/

  
    
}
