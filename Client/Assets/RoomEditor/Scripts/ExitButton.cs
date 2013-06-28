using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour {

	// Go back to the teacher menu
	void OnClick () {
		Application.LoadLevel("TeacherMenu");
	}
	
