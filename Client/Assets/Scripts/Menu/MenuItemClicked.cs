using UnityEngine;
using System.Collections;

public class MenuItemClicked : MonoBehaviour {
	
	public enum Item
	{
		ShowAssignment,
		EditOwnRoom,
		ShowOtherRooms,
		ShowClasses,
		ShowRooms,
		ShowStudentResults,
		ShowStudents,
		Login,
		Logout,
		Quit,
		CHANGEME,
		LogoutYes,
		LogoutNo,
	}
	
	public Item item = Item.CHANGEME;
	

	void OnClick ()
	{
		if (item == Item.EditOwnRoom)
		{
			
		}
		
		if (item == Item.ShowAssignment)
		{
			
		}
		
		if (item == Item.Login)
		{
			GameObject usernamelabel = GameObject.Find("UsernameLabel");
			UILabel UIusernamelabel = (UILabel)usernamelabel.GetComponent("UILabel");
			string username = UIusernamelabel.text;
			GameObject passwordlabel = GameObject.Find("PasswordLabel");
			UILabel UIpasswordlabel = (UILabel)passwordlabel.GetComponent("UILabel");
			string password = UIpasswordlabel.text;
			//do real check here
			if (password == "student" && username == "student")
			{
				Application.LoadLevel("RoomTest");
			}
			else if (password == "teach" && username == "teach")
			{
				Application.LoadLevel("TeacherMenu");
			}
			else
			{
				Debug.LogError("ELSE EMPTY");
			}
		}
		
		if (item == Item.Logout)
		{
			GameObject surepanel = GameObject.Find("LogoutSurePanel");
			surepanel.GetComponent<UIPanel>().alpha = 1f;
			GameObject teachmenu = GameObject.Find("TeacherMainMenu");
			teachmenu.GetComponent<UIPanel>().alpha = 0f;
		}
		
		if (item == Item.Quit)
		{
			Application.Quit();
		}
		
		if (item == Item.ShowOtherRooms)
		{

		}
		
		if (item == Item.ShowClasses)
		{
			
		}
		
		if (item == Item.ShowOtherRooms)
		{
			
		}
		
		if (item == Item.ShowRooms)
		{
			
		}
		
		if (item == Item.ShowStudentResults)
		{
			
		}
		
		if (item == Item.ShowStudents)
		{
			
		}	
		
		if (item == Item.LogoutNo)
		{
			GameObject surepanel = GameObject.Find("LogoutSurePanel");
			surepanel.GetComponent<UIPanel>().alpha = 0f;
			try
			{
				GameObject teachmenu = GameObject.Find("TeacherMainMenu");
				teachmenu.GetComponent<UIPanel>().alpha = 1f;
			}
			catch{}
			try
			{
				GameObject teachmenu = GameObject.Find("StudentMainMenu");
				teachmenu.GetComponent<UIPanel>().alpha = 1f;
			}
			catch{}
			//surepanel.GetComponent<UIPanel>().enabled = false;
		}	
		
		if (item == Item.LogoutYes)
		{
			Application.LoadLevel("MainMenu");
		}
	}
}
