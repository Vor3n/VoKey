using System;
using GuiTest;

namespace VokeySharedEntities
{
	public class VokeySession
	{
		public static int sessionTimeout = 3600;
		private bool invalidated = false;
		
		public bool IsTeacher {
			get {
				return (user.type == User.UserType.Teacher);
			}
		}
		
		public bool IsStudent {
			get {
				return (user.type == User.UserType.Student);
			}
		}
		
		public bool isValid {
			get {
				return (!invalidated && DateTime.Now < SessionEnd);
			}
		}
		
		public void heartBeat()
		{
			SessionStart.AddMinutes(30);
		}
		
		private User user;
		public DateTime SessionStart;
		public DateTime SessionEnd {
			get {
				return SessionStart.AddSeconds (sessionTimeout);
			}
		}

		public string SessionHash {
			get {
				return EncryptionUtilities.GenerateSaltedSHA1("" + SessionStart.ToString () + user.username);
			}
		}

		public VokeySession (User u)
		{
			user = u;
			SessionStart = DateTime.Now;
		}
		
		public void Invalidate(){
			invalidated = true;
		}
	}
}

