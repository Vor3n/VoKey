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
		Logout,
		Quit,
		CHANGEME,
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
		
		if (item == Item.Logout)
		{
			
		}
		
		if (item == Item.Quit)
		{
			
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
	}
}
