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
				return (_user.vtype == VokeyUser.VokeyUserType.Teacher);
			}
		}
		
		public bool IsStudent {
			get {
				return (_user.vtype == VokeyUser.VokeyUserType.Student);
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
		
		public VokeyUser User{
			get {
				return _user;
			}
		}
		
		private VokeyUser _user;
		public DateTime SessionStart;
		public DateTime SessionEnd {
			get {
				return SessionStart.AddSeconds (sessionTimeout);
			}
		}

		public string SessionHash {
			get {
				return EncryptionUtilities.GenerateSaltedSHA1("" + SessionStart.ToString () + _user.username);
			}
		}

		public VokeySession (VokeyUser u)
		{
			_user = u;
			SessionStart = DateTime.Now;
		}
		
		public void Invalidate(){
			invalidated = true;
		}
	}
}

