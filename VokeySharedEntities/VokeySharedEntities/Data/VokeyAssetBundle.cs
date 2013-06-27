using System.Collections.Generic;
using System.Text;
using System.Xml;
using System;
using System.Xml.Serialization;
using System.IO;

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
	
	/// <summary>
	/// The resource URL.
	/// </summary>
	[XmlAttribute("ResourceFilename")]
    public string resourceFilename;
	
    private Dictionary<int, VokeyAsset> containedAssets;
  
	/// <summary>
	/// The list of objects inside of this VokeyAssetBundle.
	/// </summary>
	[XmlArray("VokeyAssets")]
    [XmlArrayItem("VokeyAsset")]
	public List<VokeyAsset> objects;
	
	public List<string> ids;
  
    private bool _binaryFilesLoaded = false;
    [XmlIgnore]
    public bool binaryFilesLoaded
    {
      get
      {
        return _binaryFilesLoaded;
      }
    }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="VokeyAssetBundle"/> class.
	/// </summary>
	public VokeyAssetBundle ()
	{
		objects = new List<VokeyAsset> ();
		ids = new List<String> ();
        containedAssets = new Dictionary<int, VokeyAsset>();
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
	public UnityEngine.GameObject GetGameObjectById (int id)
	{
        VokeyAsset theAsset = null;
        containedAssets.TryGetValue(id, out theAsset);
        if(theAsset == null) throw new UnityEngine.UnityException ("Could not load asset: " + id + " from bundle " + name);
        return (UnityEngine.GameObject)UnityEngine.GameObject.Instantiate (theAsset.resource); 
	}
	
	/// <summary>
	/// Gets an array of Object that are from the specified type
	/// </summary>
	/// <returns>
	/// The game object array by type.
	/// </returns>
	/// <param name='type'>
	/// Type of objects to get.
	/// </param>
	public VokeyAsset[] GetObjectArrayByType(string type){
		List<VokeyAsset> resultObjects = new List<VokeyAsset>();
		foreach(VokeyAsset va in objects){
			if(va.ObjectType.Equals(type)) resultObjects.Add (va);
		}
		return resultObjects.ToArray();
	}
  
    public void LoadBinaryObjects(UnityEngine.Object[] binaryAssets)
  {
      for (int i = 0; i > binaryAssets.Length; i++)
      {
        objects[i].loadResource( binaryAssets[i]);
      }
        _binaryFilesLoaded = true;
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
			if (o.GetType () == typeof(UnityEngine.GameObject)) {
				//a.ids.Add(o.GetHashCode());
				((UnityEngine.GameObject)o).name = "" + o.GetHashCode ();
				VokeyAsset va = new VokeyAsset();
				va.resource = o;
				a.objects.Add (va);
			} 
		}
        a._binaryFilesLoaded = true;
		return a;
	}
	
	/// <summary>
	/// Froms the VokeyAssetBundle from an objects array.
	/// </summary>
	/// <returns>
	/// The VokeyAssetBundle formed from the objects array.
	/// </returns>
	/// <param name='contents'>
	/// Contents.
	/// </param>
	public static VokeyAssetBundle FromObjectsArray(UnityEngine.Object[] contents){
		VokeyAssetBundle a = new VokeyAssetBundle ();

		foreach (UnityEngine.Object o in contents) {
			a.objects.Add (VokeyAsset.FromAsset(o));
		}
		return a;
	}
  
  public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
