using UnityEngine;
using System.Collections;

public class SpawnCubes : MonoBehaviour
{
	public int zoffset = 0;
	int rows = 0;

    Color[] colors;
    System.Random r;
	// Use this for initialization
	void Start ()
	{
        colors = new Color[8];
        colors[0] = Color.blue;
        colors[1] = Color.cyan;
        colors[2] = Color.green;
        colors[3] = Color.magenta;
        colors[4] = Color.red;
        colors[5] = Color.yellow;
        colors[6] = Color.white;
        colors[7] = Color.black;
        r = new System.Random();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void OnMouseOver ()
	{
		guiText.material.color = new Color (0, 100, 255);
	}

	void OnMouseExit ()
	{
		guiText.material.color = Color.white;
	}

	void OnMouseUpAsButton ()
	{
		if (rows < 3) {
			for (int offset = -4; offset <= 4; offset += 2) {
				// Spawn a cube
				GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				cube.AddComponent<Rigidbody> ();
                cube.renderer.material.color = getRandomColor();
				cube.transform.localScale = new Vector3 (1, 1, 1);
				cube.transform.position = new Vector3 (offset, 2, zoffset);

				// Add the itembehaviour script
				cube.AddComponent<ItemSelect> ();
				cube.AddComponent<Animation> ();
				
				
				AnimationClip clips = Resources.Load("myanim", typeof (AnimationClip)) as AnimationClip;
				if ( ! clips){
					Debug.LogError("The Clip 'myanim' could not be loaded as AnimationClip");
				}
				else {
					cube.animation.AddClip(clips, clips.name);
					Debug.Log("Added clip to cube");
				}
				
				ItemSelect currentItem = cube.GetComponent<ItemSelect>();
				currentItem.findable = true;
				
			}

			zoffset += 2;
			rows++;
		} else {
			rows = 0;
			zoffset = 0;
		}
	}

    public Color getRandomColor()
    {
        return colors[r.Next(0, colors.GetLength(0))];
    }
}
