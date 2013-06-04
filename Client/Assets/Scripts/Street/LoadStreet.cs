using UnityEngine;
using System.Collections;

public class LoadStreet : MonoBehaviour
{
	public GameObject HousePrefab;

	// Use this for initialization
	void Start ()
	{
		CreateSingleStreet (Street.StartingCoordinates);

		// onhover for the houses, each shows a name from array, when array is empty, further houses are 'free'
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void CreateSingleStreet (Vector3 StartingCoordinates)
	{
		Quaternion rot = Quaternion.Euler (new Vector3 (-90, -90, 0));

		for (int i = 0; i < Street.studentNames.Count; i++) {
			GameObject house = (GameObject)Instantiate (HousePrefab, StartingCoordinates + new Vector3 (0, 0, i * Street.Increment), rot);
			house.name = Street.studentNames [i] + "'s House";

			// Assign a student name to the house
			house.AddComponent<MeshCollider> ();
			house.AddComponent<OpenRoom> ();
			house.GetComponent<OpenRoom> ().Room = Street.studentNames [i];
			
			ChangeHousePartColor (house, "WaLL", new Color (0.3f, 0.3f, 0.0f));
			ChangeHousePartColor (house, "Door", new Color (1.0f, 0.5f, 0.0f));
			ChangeHousePartColor (house, "roof", new Color (123, 0.0f, 1.0f));
			ChangeHousePartColor (house, "windows", new Color (0.5f, 0.5f, 255, 1.0f));
		}
	}
	
	/// <summary>
	/// Changes the color of a house part.
	/// </summary>
	/// <param name='house'>
	/// The house to change the color of.
	/// </param>
	/// <param name='newColor'>
	/// The new color of the house part.
	/// The RGB values have to be between 1.0f and 255.0f and/or normalized between 0,0f and 1,0f
	/// </param>
	/// <param name='housePart'>
	/// The part of the house to recolor. Part names are : 'Wall', 'Door', 'Roof', 'Windows', 'Venster'
	/// </param>
	void ChangeHousePartColor (GameObject house, string housePart, Color newColor)
	{
		Color normalizedColor = NormalizeColor (newColor);
		if (housePart.ToLower ().Equals ("windows")) { // we want to have the windows always the same alfa value
			normalizedColor.a = 100.0f / 255.0f;
		}
		// iterate through the materials until the right material has been found
		for (int iMat = 0; iMat < house.renderer.materials.Length; iMat ++) {
			if (house.renderer.materials [iMat].name.ToLower ().StartsWith (housePart.ToLower ())) {
				house.renderer.materials [iMat].SetColor ("_Color", normalizedColor);
			}
		}
	}
	
	/// <summary>
	/// Normalizes the color. ( a value between 0.0f and 1.0f )
	/// </summary>
	/// <returns>
	/// The Normalized color.
	/// </returns>
	/// <param name='color'>
	/// A color.
	/// </param>
	private Color NormalizeColor (Color color)
	{
		Color newColor = new Color (color.r, color.g, color.b, color.a);
		while (newColor.r > 1.0f || newColor.g > 1.0f || newColor.b > 1.0f) {
			if (newColor.r > 1.0f) {
				newColor.r = newColor.r / 255.0f;
			}
			if (newColor.g > 1.0f) {
				newColor.g = newColor.g / 255.0f;
			}
			if (newColor.b > 1.0f) {
				newColor.b = newColor.b / 255.0f;
			}
		}
		Debug.Log ("Normalized RGB: " + newColor.r + "," + newColor.g + "," + newColor.b);
		return newColor;
	}
	
}
