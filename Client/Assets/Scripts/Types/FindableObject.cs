using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

[System.Serializable]
public class FindableObject {
    [XmlAttribute("Friendly Name")]
    public string friendlyName;

    [XmlAttribute("Id")]
    public System.Guid id;

    /// <summary>
    /// Constructor for serialisation
    /// </summary>
    public FindableObject()
    {

    }

    [XmlAttribute("Position")]
    public Vector3 position;
    [XmlAttribute("Scale")]
    public Vector3 scale;
    [XmlAttribute("Rotation")]
    public Quaternion rotation;
    [XmlAttribute("LinkedObject")]
    public System.Guid GameObjectId;

    /// <summary>
    /// Constructor for a new FindableObject.
    /// </summary>
    /// <param name="friendlyName">Name of the object</param>
    /// <param name="originalInstance">The original instance to create a clone of</param>
    public FindableObject(string friendlyName, GameObject originalInstance)
    {
        position = originalInstance.transform.localPosition;
        scale = originalInstance.transform.localScale;
        rotation = originalInstance.transform.localRotation;
        GameObjectId = new System.Guid(originalInstance.name);
    }
}
