using System.Collections;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    public static bool Exists = false;
    public static bool Visible = false;

    public GameObject ArrowX, ArrowY, ArrowZ, ArrowPrefab;

    void Start()
    {
        Init();
    }

    void Init()
    {
        Vector3 pos = new Vector3(2.5f, 0, 0);
        Object arrX = Gizmo.Instantiate(ArrowPrefab, pos, Quaternion.identity);
        ArrowX = (GameObject)arrX;
        ArrowX.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        ArrowX.transform.Rotate(0, 270, 0);
        ArrowX.renderer.material.color = Color.red;
        ArrowX.AddComponent<MeshCollider>();
        ArrowX.AddComponent<HighLightArrow>();
        ArrowX.AddComponent<MoveObject>();

        pos = new Vector3(0, 2.5f, 0);
        Object arrY = Instantiate(ArrowPrefab, pos, Quaternion.identity);
        ArrowY = (GameObject)arrY;
        ArrowY.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        ArrowY.transform.Rotate(90, 0, 0);
        ArrowY.renderer.material.color = Color.green;
        ArrowY.AddComponent<MeshCollider>();
        ArrowY.AddComponent<HighLightArrow>();

        pos = new Vector3(0, 0, -2.5f);
        Object arrZ = Instantiate(ArrowPrefab, pos, Quaternion.identity);
        ArrowZ = (GameObject)arrZ;
        ArrowZ.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        ArrowZ.transform.Rotate(0, 0, 0);
        ArrowZ.renderer.material.color = Color.blue;
        ArrowZ.AddComponent<MeshCollider>();
        ArrowZ.AddComponent<HighLightArrow>();

        Exists = true;

        Hide();
    }

    void FixedUpdate()
    {
        if (Variables.Selected != null)
        {
            if (!Gizmo.Exists)
            {
                // Initialize Gizmo Arrows
                Init();
            }

            // Move Gizmo??
            MoveToSelectedObject();

            if (!Visible)
            {
                Show();
            }
        }
        else
        {
            // Hide Gizmo
            Hide();
        }
    }

    void MoveToSelectedObject()
    {
        Vector3 pos = Variables.Selected.transform.position;
        ArrowX.transform.position = new Vector3(pos.x + 2.5f, pos.y, pos.z);
        ArrowY.transform.position = new Vector3(pos.x, pos.y + 2.5f, pos.z);
        ArrowZ.transform.position = new Vector3(pos.x, pos.y, pos.z - 2.5f);
    }

    void Hide()
    {
        ArrowX.transform.renderer.enabled = false;
        ArrowY.transform.renderer.enabled = false;
        ArrowZ.transform.renderer.enabled = false;

        Visible = false;
    }

    void Show()
    {
        ArrowX.transform.renderer.enabled = true;
        ArrowY.transform.renderer.enabled = true;
        ArrowZ.transform.renderer.enabled = true;

        Visible = true;
    }

    void Destroy()
    {
        ArrowX = null;
        ArrowY = null;
        ArrowZ = null;

        Exists = false;
        Visible = false;
    }
}