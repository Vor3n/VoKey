using System;
using System.Collections.Generic;

namespace VokeySharedEntities
{
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
    
    [XmlArrayAttribute("Rooms"), System.Xml.Serialization.XmlArrayItem(typeof(Room))]
	/// <summary>
	/// The contained FindableObjects.
	/// </summary>
    public List<Room> rooms;
		public House(GuiTest.User owner)
		{
			ownerId = owner.userGuid;
			
		}
	}
}

