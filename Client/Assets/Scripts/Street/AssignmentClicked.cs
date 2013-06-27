using UnityEngine;
using System.Collections;
using VokeySharedEntities;
using System.Threading;

public class AssignmentClicked : MonoBehaviour
{
    public Assignment assignment;
    UIPanel label;
    UISprite sprite;
    bool animated = false;

    // Use this for initialization
    void Start()
    {
        label = GameObject.Find("AssignmentDescriptionLabel").GetComponent<UIPanel>();
        sprite = GameObject.Find("AssignmentDescriptionBackground").GetComponent<UISprite>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animated)
        {
            if (!sprite.animation.isPlaying)
            {
                label.alpha = 1f;
                animated = false;
            }
        }
    }

    void OnClick()
    {
        //Debug.Log("Assignment clicked: " + assignment.name);
        Street.CurrentAssignment = assignment;

        // Remove colliders from assignment list / houses / scroll bars
        GameObject[] objects = (GameObject[])GameObject.FindSceneObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
        {
            if (go.name == "No" || go.name == "Yes") continue;

            MeshCollider m = go.GetComponent<MeshCollider>();
            if (m != null)
            {
                m.enabled = false;
            }

            BoxCollider b = go.GetComponent<BoxCollider>();
            if (b != null)
            {
                b.enabled = false;
            }
        }

        // Initialize labels for assignment box
        (GameObject.Find("AssignmentDescription").GetComponent<UILabel>()).text = assignment.summary;
        (GameObject.Find("AssignmentName").GetComponent<UILabel>()).text = assignment.name + "   Start Assignment?";
        (GameObject.Find("AssignmentDescriptionPanel").GetComponent<UIPanel>()).alpha = 1f;

        sprite.animation.Play("AssignmentDescriptionOpened");

        animated = true;
    }
}
