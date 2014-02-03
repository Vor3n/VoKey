using UnityEngine;
using System.Collections;

public class SelectMovable : MonoBehaviour
{
	/// <summary>
	/// The previous desired position.
	/// </summary>
	private Vector3 prevDesiredPosition;
	
	/// <summary>
	/// Gets or sets the desired position.
	/// </summary>
	/// <value>
	/// The desired position.
	/// </value>
	public Vector3 DesiredPosition{
		get {
			return prevDesiredPosition;
		}
		set {
			if(axisDifferenceExceedsThreshold(value.x, DesiredPosition.x, distanceThreshold) 
				|| axisDifferenceExceedsThreshold(value.y, DesiredPosition.y, distanceThreshold) 
				|| axisDifferenceExceedsThreshold(value.z, DesiredPosition.z, distanceThreshold)){
				DesiredPositionForUnity = prevDesiredPosition = value;
				desiredPositionChanged = true;
			} else {
				desiredPositionChanged = false;
			}
		}
	}
	
	public Vector3 DesiredPositionForUnity;
	
	/// <summary>
	/// Indicates whether the desired position has changed.
	/// </summary>
	public bool desiredPositionChanged = false;
	
	/// <summary>
	/// The distance threshold.
	/// </summary>
	public float distanceThreshold = 1f;
	
	/// <summary>
	/// Gets or sets the distance from desired position.
	/// </summary>
	/// <value>
	/// The distance from desired position.
	/// </value>
	public Vector3 DistanceFromDesiredPosition {
		get {
			#if UNITY_EDITOR
		      DistanceFromDesiredPositionDebug = DesiredPosition- this.transform.position;
		    #endif
			return DesiredPosition- this.transform.position;
		}
		set {}
	}
	
	public Vector3 DistanceFromDesiredPositionDebug;
	
    // Use this for initialization
    void Start()
    {
		prevDesiredPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
	
	public void StopPlease(){
		
		rigidbody.Sleep();
		rigidbody.velocity = rigidbody.velocity *-1;
	}
	
	void FixedUpdate(){
		//var x = DistanceFromDesiredPosition;
		if(desiredPositionChanged){
			if(axisAreWithinThreshold()){
				Debug.Log ("Values are within threshold AND the desired position WAS changed. Now stopping this madness.");
				desiredPositionChanged = false;
				DesiredPosition = this.transform.position;
				rigidbody.velocity = new Vector3(0, 0, 0);
				rigidbody.Sleep();
				
			}else{
			Debug.Log ("Moving stuff");
			//Vector3 toMove = DistanceFromDesiredPosition;
			
			rigidbody.velocity = new Vector3 (DistanceFromDesiredPosition.x, DistanceFromDesiredPosition.y, DistanceFromDesiredPosition.z);
			}
			
		}
	}
	
	bool axisAreWithinThreshold(){
		
		return ((Mathf.Abs(DistanceFromDesiredPosition.x) < distanceThreshold)
			&& ( Mathf.Abs(DistanceFromDesiredPosition.y) < distanceThreshold)
			&& ( Mathf.Abs(DistanceFromDesiredPosition.z) < distanceThreshold));
		
		/*return (axisDifferenceSmallerThan(this.transform.position.x, DesiredPosition.x, distanceThreshold) 
				&& axisDifferenceSmallerThan(this.transform.position.y, DesiredPosition.y, distanceThreshold) 
				&& axisDifferenceSmallerThan(this.transform.position.z, DesiredPosition.z, distanceThreshold));*/
	}
	
	bool axisDifferenceSmallerThan(float oldValue, float currentValue, float threshold){
		return (Mathf.Abs(oldValue - currentValue) < threshold);
	}
	
	bool axisDifferenceExceedsThreshold(float oldValue, float currentValue, float threshold){
		return (Mathf.Abs(oldValue - currentValue) > threshold);
	}

    void OnMouseUpAsButton()
    {
        Variables.Selected = gameObject;
        //Gizmo.MoveToSelected(Variables.Selected);
        Debug.Log("M-Selected: " + gameObject.name);
    }

    void OnTouch()
    {
        Variables.Selected = gameObject;
        Debug.Log("T-Selected: " + gameObject.name);
    }
}
