using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using VokeySharedEntities;

namespace GuiTest
{
	[System.Serializable]
	public class VokeyUser : Thisiswhytheinternetexists.WebCore.User
	{
		public enum VokeyUserType {
			[XmlEnum(Name = "Student")]
			Student,
			[XmlEnum(Name = "Teacher")]
			Teacher
		}
		

		[XmlAttribute("UserType")]
		public VokeyUserType vtype;

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
        
        [XmlElement("Assignments")]
        public AssignmentList assignments {
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
		
		public VokeyUser()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GuiTest.User"/> class.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public VokeyUser (string username, string password, VokeyUserType userType)
		{
			userGuid = Guid.NewGuid ();
			vtype = userType;
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
		public VokeyUser (string username, string password, string fullName, VokeyUserType userType)
		{
			userGuid = Guid.NewGuid ();
			vtype = userType;
			setPassword (password);
			this.username = username;
			FullName = fullName;
			userHouse = new House(this);
		}

        public void addAssignment(Assignment a)
        {
            if(assignments == null) assignments = new AssignmentList();
            assignments.Add(a);
        }
	}
}
