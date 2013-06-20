using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VokeySharedEntities
{
[System.Serializable]
	public class House
	{
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
    
    [XmlAttribute("OwnerGuid")]
    public System.Guid ownerId;
    
    
    public House()
		{
		}
		public House(GuiTest.User owner)
		{
			ownerId = owner.userGuid;
			id = Guid.NewGuid();
			rooms = new List<Room>();
			rooms.Add (new Room("Kitchen"));
		}
		
    [XmlArrayAttribute("Rooms"), System.Xml.Serialization.XmlArrayItem(typeof(Room))]
	/// <summary>
	/// The contained FindableObjects.
	/// </summary>
    public List<Room> rooms;

	}
}

