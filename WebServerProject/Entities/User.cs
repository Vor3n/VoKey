using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

namespace GuiTest
{
	public enum UserType {
		Student,
		Teacher
	}

	[System.Serializable]
	public class User
	{
		[XmlAttribute("Name")]
		public string Name {
			get;
			set;
		}

		[XmlAttribute("UserType")]
		public UserType type;

		[XmlAttribute("PasswordHash")]
		public string PasswordHash {
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

		/// <summary>
		/// Initializes a new instance of the <see cref="GuiTest.User"/> class.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public User (string username, string password, UserType userType)
		{
			userGuid = Guid.NewGuid ();
			setPassword (password);
			Name = username;
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
