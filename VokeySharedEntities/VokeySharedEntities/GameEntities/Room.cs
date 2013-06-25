using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.Xml;
using System.Text;
using System.IO;

namespace VokeySharedEntities
{
[System.Serializable]
public class Room {
    [XmlAttribute("Name")]
	/// <summary>
	/// The name.
	/// </summary>
    public string name;

    [XmlAttribute("Description")]
	/// <summary>
	/// The description.
	/// </summary>
    public string description;

    [XmlAttribute("Id")]
	/// <summary>
	/// The identifier.
	/// </summary>
    public System.Guid id;

    [XmlArrayAttribute("FindableObjects"), System.Xml.Serialization.XmlArrayItem(typeof(FindableObject))]
	/// <summary>
	/// The contained FindableObjects.
	/// </summary>
    public List<FindableObject> containedObjects;

    [XmlIgnore]
	/// <summary>
	/// Gets the number of objects.
	/// </summary>
	/// <value>
	/// The number of objects.
	/// </value>
    public int NumberOfObjects
    {
        get
        {
            return containedObjects.Count;
        }
    }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Room"/> class.
	/// </summary>
    public Room()
    {

    }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Room"/> class.
	/// </summary>
	/// <param name='name'>
	/// Name of the room to create.
	/// </param>
    public Room(string name)
    {
        containedObjects = new List<FindableObject>();
		id = System.Guid.NewGuid();
		
        this.name = name;
    }

    /// <summary>
    /// Adds an instance of gameobject to the room at the specified position.
    /// </summary>
    /// <param name="g">The GameObject to add</param>
    /// <param name="position">The position to add the object at</param>
    public void AddGameObject(GameObject g, Vector3 position)
    {
        AddGameObject(g, position, new Vector3(1, 1, 1), new Vector3(0, 0, 0));
    }

    /// <summary>
    /// Adds an instance of gameobject to the room at the specified position.
    /// </summary>
    /// <param name="g">The GameObject to add</param>
    /// <param name="position">The position to add the object at</param>
    /// <param name="scale">The scale that the object should have</param>
    public void AddGameObject(GameObject g, Vector3 position, Vector3 scale)
    {
        AddGameObject(g, position, scale, new Vector3(0, 0, 0));
    }

    /// <summary>
    /// Adds an instance of gameobject to the room at the specified position.
    /// </summary>
    /// <param name="g">The GameObject to add</param>
    /// <param name="position">The position to add the object at</param>
    /// <param name="scale">The scale that the object should have</param>
    /// <param name="rotation">The rotation of the object</param>
    public void AddGameObject(GameObject g, Vector3 position, Vector3 scale, Vector3 rotation)
    {
        GameObject theObject = (GameObject)GameObject.Instantiate(g);
        theObject.transform.position = position;
        theObject.transform.localScale = scale;
        Quaternion rot = theObject.transform.rotation;
        theObject.transform.rotation = rot * Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        theObject.name = System.Guid.NewGuid().ToString("D");
        containedObjects.Add(new FindableObject(theObject.name, theObject));
    }
}
}
