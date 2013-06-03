using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;

namespace Vokey
{
	[System.Serializable]
    public class AssetServer
	{
		private static AssetServer myInstance;

		public static AssetServer getInstance ()
		{
			if (myInstance == null)
				myInstance = new AssetServer ();
			return myInstance;
		}

		private WebServer ws;
		private ObservableCollection<Room> _roomList;
		
		/// <summary>
		/// Occurs a message is logged.
		/// </summary>
		public event Action<string> LogMessage;

		public ObservableCollection<Room> RoomList {
			get {
				if (_roomList == null)
					_roomList = new ObservableCollection<Room> ();
				return _roomList;
			}
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
		
		private ObservableCollection<VokeyAssetBundle> assetBundles;
		private BackgroundWorker scanForAssetsWorker;

		public AssetServer ()
		{
			myInstance = this;
			assetBundles = new ObservableCollection<VokeyAssetBundle> ();
			scanForAssetsWorker = new BackgroundWorker ();
			scanForAssetsWorker.DoWork += ScanForAssetsWorkerWork;
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
			string fullPath = System.Reflection.Assembly.GetAssembly (typeof(AssetServer)).Location;
			int numBefore = assetBundles.Count;
			string theDirectory = Path.GetDirectoryName (fullPath);
			Log ("location to scan: " + theDirectory);
			string[] xmlBundleMetaFiles = Directory.GetFiles (theDirectory + System.IO.Path.DirectorySeparatorChar + "AssetBundles", "vab*.xml");
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
			request.Response.ContentLength64 = content.Length;
			request.Response.OutputStream.Write (content, 0, content.Length);
		}

		/// <summary>
		/// Sends a file and uses the provided content type.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="contentType">Content type.</param>
		/// <param name="data">Data.</param>
		public void sendFileWithContentType(HttpListenerContext request, string contentType, byte[] data){
			request.Response.ContentType = contentType;
			request.Response.ContentLength64 = data.Length;
			request.Response.OutputStream.Write (data, 0, data.Length);
		}

		/// <summary>
		/// Handles a file request.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="filename">Filename.</param>
		public void handleFileRequest(HttpListenerContext request, string filename){
			getInstance ().sendStandardResponse (request, Encoding.UTF8.GetBytes (string.Format ("<HTML><BODY>Vokey Server<br>" + DateTime.Now.ToShortTimeString () + " {0}</BODY></HTML>", DateTime.Now)));
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
			if (argument == "rooms" || argument == "assetbundles" || argument == "houses" || argument == "file" || argument == "favicon.ico"){
				return true;
			}
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
			throw new Exception ("Item not in array");
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
		/// Sends the response.
		/// </summary>
		/// <returns>The response.</returns>
		/// <param name="request">Request.</param>
		public static byte[] SendResponse (HttpListenerContext request)
		{
			string requestUri = request.Request.Url.ToString ();
			string[] requestPieces = requestUri.Split ('/');
			string handleableAction;
			string handleableActionParameter;

			getInstance ().Log ("Got a request!" + requestUri + "pieces:" + requestPieces [requestPieces.Length - 1]);

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
						byte[] output;
						MemoryStream ms = new MemoryStream ();
							try {
								using (FileStream fileStream = File.OpenRead("AssetBundles" + Path.DirectorySeparatorChar + requestPieces [i + 1]))
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
							byte[] output;
							MemoryStream ms = null;
							try {
								ms = new MemoryStream ();
								System.Drawing.Image img = System.Drawing.Image.FromFile ("AssetBundles/favicon.ico");
								img.Save (ms, System.Drawing.Imaging.ImageFormat.Gif);
							} catch {
							}
							getInstance ().sendFileWithContentType (request, "image/vnd.microsoft.icon", ms.ToArray ());
							break;
						case "assetbundles":
							getInstance().getAssetListAndReturnToHttpClient (request);
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
