using System;
using GuiTest;

namespace VokeySharedEntities
{
	public class VokeySessionContainer
	{
		public VokeySessionContainer ()
		{
			sessions = new System.Collections.Generic.Dictionary<string, VokeySession> ();
		}

		private System.Collections.Generic.Dictionary<string, VokeySession> sessions;

		/// <summary>
		/// Gets the session.
		/// </summary>
		/// <returns>The session.</returns>
		/// <param name="sessionHash">Session hash.</param>
		public VokeySession getSession(string sessionHash){
			VokeySession vs = null;
			if (sessions.ContainsKey (sessionHash)) 
				sessions.TryGetValue (sessionHash, out vs);
			return vs;
		}

		/// <summary>
		/// Creates the vokey session.
		/// </summary>
		/// <returns>The vokey session.</returns>
		/// <param name="u">U.</param>
		public string CreateVokeySession (User u){
			VokeySession v = new VokeySession (u);
			sessions.Add (v.SessionHash, v);
			return v.SessionHash;
		}
	}
}

