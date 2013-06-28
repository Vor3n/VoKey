using UnityEngine;
using System.Collections;
using System;

public class InitializeItem : MonoBehaviour {
	
	public string Hash;
	public string Name;
	public Guid Id;
	// Use this for initialization
	void Start () {
		InitialiseItem(Name, Hash, Id);
		//Debug.Log(Id.ToString("D"));
	}
	
	public void InitialiseItem(string _name,string _hash, Guid _Id){
			Hash = _hash;
			Name = _name;
			UILabel lbl = this.GetComponentInChildren<UILabel>();
			lbl.text = Name;
			Id = _Id;
		this.GetComponentInChildren<AddItem>().hash = _hash;
		
	}
	
}
