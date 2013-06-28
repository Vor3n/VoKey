using UnityEngine;
using System.Collections;

public class UndoButton : MonoBehaviour {

	/// <summary>
	/// Responds to the click event.
	/// Loads the previous state of the room
	/// </summary>
	void OnClick(){
		Debug.Log("Undo Changes");
		RoomManager  RM =	(RoomManager) FindObjectOfType(typeof(RoomManager));
		RM.LoadRoom(RM.RoomID);
	}
}
