using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using Thisiswhytheinternetexists;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.InteropServices;
[System.Serializable]
public class VokeyAsset {
    [XmlIgnoreAttribute]
    public UnityEngine.Object resource;
	[XmlAttribute("Name")]
    public string name;
	
	[XmlAttribute("Hash")]
    public int hashString;
	
	/// <summary>
	/// Froms the asset from a GameObject.
	/// </summary>
	/// <returns>
	/// The asset.
	/// </returns>
	/// <param name='bj'>
	/// The asset to turn into a VokeyAsset
	/// </param>
    public static VokeyAsset FromAsset(UnityEngine.Object bj)
    {
		VokeyAsset va = new VokeyAsset();
		va.resource = bj;
		va.name = bj.name;
		va.hashString = va.GetHashCode();
		Debug.Log ("Object Hash: " + va.hashString);
        return va;
    }
	
	
	/// <summary>
    /// Calculates the lenght in bytes of an object 
    /// and returns the size 
    /// </summary>
    /// <param name="TestObject"></param>
    /// <returns></returns>
    private int GetObjectSize(object TestObject)
    {
        /*BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        byte[] Array;
        bf.Serialize(ms, TestObject);
        Array = ms.ToArray();
        return Array.Length;*/
		return Marshal.SizeOf(TestObject); 
    }
	
	public override int GetHashCode()
	{
	    unchecked
	    {
	        int result = (name != null ? name.GetHashCode() : 0);
	        result = (result*397) ^ (resource.name != null ? resource.name.GetHashCode() : 0);
			result = (result*397) ^ (resource != null ? resource.GetType().ToString().GetHashCode() : 0);
	        //result = (result*397) ^ (resource != null ? GetObjectSize(resource) : 0);
	        return result;
	    }
	}
	
}
