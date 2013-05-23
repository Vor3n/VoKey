using UnityEngine;
using System.Collections;

public class MenuItemClicked : MonoBehaviour {
	
	public enum Item
	{
		ShowAssignment,
		EditOwnRoom,
		ShowOtherRooms,
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
	}
}
