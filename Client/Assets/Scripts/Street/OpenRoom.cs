using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenRoom : MonoBehaviour
{
    Color[] InitialColors;
    public string Room;
    bool showLabel = false;
    List<string> menuItems = new List<string>();
    public GameObject ButtonPrefab;

    // Use this for initialization
    void Start()
    {
        InitialColors = new Color[gameObject.renderer.materials.Length];
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (showLabel)
        {
            GUI.Label(new Rect(Screen.width/2-50, Screen.height/4, 100, 20), Room + "'s House");
        }
    }

    void OnMouseEnter()
    {
        showLabel = true;

        for (int i = 0; i < gameObject.renderer.materials.Length; i++)
        {
            InitialColors[i] = gameObject.renderer.materials[i].color;
            gameObject.renderer.materials[i].color += Color.white;
        }
    }

    void OnMouseExit()
    {
        showLabel = false;

        for (int i = 0; i < gameObject.renderer.materials.Length; i++)
        {
            gameObject.renderer.materials[i].color = InitialColors[i];
        }
    }

    void OnMouseUpAsButton()
    {
        // Get List of Rooms for Student
        // TODO

        // Create Room List
        CreateRoomList(null);

        // Show menu with rooms
        Street.RoomMenuPanel.alpha = 1f;
    }

    void CreateRoomList(List<Room> roomList)
    {
        // Read Rooms belonging to student
        menuItems.Add("test entry");
        menuItems.Add("test entry");
        menuItems.Add("test entry");
        menuItems.Add("test entry");

        // Resize the panel
        // TODO: Paging if there are too many rooms
        GameObject panel = (GameObject)GameObject.Find("RoomMenuSprite");
        //panel.transform.localScale = new Vector3(200, 45 * menuItems.Count, 1);

        // Add Buttons
        GameObject ButtonPrefab = (GameObject)Resources.Load("Prefabs/RoomMenuPrefab", typeof(GameObject));
        /*
        for(int i = 0; i < 1; i++) {
            UIButton go = (UIButton)Instantiate(ButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            //go.transform.position = new Vector3(panel.transform.position.x + 10, panel.transform.position.y + (25 * i), 0);
            Debug.Log("Position: " + go.transform.position);
            go.transform.localScale = new Vector3(0.002f, 0.002f, 0.002f);
        } */

        UIPopupList list = panel.AddComponent<UIPopupList>();
        list.items.Add("test");
    }
}
