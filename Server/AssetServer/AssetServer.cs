using AssetServer.Entities;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;
using System.Text;

namespace AssetServer
{
    public class AssetServer
    {
        private static AssetServer myInstance;
        public static AssetServer getInstance()
        {
            if (myInstance == null) myInstance = new AssetServer();
            return myInstance;
        }
        private WebServer ws;
        private ObservableCollection<Room> roomList;

        public ObservableCollection<Room> RoomList
        {
            get
            {
                if(roomList == null) roomList = new ObservableCollection<Room>();
                return roomList;
            }
        }


        public AssetServer()
        {
            myInstance = this;
        }
        

        public void Start()
        {
            ws = new WebServer(SendResponse, "http://+:8080/");
            ws.Run();
        }
        /// <summary>
        /// Scans for assets
        /// </summary>
        public void scanForAssets()
        {
            
        }

        public string serializeAllRooms()
        {
            string s = "";
            foreach (Room r in RoomList)
            {
                s += r.Serialize(r).InnerXml.ToString() + "\n";
            }
            return s;
        }

        public static byte[] SendResponse(HttpListenerRequest request)
        {
            string requestUri = request.Url.ToString();
            string[] requestPieces = requestUri.Split('/');
            Console.WriteLine("Got a request!" + requestUri);

            if (requestUri.Contains(".html") || requestUri[requestUri.Length - 1] == '/')
            {
                return Encoding.UTF8.GetBytes(string.Format("<HTML><BODY>My web page.<br>" + DateTime.Now.ToShortTimeString() + " {0}</BODY></HTML>", DateTime.Now));
            }
            else
            {
                switch (requestPieces[requestPieces.Length - 1])
                {
                    case "rooms":
                        return Encoding.UTF8.GetBytes(getInstance().serializeAllRooms());
                    default:
                        Guid g;
                        if (Guid.TryParseExact(requestPieces[requestPieces.Length - 1], ConfigurationManager.AppSettings["GuidFormatForWebService"], out g))
                        {
                              
                        }
                        return Encoding.UTF8.GetBytes(string.Format("<HTML><BODY>File not found or invalid GUID specified.<br></BODY></HTML>"));
                }
               
            }

            
        }
    }
}
