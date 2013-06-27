using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class TeacherMenuStudents : MonoBehaviour {
	LoadWebPage lwp;
	// Use this for initialization
	public void DoStart () 
	{
		float heightpercentage = Screen.height;
		heightpercentage = heightpercentage/100;
		Debug.Log("Height: " + heightpercentage);
		float widthpercentage = Screen.width;
		widthpercentage = widthpercentage / 100;
		Debug.Log("Width: " + widthpercentage);
		Debug.Log("TEACHERMENUSTUDENTS DOSTART");
		lwp = GameObject.Find("StudentList").AddComponent<LoadWebPage>();
		lwp.enabled = true;
		lwp.DoStart((int)(widthpercentage*1),(int)(heightpercentage*1),(int)(widthpercentage*98),(int)(heightpercentage*98),100f,"studentpage","dynamic");
	}
	
	public void DoClear()
	{
		lwp.enabled = false;
	}
}