using UnityEngine;
using System.Collections;

public class UndoButton : MonoBehaviour {

	// Use this for initialization
	void OnClick(){
		Debug.Log("Undo Changes");
	RoomManager  RM =	(RoomManager) FindObjectOfType(typeof(RoomManager));
		RM.LoadRoom(RM.RoomID);
	}
}
