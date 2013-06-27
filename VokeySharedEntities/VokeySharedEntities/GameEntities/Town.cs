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
		List<Street> total = new List<Street>();
		total.AddRange (_residentialStreets);
		total.AddRange(_educationalStreets);
		return total;
	}
	
	private List<Street> _residentialStreets = null;
	
	[XmlIgnore]
	public List<Street> residentialStreets {
		get {
			if(_residentialStreets == null) {
				_residentialStreets = new List<Street> ();
				foreach (Street s in streets) {
					if(s.type == Street.StreetType.Residential) {
						_residentialStreets.Add (s);
					}
				}
				foreach (User p in pupils) {
					addPupilToFirstAvaliableStreet (p);
				}
			}
			return _residentialStreets;
		}
		set {
			_residentialStreets = value;
		}
	}
	
	private List<Street> _educationalStreets = null;
	
	[XmlIgnore]
	public List<Street> educationalStreets {
		get {
			if(_educationalStreets == null) {
				_educationalStreets = new List<Street> ();
				foreach (Street s in streets) {
					if(s.type == Street.StreetType.Educational) {
						_educationalStreets.Add (s);
					}
				}
			}
			return _educationalStreets;
		}
		set {
			_educationalStreets = value;
		}
	}
	
	public void addPupilToFirstAvaliableStreet(User pupil)
		{
			foreach (Street s in residentialStreets) {
				if(s.type == Street.StreetType.Residential) {
					if(s.houses.Count < 10) {
						s.addHouse (pupil.userHouse);
						return;
					}
				}
			}
			UnityEngine.Debug.LogError("No streets available to stuff pupils in! Number of streets: " + residentialStreets.Count);
		}
	
	public Town()
	{
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="VokeySharedEntities.Town"/> class.
	/// </summary>
	/// <param name="townName">Town name.</param>
	/// <param name="classroomName">Classroom name.</param>
	public Town(string townName, string classroomName) {
		pupils =  new List<User>();
		streets = new List<Street>();
		id = Guid.NewGuid();
		this.classroomName = classroomName;
		this.name = townName;
	}
		
    /// <summary>
    /// Adds the street.
    /// </summary>
    /// <param name="s">S.</param>
    public void addStreet(Street s)
		{
			if(s.type == Street.StreetType.Educational) {
				streets.Add (s);
				if(_educationalStreets != null) educationalStreets.Add (s);
			} else {
				streets.Add (s);
				if(_residentialStreets != null) residentialStreets.Add (s);
			}
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
			foreach (Street s in residentialStreets) {
				if(s.id == id) {
					return s;
				} else {
					UnityEngine.Debug.Log (s.id + " does not match " + id);
				}
			}
			foreach (Street s in educationalStreets) {
				if(s.id == id) {
					return s;
				}
			}
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
        
        
            public bool ContainsUser(Guid id)
        {
            foreach (User u in pupils) {
                if(u.userGuid == id) {
                    return true;
                }
            }
            return false;
        }
		
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="username">Username.</param>
		public User getUser(string username){
			foreach (User u in pupils) {
				if(u.username == username) {
					return u;
				}
			}
			return null;
		}
        
        /// <summary>
        /// Removes the user.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void removeUser(Guid id)
        {
            if (ContainsUser(id))
            {
                User ux = null;
                foreach (User u in pupils)
                {
                    if (u.userGuid == id)
                    {
                        ux = u;
                    }
                }
                pupils.Remove(ux);
            }
            else
            {
                throw new Exception ("User does not exist");
            }
            
        }
		
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="userId">User identifier.</param>
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

