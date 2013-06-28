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
		
	}
	/// <summary>
	/// Initialises the item.
	/// </summary>
	/// <param name='_name'>
	/// _The Item name.
	/// </param>
	/// <param name='_hash'>
	/// _The Item hash.
	/// </param>
	/// <param name='_Id'>
	/// _ AssetBundle identifier.
	/// </param>
	public void InitialiseItem(string _name,string _hash, Guid _Id){
			Hash = _hash;
			Name = _name;
			UILabel lbl = this.GetComponentInChildren<UILabel>();
			lbl.text = Name;
			Id = _Id;
		this.GetComponentInChildren<AddItem>().hash = _hash;
		
	}
	
}
