using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VokeySharedEntities
{
	[System.Serializable]
	public class Street
	{
	
	public enum StreetType {
		[XmlEnum(Name = "Residential")]
		Residential,
		[XmlEnum(Name = "Educational")]
		Educational
	}
	
    [XmlAttribute("Id")]
	/// <summary>
	/// The identifier.
	/// </summary>
    public System.Guid id;
    
    [XmlAttribute("Type")]
    public StreetType type;
    
    [XmlAttribute("Name")]
    public string name;
    
    [XmlArrayAttribute("Houses"), System.Xml.Serialization.XmlArrayItem(typeof(House))]
	/// <summary>
	/// The contained houses.
	/// </summary>
    public List<House> houses;
    
    public Street()
		{
		houses = new List<House>();
		}
    
    public Street(string name, StreetType type)
		{
		houses = new List<House>();
		this.name = name;
		this.type = type;
		id = Guid.NewGuid();
		}
    
    public House getHouse(Guid g)
    {
      foreach(House h in houses){
        if(h.id == g) return h;
      }
      return null;
    }
    
    /// <summary>
    /// Adds a house to the street.
    /// </summary>
    /// <param name="h">The house to add.</param>
    public void addHouse(House h)
		{
			houses.Add (h);
		}
    
	}
}

