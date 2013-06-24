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
    
    public List<Street> getFilledStreets()
	{
		List<Street> result = new List<Street>();
		return result;
	}
	
	private List<Street> _pupStreets = null;
	
	[XmlIgnore]
	public List<Street> pupilStreets {
		get {
			if(_pupStreets == null) {
				_pupStreets = new List<Street> ();
				foreach (Street s in streets) {
					if(s.type == Street.StreetType.Residential) {
						_pupStreets.Add (s);
					}
				}
				foreach (User p in pupils) {
					addPupilToFirstAvaliableStreet (p);
				}
			}
			return _pupStreets;
		}
		set {
			_pupStreets = value;
		}
	}
	
	public void addPupilToFirstAvaliableStreet(User pupil)
		{
			foreach (Street s in _pupStreets) {
				if(s.type == Street.StreetType.Residential) {
					if(s.houses.Count < 10) {
						s.addHouse(pupil.userHouse);
						return;
					}
				}
			}
			UnityEngine.Debug.LogError("No streets available to stuff pupils in!");
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
	
		public bool ContainsStreet(Guid id)
		{
			foreach (Street s in streets) {
				if(s.id == id) {
					return true;
				}
			}
			return false;
		}
		
		public Street getStreet(Guid id)
		{
			foreach (Street s in pupilStreets) {
				if(s.id == id) {
					return s;
				}
			}
			/*foreach (Street s in pupilStreets) {
				if(s.id == id) {
					return s;
				}
			}*/
			throw new Exception("The specified street is not found in this town.");
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

