using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using GuiTest;
using System.Xml.Serialization;
using VokeySharedEntities;

namespace Vokey
{
		[System.Serializable]
	public class AssetServer
		{
				public bool Running {
						get {
								return ws.Running;
						}
				}

		public static string AssetRoot = "";
				public static string[] acceptableCommands = {"rooms", "assetbundles", "login", "houses", "file", "favicon.ico"};
		private static AssetServer myInstance;

		public static AssetServer getInstance ()
		{
			if (myInstance == null)
				myInstance = new AssetServer ();
			return myInstance;
		}

		private WebServer ws;
		private List<Room> _roomList;

		/// <summary>
		/// Occurs a message is logged.
		/// </summary>
		public event Action<string> LogMessage;

		public List<Room> RoomList {
			get {
				if (_roomList == null)
					_roomList = new List<Room> ();
				return _roomList;
			}
		}

		[XmlArray("Users")]
		[XmlArrayItem("User")]
		public List<User> Users {
			get;
			set;
		}

		/// <summary>
		/// Log the specified message.
		/// </summary>
		/// <param name='message'>
		/// Message.
		/// </param>
		private void Log (string message)
		{
			if (LogMessage != null)
				LogMessage (message);
		}

		private List<VokeyAssetBundle> assetBundles;
		private BackgroundWorker scanForAssetsWorker;
		private VokeySessionContainer sessions;

		public AssetServer ()
		{
			myInstance = this;
			assetBundles = new List<VokeyAssetBundle> ();
			Users = new List<User> ();
			sessions = new VokeySessionContainer ();
			scanForAssetsWorker = new BackgroundWorker ();
			scanForAssetsWorker.DoWork += ScanForAssetsWorkerWork;
			readUserData ();
			string fullPath = System.Reflection.Assembly.GetAssembly (typeof(AssetServer)).Location;
			AssetRoot = Path.GetDirectoryName (fullPath) + System.IO.Path.DirectorySeparatorChar + ".." + System.IO.Path.DirectorySeparatorChar + "..";

			//scanForAssetsWorker.
		}

		/// <summary>
		/// Makes the AssetServer Scan for Assets (this is a event handler)
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void ScanForAssetsWorkerWork (object sender, DoWorkEventArgs e)
		{
			scanForAssets ();
		}

		void readUserData (){
			Users.Add (new User ("pascal", "pascal", UserType.Teacher));
			Users.Add (new User ("student", "student", UserType.Student));
			Users.Add (new User ("teacher", "teacher", UserType.Teacher));

		}

		/// <summary>
		/// Adds the user.
		/// </summary>
		/// <param name="u">The user t add.</param>
		void addUser(User u){
			Users.Add (u);
		}

		/// <summary>
		/// Start this AssetServer.
		/// </summary>
		public void Start ()
		{
			ws = new WebServer (SendResponse, "http://+:8080/");
			ws.LogMessage += HandleWsLogMessage;
			ws.Run ();
		}

		/// <summary>
		/// Stop this instance of the WebServer.
		/// </summary>
		public void Stop(){
			ws.Stop ();
		}

		/// <summary>
		/// Handles the ws log message.
		/// </summary>
		/// <param name="obj">Object.</param>
		void HandleWsLogMessage (string obj)
		{
			Log (obj);
		}

		/// <summary>
		/// Scans for assets
		/// </summary>
		public void scanForAssets ()
		{

			int numBefore = assetBundles.Count;
			Log ("location to scan: " + AssetRoot);
			string[] xmlBundleMetaFiles = Directory.GetFiles (AssetRoot + System.IO.Path.DirectorySeparatorChar + "AssetBundles", "vab*.xml");
			foreach (string s in xmlBundleMetaFiles) {
				VokeyAssetBundle vob = VokeyAssetBundle.FromXml (s);
				if (!assetBundles.Contains (vob))
					assetBundles.Add (vob);

			}
			Log ("Number of assets in the bundle after scan: " + assetBundles.Count + "before was: " + numBefore);

		}

		/// <summary>
		/// Serializes all rooms.
		/// </summary>
		/// <returns>The all rooms.</returns>
		public string serializeAllRooms ()
		{
			string s = "";
			//XmlDocument d = Room.Serialize(RoomList);

			foreach (Room r in RoomList) {
				s += Room.ToXml (r);//.InnerXml.ToString() + "\n";
			}
			//return d.DocumentElement.ToString();
			return s;
		}

		/// <summary>
		/// Sends a standard response.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="content">Content.</param>
		public void sendStandardResponse (HttpListenerContext request, byte[] content)
		{
			Console.WriteLine ("sendStandardResponse start");
			request.Response.ContentLength64 = content.Length;
			request.Response.OutputStream.Write (content, 0, content.Length);
			Console.WriteLine ("sendStandardResponse stop");
		}

		/// <summary>
		/// Sends a file and uses the provided content type.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="contentType">Content type.</param>
		/// <param name="data">Data.</param>
		public void sendFileWithContentType(HttpListenerContext request, string contentType, byte[] data){
			Console.WriteLine ("sendFileWithContentType start");
			request.Response.ContentType = contentType;
			request.Response.ContentLength64 = data.Length;
			request.Response.OutputStream.Write (data, 0, data.Length);
			Console.WriteLine ("sendFileWithContentType stop");
		}

		/// <summary>
		/// Handles a file request.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="filename">Filename.</param>
		public void handleFileRequest(HttpListenerContext request, string filename){
			getInstance ().sendStandardResponse (request, Encoding.UTF8.GetBytes (string.Format ("<HTML><BODY>Vokey Server<br>" + DateTime.Now.ToShortTimeString () + " {0}</BODY></HTML>", DateTime.Now)));
		}

		public Dictionary<string, string> getPostDataFromRequest (HttpListenerContext hlc)
				{
			Dictionary<string, string> formData = new Dictionary<string, string> ();
			if (hlc.Request.HasEntityBody)

			{

				System.IO.Stream body = hlc.Request.InputStream;

				System.Text.Encoding encoding = hlc.Request.ContentEncoding;

				System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);

				if (hlc.Request.ContentType.ToLower() == "application/x-www-form-urlencoded")

				{

					string s = reader.ReadToEnd();

					string[] pairs = s.Split('&');
					for (int x = 0; x < pairs.Length; x++)
					{
						string[] item = pairs[x].Split('=');
						formData.Add(item[0],item[1]);
						UnityEngine.Debug.Log ("Form data: " + item[0] + " = " + item[1]);
					}

				}

				body.Close();

				reader.Close();

			}
			return formData;

				}

		/// <summary>
		/// Handles an unknown request.
		/// </summary>
		/// <param name="request">Request.</param>
		public void handleUnknownRequest(HttpListenerContext request){
			getInstance ().sendStandardResponse (request, Encoding.UTF8.GetBytes (string.Format ("<HTML><BODY>Vokey Server<br>" + DateTime.Now.ToShortTimeString () + " {0}</BODY></HTML>", DateTime.Now)));
		}

		/// <summary>
		/// Sends the text response.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="message">Message.</param>
		public void sendTextResponse(HttpListenerContext request, string message){
			getInstance ().sendStandardResponse (request, Encoding.UTF8.GetBytes (string.Format ("<HTML><BODY>Vokey Server<br>" + DateTime.Now.ToShortTimeString () + " {0}<br />" + message + "</BODY></HTML>", DateTime.Now)));
		}

		/// <summary>
		/// Returns whether the argument handleable.
		/// </summary>
		/// <returns><c>true</c>, if argument was handable , <c>false</c> otherwise.</returns>
		/// <param name="argument">Argument.</param>
		public static bool argumentHandleable(string argument){
			Console.WriteLine ("argumentHandleable start");
			if (argument == "rooms" || argument == "assetbundles" || argument == "login" || argument == "houses" || argument == "file" || argument == "favicon.ico"){
				Console.WriteLine ("argumentHandleable stop");
				return true;
			}
			Console.WriteLine ("argumentHandleable stop");
			return false;
		}

		public static bool isLastInArguments(string[] arguments, string item){
			int length = arguments.Length;
			int count = 1;
			foreach (string s in arguments) {
				if (s == item)
					return (count == length);
				count++;
			}
			Console.WriteLine ("SMACK SOMEONE WITH AN ERASER. THERE IS SOMETHING VERY WRONG.");
			return false;
		}

		public void getAssetListAndReturnToHttpClient(HttpListenerContext request){
			List<VokeyAssetBundle> tempList = new List<VokeyAssetBundle> ();
			byte[] returnBytes;
			foreach (VokeyAssetBundle vab in getInstance().assetBundles) {
				tempList.Add (vab); 
			}
			try {
				returnBytes = Encoding.UTF8.GetBytes (VokeyAssetBundle.SerializeToXML (tempList));
			} catch (Exception e) {
				Console.WriteLine (e.GetBaseException ());
				returnBytes = Encoding.UTF8.GetBytes ("No data in response");
			}
			getInstance ().sendStandardResponse (request, returnBytes);
		}

		/// <summary>
		/// Tries the login. 
		/// </summary>
		/// <returns><c>true</c>, if login was correct, <c>false</c> otherwise.</returns>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public User TryLogin(string username, string password){
			foreach (User u in Users) {
				if (u.Name == username) {
					UnityEngine.Debug.Log ("Username match");
										if (u.PasswordHash == EncryptionUtilities.GenerateSaltedSHA1 (password)) {
												UnityEngine.Debug.Log ("Password match");
												return u;
										} else {
						UnityEngine.Debug.Log (u.PasswordHash);
										}
				}
			}
			return null;
		}

		/// <summary>
		/// Sends the response.
		/// </summary>
		/// <returns>The response.</returns>
		/// <param name="request">Request.</param>
		public static byte[] SendResponse (HttpListenerContext request)
		{
			string requestUri = request.Request.Url.ToString ();
			string[] requestPieces = requestUri.Split ('/');
			string handleableAction;

			getInstance ().Log ("Got a request!" + requestUri + "pieces:" + requestPieces [requestPieces.Length - 1]);
			Console.WriteLine ("request: " + requestUri);
			foreach (string s in request.Request.AcceptTypes) {
				Console.WriteLine ("types accepted: " + s);
			}
			for (int i = 0; i < requestPieces.Length; i++) {
				if (argumentHandleable (requestPieces[i])) {
					handleableAction = requestPieces [i];
					getInstance ().Log ("Handling the following action: " + handleableAction);
					if (!isLastInArguments(requestPieces, requestPieces[i])) {
						switch (handleableAction) {
							case "assetbundles":
							if (requestUri.LastIndexOf ('/') == requestUri.Length - 1) {
								getInstance().getAssetListAndReturnToHttpClient (request);
							}
							MemoryStream ms = new MemoryStream ();
							try {
								using (FileStream fileStream = File.OpenRead(AssetRoot + Path.DirectorySeparatorChar + "AssetBundles" + Path.DirectorySeparatorChar + requestPieces [i + 1]))
								{
									ms.SetLength(fileStream.Length);
									fileStream.Read(ms.GetBuffer(), 0, (int)fileStream.Length);
								}
								getInstance ().sendFileWithContentType (request, "application/octet-stream", ms.ToArray ());

							} catch (Exception e){
								getInstance ().sendTextResponse (request, "Unknown file: " + requestPieces [i + 1]);
								Console.WriteLine (e.GetBaseException());
							}
							break;

						}
						//We want a specific resource
					} else {
						//This means we want a list or favicon.ico
						switch (handleableAction) {
							case "rooms":
							getInstance ().sendStandardResponse (request, Encoding.UTF8.GetBytes (getInstance ().serializeAllRooms ()));
							break;
							case "favicon.ico":
							MemoryStream ms = null;
							try {
								using (FileStream fileStream = File.OpenRead(AssetRoot + Path.DirectorySeparatorChar + "AssetBundles/favicon.ico"))
								{
									ms.SetLength(fileStream.Length);
									fileStream.Read(ms.GetBuffer(), 0, (int)fileStream.Length);
								}
								//System.Drawing.Image img = System.Drawing.Image.FromFile ("AssetBundles/favicon.ico");
								//img.Save (ms, System.Drawing.Imaging.ImageFormat.Gif);
							} catch {
							}
							getInstance ().sendFileWithContentType (request, "image/vnd.microsoft.icon", ms.ToArray ());
							break;
							case "assetbundles":
							getInstance().getAssetListAndReturnToHttpClient (request);
							break;
							case "login":
							//Console.WriteLine ("DOUBLE DIP: " + request.Request.HttpMethod);
							//getInstance().getAssetListAndReturnToHttpClient (request);
							Dictionary<string,string> wop = getInstance().getPostDataFromRequest (request);
							if (wop.ContainsKey ("username") && wop.ContainsKey ("password")) {
									string username = null, password = null;
									wop.TryGetValue ("username", out username);
									wop.TryGetValue ("password", out password);
								User u = getInstance ().TryLogin (username, password);
								if (u != null) {
																		string s = getInstance ().sessions.CreateVokeySession (u);
									getInstance ().sendTextResponse (request, "HIER IS JE SESSIE, BITCH: " + s);
								} else {
									getInstance ().sendTextResponse (request, "JE LOGIN IS INVALIDE");
								}
							}
							getInstance ().sendTextResponse (request, "JIJ BENT INVALIDE");

							break;
												case "logout":
														Dictionary<string,string> wop2 = getInstance ().getPostDataFromRequest (request);
														if (wop2.ContainsKey ("session")) {
																string sessionHash = null;
																wop2.TryGetValue ("session", out sessionHash);
																VokeySession v = getInstance ().sessions.getSession (sessionHash);
																//v.InvalidateSession ();
														}


														break;
						}
					}
				}
			}
			request.Response.OutputStream.Close ();
			/*

			if (requestUri.Contains (".html") || requestUri [requestUri.Length - 1] == '/') {
				getInstance ().handleFileRequest ();
			} else {
				switch (requestPieces [requestPieces.Length - 1]) {

				
				default:
                       */ /*if (Guid.TryParseExact(requestPieces[requestPieces.Length - 1], ConfigurationManager.AppSettings["GuidFormatForWebService"], out g))
                        {
                              
                        }*//*
			getInstance().sendStandardResponse (request, Encoding.UTF8.GetBytes (string.Format ("<HTML><BODY>File not found or invalid GUID specified.<br></BODY></HTML>")));
			break;
		}

	}*/

		return new byte[] { 0x20 };
}
}
}
