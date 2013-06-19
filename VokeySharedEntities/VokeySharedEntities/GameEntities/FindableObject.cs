using System.Collections;
using System.Xml.Serialization;

[System.Serializable]
public class FindableObject {
    [XmlAttribute("Friendly Name")]
	/// <summary>
	/// The friendly name for the FindableObject
	/// </summary>
    public string friendlyName;

    [XmlAttribute("Id")]
	/// <summary>
	/// The identifier.
	/// </summary>
    public System.Guid id;

    /// <summary>
    /// Constructor for serialisation
    /// </summary>
    public FindableObject()
    {

    }

    [XmlAttribute("Position")]
	/// <summary>
	/// The position.
	/// </summary>
    public UnityEngine.Vector3 position;
    [XmlAttribute("Scale")]
	/// <summary>
	/// The scale.
	/// </summary>
	public UnityEngine.Vector3 scale;
    [XmlAttribute("Rotation")]
	/// <summary>
	/// The rotation.
	/// </summary>
    public UnityEngine.Quaternion rotation;
    [XmlAttribute("LinkedObject")]
	/// <summary>
	/// The game object identifier.
	/// </summary>
    public System.Guid GameObjectId;

    /// <summary>
    /// Constructor for a new FindableObject.
    /// </summary>
    /// <param name="friendlyName">Name of the object</param>
    /// <param name="originalInstance">The original instance to create a clone of</param>
    public FindableObject(string friendlyName, UnityEngine.GameObject originalInstance)
    {
        position = originalInstance.transform.localPosition;
        scale = originalInstance.transform.localScale;
        rotation = originalInstance.transform.localRotation;
        GameObjectId = new System.Guid(originalInstance.name);
    }
}
