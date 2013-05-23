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
    public System.Uri resourceUrl;
	
	/// <summary>
	/// The list of objects inside of this VokeyAssetBundle.
	/// </summary>
    public List<VokeyAsset> objects;
    public List<string> ids;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="VokeyAssetBundle"/> class.
	/// </summary>
    public VokeyAssetBundle()
    {
        objects = new List<VokeyAsset>();
        ids = new List<String>();
    }

    /// <summary>
    /// Serializes a room to Xml
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public static string ToXml(AssetBundle ab)
    {
        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(AssetBundle));
        StringBuilder sb = new StringBuilder();
        XmlWriter xw = XmlWriter.Create(sb);
        x.Serialize(xw, ab);
        return sb.ToString();
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
    public static VokeyAssetBundle FromBundle(UnityEngine.Object[] contents)
    {
        VokeyAssetBundle a = new VokeyAssetBundle();

        foreach (object o in contents)
        {
            if (o.GetType() == typeof(GameObject))
            {
                //a.ids.Add(o.GetHashCode());
                ((GameObject)o).name = "" + o.GetHashCode();
                a.objects.Add(null);
            }
        }
        return a;
    }

}
