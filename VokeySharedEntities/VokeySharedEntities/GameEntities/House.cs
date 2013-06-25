using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VokeySharedEntities
{
[System.Serializable]
	public class House
	{
	
	public enum HouseType
	{
		Educational,
		Residential
	}	
	
	[XmlAttribute("Name")]
	/// <summary>
	/// The name of the house.
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
    
    [XmlAttribute("OwnerGuid")]
    public System.Guid ownerId;
    
    /// <summary>
    /// The type of this house.
    /// </summary>
    public HouseType type;
    
    public House()
	{
	}
		
	public House(GuiTest.User residentialHouseOwner)
	{
		name = residentialHouseOwner.FullName + "'s House";
		type = HouseType.Educational;
		ownerId = residentialHouseOwner.userGuid;
		id = Guid.NewGuid();
		rooms = new List<Room>();
		rooms.Add (new Room("Kitchen"));
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="VokeySharedEntities.House"/> class.
	/// </summary>
	/// <param name="name">Name.</param>
	public House(string shopName, string shopDescription)
	{
		id = Guid.NewGuid();
		rooms = new List<Room>();
		rooms.Add (new Room(shopName));
		description = shopDescription;
	}
		
    [XmlArrayAttribute("Rooms"), System.Xml.Serialization.XmlArrayItem(typeof(Room))]
	/// <summary>
	/// The contained FindableObjects.
	/// </summary>
    public List<Room> rooms;

	}
}

