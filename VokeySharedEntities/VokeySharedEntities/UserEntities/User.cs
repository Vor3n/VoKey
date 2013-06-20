using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using VokeySharedEntities;

namespace GuiTest
{
	[System.Serializable]
	public class User
	{
		public enum UserType {
			[XmlEnum(Name = "Student")]
			Student,
			[XmlEnum(Name = "Teacher")]
			Teacher
		}
	
		[XmlElement("FullName")]
		public string FullName {
			get;
			set;
		}
		
		[XmlAttribute("Username")]
    	public string username;

		[XmlAttribute("UserType")]
		public UserType type;

		[XmlElement("PasswordHash")]
		public string PasswordHash {
			get;
			set;
		}

		[XmlAttribute("Town")]
		public Guid townGuid {
			get;
			set;
		}

		[XmlElement("House")]
		public House userHouse{
			get;
			set;
		}

		[XmlAttribute("UserGuid")]
		public Guid userGuid {
			get;
			set;
		}

		/// <summary>
		/// Sets the password.
		/// </summary>
		/// <param name="password">Password.</param>
		public void setPassword(string password){
			PasswordHash = EncryptionUtilities.GenerateSaltedSHA1 (password);
		}
		
		public User()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GuiTest.User"/> class.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public User (string username, string password, UserType userType)
		{
			userGuid = Guid.NewGuid ();
			type = userType;
			setPassword (password);
			FullName = username;
			this.username = username;
			userHouse = new House(this);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="GuiTest.User"/> class.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public User (string username, string password, string fullName, UserType userType)
		{
			userGuid = Guid.NewGuid ();
			type = userType;
			setPassword (password);
			this.username = username;
			FullName = fullName;
			userHouse = new House(this);
		}


		public static string SerializeToXML(List<User> userList)
		{
			var serializer = new XmlSerializer(typeof(List<User>));
			string utf8;
			using (StringWriter writer = new VokeyAssetBundle.Utf8StringWriter())
			{
				serializer.Serialize(writer, userList);
				utf8 = writer.ToString();
			}
			return utf8;
		}
	}
}
