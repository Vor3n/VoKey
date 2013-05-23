using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System;
using System.Xml.Serialization;

[System.Serializable]
[XmlRoot("VokeyAssetBundle")]
public class VokeyAssetBundle
{
	[XmlAttribute("Name")]
	/// <summary>
	/// The name.
	/// </summary>
    public string name;
	[XmlAttribute("ModelId")]
	/// <summary>
	/// The model identifier.
	/// </summary>
    public System.Guid modelId;
	[XmlAttribute("Resource Urls")]
	/// <summary>
	/// The resource URL.
	/// </summary>
	[XmlIgnore]
    public System.Uri resourceUrl;
	
	/// <summary>
	/// The list of objects inside of this VokeyAssetBundle.
	/// </summary>
	[XmlArray("VokeyAssets")]
    [XmlArrayItem("VokeyAsset")]
	public List<VokeyAsset> objects;
	
	public List<string> ids;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="VokeyAssetBundle"/> class.
	/// </summary>
	public VokeyAssetBundle ()
	{
		objects = new List<VokeyAsset> ();
		ids = new List<String> ();
	}
	
	/// <summary>
	/// Gets the game object by identifier.
	/// </summary>
	/// <returns>
	/// The game object by identifier.
	/// </returns>
	/// <param name='id'>
	/// Identifier.
	/// </param>
	/// <exception cref='UnityException'>
	/// Is thrown when the unity exception.
	/// </exception>
	public GameObject GetGameObjectById (string id)
	{
		if (ids.Contains (id)) {
			foreach (VokeyAsset va in objects) {
				if (va.name == id) 
					return (GameObject)GameObject.Instantiate (va.resource); 
			}
		}
		
		throw new UnityException ("Could not load asset: " + id + " from bundle " + name);
	}
	
	/// <summary>
	/// Serializes a room to Xml
	/// </summary>
	/// <param name="r"></param>
	/// <returns></returns>
	public static string ToXml (VokeyAssetBundle ab)
	{
		System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer (typeof(VokeyAssetBundle));
		StringBuilder sb = new StringBuilder ();
		XmlWriter xw = XmlWriter.Create (sb);
		x.Serialize (xw, ab);
		return sb.ToString ();
	}
	
	/// <summary>
	/// Froms the bundle.
	/// </summary>
	/// <returns>
	/// The bundle.
	/// </returns>
	/// <param name='contents'>
	/// Contents.
	/// </param>
	public static VokeyAssetBundle FromBundle (UnityEngine.Object[] contents)
	{
		VokeyAssetBundle a = new VokeyAssetBundle ();

		foreach (UnityEngine.Object o in contents) {
			if (o.GetType () == typeof(GameObject)) {
				//a.ids.Add(o.GetHashCode());
				((GameObject)o).name = "" + o.GetHashCode ();
				VokeyAsset va = new VokeyAsset();
				va.resource = o;
				a.objects.Add (va);
			} else {
				Debug.Log ("Object is nog a GameObject " + o.GetType().ToString());
			}
		}
		return a;
	}
	
	public static VokeyAssetBundle FromObjectsArray(UnityEngine.Object[] contents){
		VokeyAssetBundle a = new VokeyAssetBundle ();

		foreach (UnityEngine.Object o in contents) {
			a.objects.Add (VokeyAsset.FromAsset(o));
		}
		return a;
	}

}
