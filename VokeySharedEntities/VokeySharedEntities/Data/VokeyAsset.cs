using System.Xml.Serialization;
[System.Serializable]
public class VokeyAsset {
    [XmlIgnoreAttribute]
    public UnityEngine.Object resource;
	[XmlAttribute("Name")]
    public string name;
	
	[XmlAttribute("Hash")]
    public int hashString;
	
	[XmlAttribute("Type")]
	public string ObjectType = "";
	
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
		va.ObjectType = bj.GetType().ToString();
        return va;
    }
    
    public void loadResource(UnityEngine.Object go)
    {
      UnityEngine.Debug.Log("Object Name in parameter" + go.name);
      resource = UnityEngine.Object.Instantiate(go);
    }
	
	/// <summary>
	/// Serves as a hash function for a <see cref="VokeyAsset"/> object.
	/// </summary>
	/// <returns>
	/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
	/// </returns>
	public override int GetHashCode()
	{
	    unchecked
	    {
	        int result = (name != null ? name.GetHashCode() : 0);
			result = (result*397) ^ (ObjectType != null ? ObjectType.GetHashCode() : 0);
	        result = (result*397) ^ (name != null ? name.GetHashCode() : 0);
	        return result;
	    }
	}
}
