using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
[System.Serializable]
public class VokeyAsset {
    
    public Object resource;
    public string name;
    public System.Guid id;
	
	/// <summary>
	/// Froms the asset from a GameObject.
	/// </summary>
	/// <returns>
	/// The asset.
	/// </returns>
	/// <param name='bj'>
	/// The asset to turn into a VokeyAsset
	/// </param>
    public static VokeyAsset FromAsset(GameObject bj)
    {
        return null;
    }
}
