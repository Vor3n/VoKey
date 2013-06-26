using UnityEngine;
using System.Collections;

public class InitializeTownItem : MonoBehaviour
{
	
	public string TownName;
	public string ClassName;
	public UILabel ClassLabel ;
	public UILabel TownLabel;
	public string GUID;
	
	// Use this for initialization
	void Start ()
	{
		InitialiseItem (ClassName, TownName);
	}
	
	public void InitialiseItem (string _classname, string _townname)
	{
		TownName = _townname;
		ClassName = _classname;
		
		ClassLabel.text = ClassName;
		TownLabel.text = TownName;
	}
	
	void OnClick ()
	{
		transform.parent.transform.parent.GetComponent<TeacherMenuTowns>().selectedItem = this.gameObject;
	}
	
	public void setSelected()
	{
		ClassLabel.color = Color.red;
		TownLabel.color = Color.red;
	}
	
	public void clearSelected()
	{
		ClassLabel.color = Color.white;
		TownLabel.color = Color.white;
	}
	
}
