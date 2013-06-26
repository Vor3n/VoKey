using UnityEngine;
using System.Collections;
using VokeySharedEntities;
using System.Text;

public class DeleteTown : MonoBehaviour {
	
	void OnClick()
	{
		 
		string classname = GameObject.Find("TownList").GetComponent<TeacherMenuTowns>().selectedItem.GetComponent<InitializeTownItem>().ClassLabel.text;
		string GUID = "";
		foreach(Town t in GameObject.Find("GameController").GetComponent<GameControllerScript>().towns)
		{
			if (t.classroomName.Equals(classname))
			{
				GUID = t.id.ToString();
			}
		}
		WebServiceRequest req = new WebServiceRequest("town/delete/" + GUID);
		StartCoroutine(req.Request());	
		GameObject TownList = GameObject.Find("TownList");
		TownList.GetComponent<TeacherMenuTowns>().DoClear();
		TownList.GetComponent<TeacherMenuTowns>().DoStart();
	}
}
