using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VokeySharedEntities;

public class MenuItemClicked : MonoBehaviour {
	
	public enum Item
	{
		ShowAssignment,
		EditOwnRoom,
		GoToTown,
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
		ShowClassesBack,
		ShowStudentsBack,
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
		
		//LOGIN
		if (item == Item.Login)
		{
			GameObject usernamelabel = GameObject.Find("UsernameLabel");
			UILabel UIusernamelabel = (UILabel)usernamelabel.GetComponent("UILabel");
			string username = UIusernamelabel.text;
			
			GameObject passwordlabel = GameObject.Find("PasswordLabel");
			UILabel UIpasswordlabel = (UILabel)passwordlabel.GetComponent("UILabel");
			string password = UIpasswordlabel.text;
			
			Login login = new Login(username,password);
			StartCoroutine(login.startLogin());
		}
		
		if(item == Item.Logout)
		//LOGOUT CLICKED
		{
			GameObject surepanel = GameObject.Find("LogoutSurePanel");
			surepanel.GetComponent<UIPanel>().alpha = 1f;
			try
			{
				GameObject teachmenu = GameObject.Find("TeacherMainMenu");
				teachmenu.GetComponent<UIPanel>().alpha = 0f;
			}
			catch {}
			try
			{
				GameObject studentmenu = GameObject.Find("StudentMainMenu");
				studentmenu.GetComponent<UIPanel>().alpha = 0f;
			}
			catch {}
		}
		
		if (item == Item.Quit)
		{
			Application.Quit();
		}
		
		if (item == Item.GoToTown)
		{

		}
		
		if (item == Item.ShowClasses)
		{
			try
			{
				GameObject teachmenu = GameObject.Find("TeacherMainMenu");
				teachmenu.GetComponent<UIPanel>().alpha = 0f;
				GameObject classpanel = GameObject.Find("ShowClassesPanel");
				classpanel.GetComponent<UIPanel>().alpha = 1f;
				GameObject TownList = GameObject.Find("TownList");
				TownList.GetComponent<TeacherMenuTowns>().DoStart();
			}
			catch {}
		}
		
		
		
		if (item == Item.ShowClassesBack)
		{
			try
			{
				GameObject classpanel = GameObject.Find("ShowClassesPanel");
				classpanel.GetComponent<UIPanel>().alpha = 0f;
				GameObject teachmenu = GameObject.Find("TeacherMainMenu");
				teachmenu.GetComponent<UIPanel>().alpha = 1f;
				GameObject TownList = GameObject.Find("TownList");
				TownList.GetComponent<TeacherMenuTowns>().DoClear();
			}
			catch {}	
		}
		
		if (item == Item.ShowRooms)
		{
			
		}
		
		if (item == Item.ShowStudentResults)
		{
			
		}
		
		if (item == Item.ShowStudents)
		{
			try
			{
				GameObject teachmenu = GameObject.Find("TeacherMainMenu");
				teachmenu.GetComponent<UIPanel>().alpha = 0f;
				GameObject studentpanel = GameObject.Find("ShowStudentsPanel");
				studentpanel.GetComponent<UIPanel>().alpha = 1f;
				GameObject StudentList = GameObject.Find("StudentList");
				StudentList.GetComponent<TeacherMenuStudents>().DoStart();
			}
			catch {}
		}	
		
		if (item == Item.ShowStudentsBack)
		{
			try
			{
				GameObject teachmenu = GameObject.Find("TeacherMainMenu");
				teachmenu.GetComponent<UIPanel>().alpha = 1f;
				GameObject studentpanel = GameObject.Find("ShowStudentsPanel");
				studentpanel.GetComponent<UIPanel>().alpha = 0f;
				GameObject StudentList = GameObject.Find("StudentList");
				StudentList.GetComponent<TeacherMenuStudents>().DoClear();
			}
			catch {}
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
