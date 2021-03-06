using System.Xml.Serialization;

namespace VokeySharedEntities
{

    [System.Serializable]
    public class SerializableVector3
    {
        public SerializableVector3(){}
        [XmlAttribute("x")]
        public float x;
		[XmlAttribute("y")]
        public float y;
		[XmlAttribute("z")]
        public float z;

    }
	
	 [System.Serializable]
    public class SerializableQuaternion
    {
        public SerializableQuaternion(){}
		[XmlAttribute("w")]
		public float w;
		[XmlAttribute("x")]
        public float x;
		[XmlAttribute("y")]
        public float y;
		[XmlAttribute("z")]
        public float z;

    }



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

    [XmlIgnore]
	/// <summary>
	/// The position.
	/// </summary>
    public UnityEngine.Vector3 position
    {
        get
        {
            return new UnityEngine.Vector3(_position.x, _position.y, _position.z);
        }
        set
        {
            _position = new SerializableVector3();
            _position.x = value.x;
            _position.y = value.y;
            _position.z = value.z;
        }
    }

    [XmlElement("Position")]
    public SerializableVector3 _position;

    [XmlIgnore]
    /// <summary>
    /// The scale.
    /// </summary>
    public UnityEngine.Vector3 scale
    {
        get
        {
            return new UnityEngine.Vector3(_scale.x, _scale.y, _scale.z);
        }
        set
        {
            _scale = new SerializableVector3();
            _scale.x = value.x;
            _scale.y = value.y;
            _scale.z = value.z;
        }
    }
    
    [XmlElement("Scale")]
    public SerializableVector3 _scale;

    [XmlIgnore]
	/// <summary>
	/// The rotation.
	/// </summary>
    public UnityEngine.Quaternion rotation{
			
			 get
        {
            return new UnityEngine.Quaternion(_rotation.w,_rotation.x, _position.y, _position.z);
        }
        set
        {
            _rotation = new SerializableQuaternion();
			_rotation.w = value.w;
            _rotation.x = value.x;
            _rotation.y = value.y;
            _rotation.z = value.z;
        }
	}
		
		
	[XmlElement("Rotation")]	
	public SerializableQuaternion _rotation;
    [XmlAttribute("LinkedObject")]
	/// <summary>
	/// The game object identifier.
	/// </summary>
    public string GameObjectId;
		
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
        GameObjectId = friendlyName;
        id = System.Guid.NewGuid();
		
		
    }
}
}
