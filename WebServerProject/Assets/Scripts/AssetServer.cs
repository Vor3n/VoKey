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
using System.Collections.Specialized;
using System.Linq;
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
            POST
,
            GET
,
            DELETE
,
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

            Town t = new Town("Lazytown", "TIV4A");
            t.addUser(new User("Felix", "Felix", "Felix Mann", User.UserType.Student));
            t.addUser(new User("KimJongUn", "cd1001", "Kim Jong Un", User.UserType.Student));
            t.addStreet(new Street("Nijenoord", Street.StreetType.Residential));
            t.addStreet(new Street("Rubenslaan", Street.StreetType.Residential));
            t.addStreet(s);
            TownList.Add(t);

            Town t1 = new Town("BossTown", "TIV4B");
            t1.addUser(new User("LeovM", "leooo", "Leo van Moergestel", User.UserType.Teacher));
            t1.addUser(new User("MartenWensink", "mw", "Marten Wensink", User.UserType.Teacher));
            t1.addUser(new User("GeraldOvink", "unityiszocool", User.UserType.Teacher));
            t1.addUser(new User("Daniel", "Plopjes", User.UserType.Teacher));
            t1.addUser(new User("LonelyIsland", "LikeABaws", User.UserType.Student));
            t1.addUser(new User("student", "student", User.UserType.Student));
            t1.addUser(new User("teacher", "teacher", User.UserType.Teacher));
            t1.addStreet(new Street("Flevolaan", Street.StreetType.Residential));
            t1.addStreet(new Street("Baksteenlaan", Street.StreetType.Residential));
            t1.addStreet(s);
            TownList.Add(t1);

            User pascal = new User("pascal", "pascal", User.UserType.Teacher);
            Users.Add(pascal);

            Users.Add(new User("dylan", "dylan", User.UserType.Teacher));

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

        public House getHouseByHouseGuid(Guid houseId)
        {
            foreach (Town t in TownList)
            {
                foreach (Street s in t.getShoppingStreets()) foreach (House h in s.houses) if (h.id == houseId) return h;
                foreach (Street s in t.educationalStreets) foreach (House h in s.houses) if (h.id == houseId) return h;
            }
            return null;
        }

        public VokeySession getSession(string hash)
        {
            return sessions.getSession(hash);
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

        public void handleComplexRequest(HttpListenerContext hlc, Dictionary<string, string> formData, VokeySession vs)
        {
            string requestUri = hlc.Request.Url.ToString();
            string handleableAction = getFirstHandlableAction(hlc);
            getInstance().Log("Handleable Action: " + handleableAction);
            switch (handleableAction)
            {
                case "assetbundles":
                    switch (hlc.Request.HttpMethod)
                    {
                        case "PUT":
                            StreamReader reader = new StreamReader(hlc.Request.InputStream);
                            String inputContent = reader.ReadToEnd();
                            UnityEngine.Debug.Log("Got input data to put. " + inputContent);

                            if (vs != null && vs.isValid)
                            {
                                UnityEngine.Debug.Log("Session is valid! " + vs.SessionHash);
                            }
                            else
                            {
                                UnityEngine.Debug.Log("Session is invalid :(! " + vs.SessionHash);
                            }
                            //check if we are teacher
                            //check if it exist
                            //store
                            //return id
                            break;
                        case "GET":
                            if (requestUri.EndsWith("/") || requestUri.EndsWith("list") || requestUri.EndsWith("list/"))
                            {
                                hlc.returnXmlStringToClient(getInstance().assetBundles.ToXml());
                            }
                            else if (requestUri.EndsWith("/file"))
                            {
                                MemoryStream ms = new MemoryStream();
                                try
                                {
                                    using (FileStream fileStream = File.OpenRead(AssetRoot + Path.DirectorySeparatorChar + "AssetBundles" + Path.DirectorySeparatorChar + getInstance().splitArrayFromHandlableAction(requestUri)[1]))
                                    {
                                        ms.SetLength(fileStream.Length);
                                        fileStream.Read(ms.GetBuffer(), 0, (int)fileStream.Length);
                                    }
                                    HttpFunctions.sendFileWithContentType(hlc, "application/octet-stream", ms.ToArray());

                                }
                                catch (Exception e)
                                {
                                    HttpFunctions.sendTextResponse(hlc, "Unknown file: " + splitArrayFromHandlableAction(requestUri)[1]);
                                    Console.WriteLine(e.GetBaseException());
                                }
                            }
                            else if (requestUri.EndsWith("/xml"))
                            {
                                UnityEngine.Debug.Log("Return XML to Client");
                            }
                            break;
                    }
                    break;
                case "house":
                    switch (hlc.Request.HttpMethod)
                    {
                        case "PUT":
                            //check whether the user putting the house is the owner
                            //put house
                            //return ok
                            break;
                        case "GET":
                            if (requestUri.EndsWith("/") || requestUri.EndsWith("list") || requestUri.EndsWith("list/"))
                            {
                                //IF STREET PROVIDED RETURN ALL HOUSES IN THE STREET
                                //IF NO STREET PROVIDED RETURN ALL HOUSES IN TOWN
                            }
                            else
                            {
                                House h = null;
                                try
                                {
                                    h = getInstance().getHouseByHouseGuid(new Guid(splitArrayFromHandlableAction(requestUri)[1]));
                                }
                                catch { }
                                if (h != null) HttpFunctions.returnXmlStringToHttpClient(hlc, h.ToXml());
                                else HttpFunctions.sendStandardResponse(hlc, "GUID NOT FOUND", 404);
                            }
                            break;
                    }

                    break;
                case "user":
                    switch (hlc.Request.HttpMethod)
                    {
                        case "PUT":
                            //check whether the user putting the house is the owner
                            //put house
                            //return ok
                            break;
                        case "GET":
                            string requestString = splitArrayFromHandlableAction(requestUri)[1];
                            User u = null;
                            try
                            {
                                if (requestString.Length == 36)
                                {
                                    u = getInstance().getUser(new Guid(requestString));
                                }
                                else
                                {
                                    u = getInstance().getUser(requestString);
                                }
                            }
                            catch { }

                            if (u != null) HttpFunctions.returnXmlStringToHttpClient(hlc, u.ToXml());
                            else HttpFunctions.sendStandardResponse(hlc, "GUID OR USERNAME NOT FOUND", 404);
                            break;
                    }

                    break;
                case "town":
                    switch (hlc.Request.HttpMethod)
                    {
                        case "PUT":
                            //check whether the user putting the house is the owner
                            //put house
                            //return ok
                            break;
                        case "GET":
                            Town t = null;
                            try
                            {
                                t = getInstance().getTown(new Guid(splitArrayFromHandlableAction(requestUri)[1]));
                            }
                            catch { }
                            if (t != null) HttpFunctions.returnXmlStringToHttpClient(hlc, t.ToXml());
                            else HttpFunctions.sendStandardResponse(hlc, "TOWN NOT FOUND", 404);
                            break;
                    }

                    break;
                case "street":
                    switch (hlc.Request.HttpMethod)
                    {
                        case "PUT":
                            //check whether the user putting the house is the owner
                            //put house
                            //return ok
                            break;
                        case "GET":
                            //lookup street in collection
                            break;
                        case "POST":
                            //update street in collection
                            break;
                    }
                    break;
                default:
                    HttpFunctions.sendTextResponse(hlc, "Resource not found. Welcome to reality.<br />Available actions are:<br /><ul><li><a href=\"town\">View towns</a></li><li><a href=\"user\">View users</a></li></ul>", 404);
                    break;
            }

            //We want a specific resource
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
            UnityEngine.Debug.Log("Split into " + requestPieces.Length + " pieces.");
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