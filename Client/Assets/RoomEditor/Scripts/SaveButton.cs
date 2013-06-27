using UnityEngine;
using System.Collections;
using System;

public class SaveButton : MonoBehaviour {

	void OnClick(){
		Debug.Log("Save Changes");
		RoomManager  RM =	(RoomManager) FindObjectOfType(typeof(RoomManager));
		RM.SaveRoom();
		
	
	}
}

