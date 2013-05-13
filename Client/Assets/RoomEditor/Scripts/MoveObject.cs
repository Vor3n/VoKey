using UnityEngine;
using System.Collections;
public enum MoveDirection{ X ,Y, Z} 
public class MoveObject : MonoBehaviour {
	public MoveDirection Axis;
	public float MoveSpeed = 0.05f;
	private float oldPos;
	public float MouseX;
	
	
	
	
	Vector3 screenPoint;
	Vector3 offset;
	
	// Use this for initialization
	void Start () {
	
	}
	void Update(){
		MouseX = Input.mousePosition.x;	
	}
	
	
	    void OnMouseDown()
    {
    screenPoint = Camera.main.WorldToScreenPoint(Variables.Selected.transform.position);
     
    offset = Variables.Selected.transform.position - Camera.main.ScreenToWorldPoint(
    new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }
     
     
    void OnMouseDrag()
    {
    	Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
     
    	Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		
		
		if(MoveDirection.Y == Axis){
			Variables.Selected.transform.position = new Vector3(this.transform.position.x,  curPosition.y  ,this.transform.position.z);
		}else if (MoveDirection.Z == Axis){
			Variables.Selected.transform.position = new Vector3(this.transform.position.x,   this.transform.position.y, curPosition.y);	
		}else{
			Variables.Selected.transform.position = new Vector3(curPosition.x,  this.transform.position.y  ,this.transform.position.z);	
		}	
		
		
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
