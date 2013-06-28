using UnityEngine;
using System.Collections;
using VokeySharedEntities;

public class OpenRoom : MonoBehaviour
{
    public enum OpenType
    {
        RoomList,
        Assignment
    }

    public OpenType OpenAs;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        System.Guid roomGUID = System.Guid.Empty;
        // Set guid in global variable
        switch (OpenAs)
        {
            case OpenType.RoomList:
                GameObject listObject = (GameObject)GameObject.Find("RoomList");
                UIPopupList list = listObject.GetComponent<UIPopupList>();
                if (Street.CurrentRooms != null)
                {
                    if (list != null)
                    {
                        Room room = Street.CurrentRooms.Find(x => x.name == list.selection);
                        if (room != null)
                        {
                            Debug.Log("[RoomList]Going to open room:[" + room.id + "]");
                            roomGUID = room.id;
                        }
                        else
                        {
                            Debug.Log("[RoomList]Cannot find room:[" + list.selection + "]");
                        }
                    }
                    else
                    {
                        Debug.Log("Cannot find RoomList");
                    }
                }
                else
                {
                    Debug.Log("Cannot find Street.CurrentRooms");
                }
                break;
            case OpenType.Assignment:
                Debug.Log("[Assignment]Going to open room:[" + Street.CurrentAssignment.roomToPlayIn + "]");
                roomGUID = Street.CurrentAssignment.roomToPlayIn;
                break;
        }

        if (roomGUID != System.Guid.Empty)
        {
            GameControllerScript gcs = GameObject.Find("GameController").GetComponent<GameControllerScript>();
            gcs.RoomToOpen = roomGUID;

            // Open the scene
            Application.LoadLevel("RoomTest");
        }
    }
}
