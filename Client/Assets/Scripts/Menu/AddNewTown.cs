using UnityEngine;
using System.Collections;
using System.Text;
using VokeySharedEntities;

public class AddNewTown : MonoBehaviour {
	
	void OnClick()
	{
		StartCoroutine(AddTown());	
	}
	
	public IEnumerator AddTown()
	{
		bool isDone = false;
		Debug.Log("STARTING REQUEST");
		float elapsedTime = 0.0f;
		Debug.Log("SENDING THING");
		string url = GlobalSettings.serverURL + "town/create";
		
		GameObject TownLabel = GameObject.Find("TownTextBoxLabel");
		UILabel tl = (UILabel)TownLabel.GetComponent("UILabel");
		string townname = tl.text;
		
		GameObject ClassLabel = GameObject.Find("ClassTextBoxLabel");
		UILabel cl = (UILabel)ClassLabel.GetComponent("UILabel");
		string classname = cl.text;
		
		if (townname.Equals("") || classname.Equals(""))
		{
			Debug.Log("NO TOWN OR CLASS NAME");
			yield break;
		}
		
		Town town = new Town(townname, classname);
		byte[] b = Encoding.UTF8.GetBytes(town.ToXml());
		Hashtable htbl = new Hashtable();
		htbl.Add("Session", GlobalSettings.SessionID);
		htbl.Add("Content-Type", "text/html");
		
		WWW www = new WWW(url, b, htbl);
		while(!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 2.0f) break;
			yield return www;
		}
		isDone = www.isDone;
		string response = www.text;
		Debug.Log("addnewtownresponse: " + response);
		GameObject TownList = GameObject.Find("TownList");
		TownList.GetComponent<TeacherMenuTowns>().DoClear();
		TownList.GetComponent<TeacherMenuTowns>().DoStart();
		
    }
	
	
	
}
