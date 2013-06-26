using UnityEngine;
using System.Collections;

public class InitializeItem : MonoBehaviour {
	
	public string Hash;
	public string Name;
	
	// Use this for initialization
	void Start () {
		InitialiseItem(Name, Hash);
	}
	
	public void InitialiseItem(string _name,string _hash){
			Hash = _hash;
			Name = _name;
			UILabel lbl = this.GetComponentInChildren<UILabel>();
			lbl.text = Name;
		
	}
	
}
