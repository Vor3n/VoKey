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
		
		public void sendStandardResponse (HttpListenerContext request, byte[] content)
		{
			request.Response.ContentLength64 = content.Length;
			request.Response.OutputStream.Write (content, 0, content.Length);
		}
		
		public static byte[] SendResponse (HttpListenerContext request)
		{
			string requestUri = request.Request.Url.ToString ();
			string[] requestPieces = requestUri.Split ('/');
			getInstance ().Log ("Got a request!" + requestUri + "pieces:" + requestPieces [requestPieces.Length - 1]);

			if (requestUri.Contains (".html") || requestUri [requestUri.Length - 1] == '/') {
				getInstance ().sendStandardResponse (request, Encoding.UTF8.GetBytes (string.Format ("<HTML><BODY>Vokey Server<br>" + DateTime.Now.ToShortTimeString () + " {0}</BODY></HTML>", DateTime.Now)));
			} else {
				switch (requestPieces [requestPieces.Length - 1]) {
				case "rooms":
					getInstance ().sendStandardResponse (request, Encoding.UTF8.GetBytes (getInstance ().serializeAllRooms ()));
					break;
				case "assetbundles":
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
					break;
				case "favicon.ico":
					byte[] output;
					MemoryStream ms = null;
					try {
						ms = new MemoryStream ();
						System.Drawing.Image i = System.Drawing.Image.FromFile ("AssetBundles/favicon.ico");
						i.Save (ms, System.Drawing.Imaging.ImageFormat.Gif);
					} catch { }
					output = ms.ToArray();
					request.Response.ContentType = "image/vnd.microsoft.icon";
					request.Response.ContentLength64 = output.Length;
					request.Response.OutputStream.Write (output, 0, output.Length);
					break;
				default:
                        /*if (Guid.TryParseExact(requestPieces[requestPieces.Length - 1], ConfigurationManager.AppSettings["GuidFormatForWebService"], out g))
                        {
                              
                        }*/
					getInstance().sendStandardResponse (request, Encoding.UTF8.GetBytes (string.Format ("<HTML><BODY>File not found or invalid GUID specified.<br></BODY></HTML>")));
					break;
				}
               
			}

            return new byte[] { 0x20 };
		}
	}
}
