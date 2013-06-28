using UnityEngine;
using System.Collections;

public class AddItem : MonoBehaviour {
	AssetBundleManager abm;
	public string hash;
	
	void Start(){
		abm  = GameObject.Find("EditorController").GetComponent<AssetBundleManager>();
	}
	
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
