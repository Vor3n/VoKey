using UnityEngine;
using System.Collections;

public class AddItem : MonoBehaviour {
	AssetBundleManager abm;
	public string hash;
	
	void Start(){
		abm  = GameObject.Find("EditorController").GetComponent<AssetBundleManager>();
	}
	
	void OnClick(){
		GameObject.Instantiate(abm.RetrieveObject(hash));
	}
}
