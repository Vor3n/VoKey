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
	public Vector3 DesiredPosition {
		get {
			return prevDesiredPosition;
		}
		set {
			
			if (axisDifferenceExceedsThreshold (value.x, this.transform.position.x, distanceThreshold) 
				|| axisDifferenceExceedsThreshold (value.y, this.transform.position.y, distanceThreshold) 
				|| axisDifferenceExceedsThreshold (value.z, this.transform.position.z, distanceThreshold)) {
				DesiredPositionForUnity = prevDesiredPosition = value;
				selectableNeedsToMove = true;
			} else {
				selectableNeedsToMove = false;
			}
		}
	}
	
	public Vector3 DesiredPositionForUnity;
	
	/// <summary>
	/// Indicates whether the desired position has changed.
	/// </summary>
	public bool selectableNeedsToMove = false;
	
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
			return DesiredPosition - this.transform.position;
		}
		set {}
	}
	
	public Vector3 DistanceFromDesiredPositionDebug;
	
	// Use this for initialization
	void Start ()
	{
 prevDesiredPosition = this.transform.position;
	}
	
	public void StopPlease ()
	{
		
		rigidbody.Sleep ();
		rigidbody.velocity = rigidbody.velocity * -1;
	}
	
	void Update ()
	{
		//var x = DistanceFromDesiredPosition;
		if (selectableNeedsToMove) {
			if (axisAreWithinThreshold ()) {
				destinationReached ();
			} else {
				//Debug.Log ("Changing velocity");
				rigidbody.isKinematic = false;
				rigidbody.velocity = new Vector3 (DistanceFromDesiredPosition.x*5, DistanceFromDesiredPosition.y*5, DistanceFromDesiredPosition.z*5);
			}
			
		}
	}
	
	void destinationReached ()
	{
		Debug.Log ("Destination reached");
		selectableNeedsToMove = false;
		DesiredPosition = this.transform.position;
		if (!rigidbody.isKinematic)
			rigidbody.velocity = new Vector3 (0, 0, 0);
		rigidbody.isKinematic = true;
	}
	
	bool axisAreWithinThreshold ()
	{
		
		return ((Mathf.Abs (DistanceFromDesiredPosition.x) < distanceThreshold)
			&& (Mathf.Abs (DistanceFromDesiredPosition.y) < distanceThreshold)
			&& (Mathf.Abs (DistanceFromDesiredPosition.z) < distanceThreshold));
		
		/*return (axisDifferenceSmallerThan(this.transform.position.x, DesiredPosition.x, distanceThreshold) 
				&& axisDifferenceSmallerThan(this.transform.position.y, DesiredPosition.y, distanceThreshold) 
				&& axisDifferenceSmallerThan(this.transform.position.z, DesiredPosition.z, distanceThreshold));*/
	}
	
	bool axisDifferenceSmallerThan (float oldValue, float currentValue, float threshold)
	{
		return (Mathf.Abs (oldValue - currentValue) < threshold);
	}
	
	bool axisDifferenceExceedsThreshold (float oldValue, float currentValue, float threshold)
	{
		return (Mathf.Abs (oldValue - currentValue) > threshold);
	}

	void OnMouseUpAsButton ()
	{
		if (Variables.Selected != null)
			Variables.Selected.GetComponent<SelectMovable> ().destinationReached ();
		Variables.Selected = gameObject;
		Debug.Log ("M-Selected: " + gameObject.name);
	}

	void OnTouch ()
	{
		if (Variables.Selected != null)
			Variables.Selected.GetComponent<SelectMovable> ().destinationReached ();
		Variables.Selected = gameObject;
		Debug.Log ("T-Selected: " + gameObject.name);
	}
}
