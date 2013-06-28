using UnityEngine;
using System.Collections;
using VokeySharedEntities;

public class EditTown : MonoBehaviour {
	string GUID = "";
	LoadWebPage lwp;
	
	void OnClick()
	{
		GUID = "";
		string classname = GameObject.Find("TownList").GetComponent<TeacherMenuTowns>().selectedItem.GetComponent<InitializeTownItem>().ClassLabel.text;
		foreach(Town t in GameObject.Find("GameController").GetComponent<GameControllerScript>().towns)
		{
			if (t.classroomName.Equals(classname))
			{
				GUID = t.id.ToString();
			}
		}
		GameObject.Find("AddTownErrorLabel").GetComponent<UILabel>().text = "";
		GameObject classpanel = GameObject.Find("ShowClassesPanel");
		classpanel.GetComponent<UIPanel>().alpha = 0f;
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
		lwp = GameObject.Find("EditTownButton").AddComponent<LoadWebPage>();
		//lwp.enabled = true;
		lwp.DoStart((int)(widthpercentage*1),(int)(heightpercentage*1),(int)(widthpercentage*98),(int)(heightpercentage*98),100f,"studentpage","dynamic/edittown/"+GUID);
		Debug.Log(GUID);
		GameObject TownList = GameObject.Find("TownList");
		TownList.GetComponent<TeacherMenuTowns>().DoClear();
	}
	
	public void DoClear()
	{
		Destroy(lwp);
		GameObject classpanel = GameObject.Find("ShowClassesPanel");
		classpanel.GetComponent<UIPanel>().alpha = 1f;
		GameObject TownList = GameObject.Find("TownList");
		TownList.GetComponent<TeacherMenuTowns>().DoStart();
	}
}
