using UnityEngine;
using System.Collections;
using VokeySharedEntities;

public class EditTown : MonoBehaviour {
	string GUID = "";
	LoadWebPage lwp;
	
	void OnClick()
	{
		string classname = GameObject.Find("TownList").GetComponent<TeacherMenuTowns>().selectedItem.GetComponent<InitializeTownItem>().ClassLabel.text;
		foreach(Town t in GameObject.Find("GameController").GetComponent<GameControllerScript>().towns)
		{
			if (t.classroomName.Equals(classname))
			{
				GUID = t.id.ToString();
			}
		}
		GameObject.Find("AddTownErrorLabel").GetComponent<UILabel>().text = "";
		DoStart();
	}
	
	
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
		lwp.DoStart((int)(widthpercentage*1),(int)(heightpercentage*1),(int)(widthpercentage*98),(int)(heightpercentage*98),100f,"studentpage","dynamic/edittown/"+GUID);
	}
	
	public void DoClear()
	{
		lwp.enabled = false;
	}
}
