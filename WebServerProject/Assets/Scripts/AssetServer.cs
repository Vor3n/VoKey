using System;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using GuiTest;
using System.Xml.Serialization;
using VokeySharedEntities;
using AssemblyCSharp;

namespace Vokey
{
    [System.Serializable]
    public class AssetServer
    {
        public bool Running
        {
            get
            {
                if (ws == null)
                    return false;
                return ws.Running;
            }
        }

        public static string AssetRoot = "";

        public Dictionary<string, Type> handlers;

        private static AssetServer myInstance;

        public static AssetServer getInstance()
        {
            if (myInstance == null)
                myInstance = new AssetServer();
            return myInstance;
        }

        public enum HttpRequestType
        {
            POST,
            GET,
            DELETE,
            PUT
        }

        private WebServer ws;
        private List<Town> _townList;
        /// <summary>
        /// Occurs a message is logged.
        /// </summary>
        public event Action<string> LogMessage;

        public List<Town> TownList
        {
            get
            {
                if (_townList == null)
                    _townList = new List<Town>();
                return _townList;
            }
        }

        public List<VokeyAssetBundle> AssetBundles
        {
            get
            {
                if (assetBundles == null)
                    assetBundles = new List<VokeyAssetBundle>();
                return assetBundles;
            }
        }

        [XmlArray("Users")]
        [XmlArrayItem("User")]
        public List<User> Users
        {
            get;
            set;
        }

        /// <summary>
        /// Log the specified message.
        /// </summary>
        /// <param name='message'>
        /// Message.
        /// </param>
        private void Log(string message)
        {
            if (LogMessage != null)
            {
                LogMessage(DateTime.Now.ToLongTimeString() + ": " + message);
            }
        }

        private List<VokeyAssetBundle> assetBundles;
        private BackgroundWorker scanForAssetsWorker;
        private VokeySessionContainer sessions;

        public AssetServer()
        {
            myInstance = this;
            AssetRoot = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(AssetServer)).Location) + System.IO.Path.DirectorySeparatorChar + ".." + System.IO.Path.DirectorySeparatorChar + "..";
            handlers = new Dictionary<string, Type>();

            addHandlersForType(new SessionHandler(null));
            addHandlersForType(new AssetBundleHandler(null));
            addHandlersForType(new TownHandler(null));
            addHandlersForType(new WelcomeHandler(null));
            addHandlersForType(new StreetHandler(null));
            addHandlersForType(new RoomHandler(null));
            addHandlersForType(new FileHandler(null));
            assetBundles = new List<VokeyAssetBundle>();
            Users = new List<User>();
            sessions = new VokeySessionContainer();
            scanForAssetsWorker = new BackgroundWorker();
            scanForAssetsWorker.DoWork += ScanForAssetsWorkerWork;
            readUserData();
            scanForAssets();

        }

        /// <summary>
        /// Adds the actions that the provided RequestHandler can add to the list of Handlable Requests.
        /// </summary>
        /// <param name="rh"></param>
        public void addHandlersForType(RequestHandler rh)
        {
            var type = rh.GetType();
            foreach (string s in rh.handlableCommands)
            {
                handlers.Add(s, type);
            }
        }

        /// <summary>
        /// Creates a RequestHandler
        /// </summary>
        /// <param name="theType">The type of RequestHandler to create</param>
        /// <param name="context">The Request to handle</param>
        /// <returns></returns>
        public RequestHandler createHandler(Type theType, HttpListenerContext context)
        {
            return (RequestHandler)Activator.CreateInstance(theType, context);
        }

        /// <summary>
        /// Makes the AssetServer Scan for Assets (this is a event handler)
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void ScanForAssetsWorkerWork(object sender, DoWorkEventArgs e)
        {
            scanForAssets();
        }

        void readUserData()
        {
            Street s = new Street("Winkelstraat", Street.StreetType.Educational);
            s.addHouse(new House("Baka", "Store to buy bakas"));

            Town t = new Town("Lazytown", "TIV4A");
            t.addStreet(new Street("Nijenoord", Street.StreetType.Residential));
            t.addStreet(new Street("Rubenslaan", Street.StreetType.Residential));
            t.addStreet(s);

            t.addUser(new User("Felix", "Felix", "Felix Mann", User.UserType.Student));
            t.addUser(new User("KimJongUn", "cd1001", "Kim Jong Un", User.UserType.Student));

            TownList.Add(t);

            Town t1 = new Town("BossTown", "TIV4B");
            t1.addStreet(new Street("Flevolaan", Street.StreetType.Residential));
            t1.addStreet(new Street("Baksteenlaan", Street.StreetType.Residential));
            t1.addStreet(s);

            t1.addUser(new User("LeovM", "leooo", "Leo van Moergestel", User.UserType.Student));
            t1.addUser(new User("MartenWensink", "mw", "Marten Wensink", User.UserType.Student));
            t1.addUser(new User("GeraldOvink", "unityiszocool", User.UserType.Student));
            t1.addUser(new User("dylan", "dylan", "Dylan Snel", User.UserType.Student));
            t1.addUser(new User("duncan", "duncan", "Duncan Jenkins", User.UserType.Student));
            t1.addUser(new User("Daniel", "Plopjes", User.UserType.Student));
            t1.addUser(new User("LonelyIsland", "LikeABaws", User.UserType.Student));
            User studentUser = new User("student", "student", User.UserType.Student);
            studentUser.addAssignment(new Assignment("Demo Assignment", "Find all the objects in the room.", Guid.NewGuid(), new List<Guid>()));
            t1.addUser(studentUser);
            
            t1.addUser(new User("alex", "student", "Alexander Streng", User.UserType.Student));
            t1.addUser(new User("rscheefh", "student", "Roy Scheefhals", User.UserType.Student));
            t1.addUser(new User("aniek", "student", "Aniek Zandleven", User.UserType.Student));

            TownList.Add(t1);

            Users.Add(new User("teacher", "teacher", User.UserType.Teacher));
            Users.Add(new User("pascal", "pascal", User.UserType.Teacher));

            //Room r = new Room("Duncan's Living Room");
            //Room r2 = new Room("Chillroom");
            //Room r3 = new Room("Pascal's werkkamer");
        }
        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="u">The user t add.</param>
        void addUser(User u)
        {
            Users.Add(u);
        }
        /// <summary>
        /// Start this AssetServer.
        /// </summary>
        public void Start()
        {
            ws = new WebServer(SendResponse, "http://+:9090/");
            ws.LogMessage += HandleWsLogMessage;
            ws.Run();
        }
        /// <summary>
        /// Stop this instance of the WebServer.
        /// </summary>
        public void Stop()
        {
            ws.Stop();
        }
        /// <summary>
        /// Handles the ws log message.
        /// </summary>
        /// <param name="obj">Object.</param>
        void HandleWsLogMessage(string obj)
        {
            Log(obj);
        }
        /// <summary>
        /// Scans for assets
        /// </summary>
        public void scanForAssets()
        {

            int numBefore = assetBundles.Count;
            Log("location to scan: " + AssetRoot);
            string[] xmlBundleMetaFiles = Directory.GetFiles(AssetRoot + System.IO.Path.DirectorySeparatorChar + "AssetBundles", "vab*.xml");
            foreach (string s in xmlBundleMetaFiles)
            {
                VokeyAssetBundle vob = VokeyAssetBundle.FromXml(s);
                if (!assetBundles.Contains(vob))
                    assetBundles.Add(vob);
            }
            Log("Number of assets in the bundle after scan: " + assetBundles.Count + "before was: " + numBefore);
        }


        public Town getTown(Guid id)
        {
            foreach (Town t in TownList) if (t.id == id) return t;
            return null;
        }
        
        public Guid townClassNameExists (string townClassName)
		{
		    foreach (Town t in TownList)
            {
				if(t.classroomName == townClassName) return t.id;
            }
            return Guid.Empty;
		}

        public User getUser(Guid id)
        {
            foreach (Town t in TownList)
            {
                foreach (User u in t.pupils)
                {
                    if (u.userGuid == id)
                        return u;
                }
            }

            foreach (User u in Users)
            {
                if (u.userGuid == id) return u;
            }
            return null;
        }

        public Street getStreet(Guid id)
        {
            foreach (Town t in TownList)
            {
                foreach (Street sim in t.getFilledStreets())
                {
                    if (sim.id == id)
                    {
                        return sim;
                    }
                }
            }
            throw new Exception("Invalid street specified!");
        }

        public User getUser(string username)
        {
            foreach (Town t in TownList)
            {
                foreach (User u in t.pupils) if (u.username == username) return u;
            }
            foreach (User u in Users)
            {
                if (u.username == username) return u;
            }
            return null;
        }

        public VokeySession getSession(string hash)
        {
            return sessions.getSession(hash);
        }
   
		public void Delete<T> (Guid g)
		{
			switch (typeof(T).ToString ()) {
				case "Town":
				case "VokeySharedEntities.Town":
					TownList.Remove (getTown(g));
				break;
			default:
				throw new Exception("Deleting " + typeof(T).ToString () + " is not yet supported.");
			}
		}
		
        public string CreateVokeySession(User u)
        {
            return sessions.CreateVokeySession(u);
        }

        /// <summary>
        /// Returns whether the argument handleable.
        /// </summary>
        /// <returns><c>true</c>, if argument was handable , <c>false</c> otherwise.</returns>
        /// <param name="argument">Argument.</param>
        public static bool argumentHandleable(string argument)
        {
            return getInstance().handlers.ContainsKey(argument);
        }

        public static bool isLastInArguments(string[] arguments, string item)
        {
            int length = arguments.Length;
            int count = 1;
            foreach (string s in arguments)
            {
                if (s == item)
                    return (count == length);
                count++;
            }
            Console.WriteLine("SMACK SOMEONE WITH AN ERASER. THERE IS SOMETHING VERY WRONG.");
            return false;
        }

        public string getFirstHandlableAction(HttpListenerContext hlc)
        {
            if (splitArrayFromHandlableAction(hlc.Request.Url.ToString()).Length > 0)
                return splitArrayFromHandlableAction(hlc.Request.Url.ToString())[0];
            else return "welcome";
        }

        public string[] splitArrayFromHandlableAction(string arguments)
        {
            string[] requestPieces = arguments.Split('/');
            List<string> actionPieces = new List<string>();
            int begin = 0;
            for (; begin < requestPieces.Length; begin++)
                if (argumentHandleable(requestPieces[begin]))
                    break;
            for (; begin < requestPieces.Length; begin++)
                actionPieces.Add(requestPieces[begin]);
            return actionPieces.ToArray();
        }
        /// <summary>
        /// Tries the login.
        /// </summary>
        /// <returns><c>true</c>, if login was correct, <c>false</c> otherwise.</returns>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public static User TryLogin(string username, string password)
        {
            foreach (Town t in AssetServer.getInstance().TownList)
            {
                if (t.ContainsUser(username))
                {
                    UnityEngine.Debug.Log("Username found in town: " + t.name);
                    User u = t.getUser(username);
                    if (u.PasswordHash == EncryptionUtilities.GenerateSaltedSHA1(password))
                    {
                        UnityEngine.Debug.Log("Password match");
                        return u;
                    }
                }
            }

            foreach (User u in AssetServer.getInstance().Users)
            {
                if (u.username == username)
                {
                    UnityEngine.Debug.Log("Username match");
                    if (u.PasswordHash == EncryptionUtilities.GenerateSaltedSHA1(password))
                    {
                        UnityEngine.Debug.Log("Password match");
                        return u;
                    }
                    else
                    {
                        UnityEngine.Debug.Log(u.PasswordHash);
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
        public static byte[] SendResponse(HttpListenerContext request)
        {
            AssetServer instance = getInstance();
            try
            {
                String firstHandlableAction = instance.getFirstHandlableAction(request);
                if (instance.handlers.ContainsKey(firstHandlableAction))
                {
                    Type typeOfHandler = null;
                    instance.handlers.TryGetValue(firstHandlableAction, out typeOfHandler);
                    RequestHandler r = instance.createHandler(typeOfHandler, request);
                    r.handleRequest();
                }
                else
                {
                    throw new Exception("No handler defined for the action: " + firstHandlableAction);
                }
            }
            catch (Exception e)
            {
                HttpFunctions.handleServerException(request, e);
            }

            request.Response.OutputStream.Close();

            return new byte[] { 0x20 };
        }

    }
}