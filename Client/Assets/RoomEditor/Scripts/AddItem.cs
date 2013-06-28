using UnityEngine;
using System.Collections;

public class AddItem : MonoBehaviour {
	AssetBundleManager abm;
	public string hash;
	
	/// <summary>
	/// Assigns the Assetbundlemanager
	/// </summary>
	void Start(){
		abm  = GameObject.Find("GameController").GetComponent<AssetBundleManager>();
	}
	
	/// <summary>
	/// Add a gameobject to the scene based on it's hash.
	/// After instatiating the gameobject it attatches the needed scripts for the editor 
	/// and sets the needed properties.
	/// </summary>
	void OnClick(){
		GameObject GObj = (GameObject) GameObject.Instantiate(abm.RetrieveObject(hash));
		GObj.AddComponent<Rigidbody>().useGravity = false;
		GObj.AddComponent<SelectMovable>();
		GObj.AddComponent<MeshCollider>();
		GObj.transform.position = new Vector3(0,6.5f,-5f);
		GObj.rigidbody.freezeRotation = true;
		GObj.name = hash;
	}
}
