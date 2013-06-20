using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using GuiTest;

namespace VokeySharedEntities
{
[System.Serializable]
	public class Town
	{
    [XmlAttribute("Id")]
	/// <summary>
	/// The identifier.
	/// </summary>
    public System.Guid id;
    
    [XmlAttribute("Name")]
    public string name;
    
    [XmlAttribute("ClassroomName")]
    public string classroomName;
    
    [XmlArray("Pupils"), System.Xml.Serialization.XmlArrayItem(typeof(User))]
    public List<User> pupils;
    
    [XmlArray("Streets"), System.Xml.Serialization.XmlArrayItem(typeof(Street))]
	/// <summary>
	/// The contained streets.
	/// </summary>
    public List<Street> streets;
    
    public List<Street> getPupilStreets()
	{
		List<Street> result = new List<Street>();
		
		return result;
	}
	
	public Town()
		{
		}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="VokeySharedEntities.Town"/> class.
	/// </summary>
	/// <param name="townName">Town name.</param>
	/// <param name="classroomName">Classroom name.</param>
	public Town(string townName, string classroomName){
		pupils =  new List<User>();
		streets = new List<Street>();
		id = Guid.NewGuid();
		this.classroomName = classroomName;
		this.name = townName;
	}
	
	public List<Street> getShoppingStreets()
	{
		List<Street> result = new List<Street>();
		
		return result;
	}
		
    /// <summary>
    /// Adds the street.
    /// </summary>
    /// <param name="s">S.</param>
    public void addStreet(Street s){
    	streets.Add (s);
	}
	
	    /// <summary>
    /// Adds the street.
    /// </summary>
    /// <param name="s">S.</param>
    public void addUser(User u){
    	u.townGuid = id;
    	pupils.Add (u);
	}
	
	public bool ContainsUser(string username)
		{
			foreach (User u in pupils) {
				if(u.username == username) {
					return true;
				}
			}
			return false;
		}
		
		public User getUser(string username){
			foreach (User u in pupils) {
				if(u.username == username) {
					return u;
				}
			}
			return null;
		}
		
		public User getUser(Guid userId){
			foreach (User u in pupils) {
				if(u.userGuid == userId) {
					return u;
				}
			}
			return null;
		}
		
		
}
}

