using UnityEngine;
using System.Collections;

public class StopThisThing : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick(){
		/*Debug.Log ("Please STAHP");
		Variables.Selected.GetComponent<SelectMovable>().StopPlease();*/
		 var x = Variables.Selected;
		x.GetComponent<SelectMovable>().DesiredPosition = new Vector3(-5, x.transform.position.y, x.transform.position.z);
		x.GetComponent<SelectMovable>().desiredPositionChanged = true;
		
	}
}
