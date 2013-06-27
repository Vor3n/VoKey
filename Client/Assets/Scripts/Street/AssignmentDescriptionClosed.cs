using UnityEngine;
using System.Collections;

public class AssignmentDescriptionClosed : MonoBehaviour {
    UIPanel label;
    UISprite sprite;
    bool animated = false;

	// Use this for initialization
	void Start () {
        sprite = GameObject.Find("AssignmentDescriptionBackground").GetComponent<UISprite>();
        label = GameObject.Find("AssignmentDescriptionLabel").GetComponent<UIPanel>();
	}
	
	// Update is called once per frame
	void Update () {
        if (animated)
        {
            if (!sprite.animation.isPlaying)
            {
                (GameObject.Find("AssignmentDescriptionPanel").GetComponent<UIPanel>()).alpha = 0f;
                animated = false;
            }
        }
	}

    void OnClick()
    {
        // Add colliders from assignment list / houses / scroll bars
        GameObject[] objects = (GameObject[])GameObject.FindSceneObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
        {
            if (go.name == "No" || go.name == "Yes") continue;

            MeshCollider m = go.GetComponent<MeshCollider>();
            if (m != null)
            {
                m.enabled = true;
            }

            BoxCollider b = go.GetComponent<BoxCollider>();
            if (b != null)
            {
                b.enabled = true;
            }
        }

        // Hide buttons & labels
        label.alpha = 0f;

        // Play closing animation
        sprite.animation.Play("AssignmentDescriptionClosed");
        animated = true;
    }
}
