using System;
using GuiTest;

namespace VokeySharedEntities
{
	public class VokeySession
	{
		public static int sessionTimeout = 3600;
		private User user;
		public DateTime SessionStart;
		public DateTime SessionEnd {
			get {
				return SessionStart.AddSeconds (sessionTimeout);
			}
		}

		public string SessionHash {
			get {
				return EncryptionUtilities.GenerateSaltedSHA1("" + SessionStart.ToString () + user.Name);
			}
		}

		public VokeySession (User u)
		{
			user = u;
			SessionStart = DateTime.Now;
		}

		public static bool isValid(VokeySession v){
			return (DateTime.Now < v.SessionEnd);
		}
	}
}

