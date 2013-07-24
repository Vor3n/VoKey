using System;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;
using AssemblyCSharp;
using Thisiswhytheinternetexists.WebCore;
using GuiTest;
using VokeySharedEntities;

namespace Vokey
{
    [System.Serializable]
    public class AssetServer : WebCoreWebServer
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

        private List<Town> _townList;

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
            get { if (assetBundles == null)
                assetBundles = new List<VokeyAssetBundle>();
                return assetBundles;
            }
        }

        [XmlArray("Users")]
        [XmlArrayItem("User")]
        public List<User> AssetServerUsers
        {
            get;
            set;
        }
        
        public List<Street> EducationalStreets;

        /// <summary>
        /// Log the specified message.
        /// </summary>
        /// <param name='message'>
        /// Message.
        /// </param>
        private void Log(string message)
        {

        }

        private List<VokeyAssetBundle> assetBundles;
        private BackgroundWorker scanForAssetsWorker;
        private VokeySessionContainer mySessions;

        public AssetServer()
            : base("http://+:9090/")
        {
            myInstance = this;
            AssetRoot = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(AssetServer)).Location) + System.IO.Path.DirectorySeparatorChar + ".." + System.IO.Path.DirectorySeparatorChar + "..";
            //addHandlersForType(new SessionHandler(null));
            addHandlersForType(new AssetBundleHandler(null));
            addHandlersForType(new TownHandler(null));
            //addHandlersForType(new WelcomeHandler(null));
            addHandlersForType(new StreetHandler(null));
            addHandlersForType(new RoomHandler(null));
            addHandlersForType (new HouseHandler(null));
            //addHandlersForType(new FileHandler(null));
            addHandlersForType (new AssignmentHandler(null));
            addHandlersForType(new DynamicContentHandler(null));
            assetBundles = new List<VokeyAssetBundle>();
            AssetServerUsers = new List<User>();
            mySessions = new VokeySessionContainer();
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
        /// Makes the AssetServer Scan for Assets (this is a event handler)
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void ScanForAssetsWorkerWork(object sender, DoWorkEventArgs e)
        {
            scanForAssets();
        }

        void readUserData ()
				{
        	
			Street s = new Street("Winkelstraat", Street.StreetType.Educational);

            Town t = new Town("Lazytown", "TIV4A");
            t.addStreet(new Street("Nijenoord", Street.StreetType.Residential));
            t.addStreet(new Street("Rubenslaan", Street.StreetType.Residential));
            t.addStreet(s);

            t.addUser(new VokeyUser("Felix", "Felix", "Felix Mann", VokeyUser.VokeyUserType.Student));
            t.addUser(new VokeyUser("KimJongUn", "cd1001", "Kim Jong Un", VokeyUser.VokeyUserType.Student));

            TownList.Add(t);

            Town t1 = new Town("BossTown", "TIV4B");
            t1.addStreet(new Street("Flevolaan", Street.StreetType.Residential));
            t1.addStreet(new Street("Baksteenlaan", Street.StreetType.Residential));
            t1.addStreet(s);

            t1.addUser(new VokeyUser("LeovM", "leooo", "Leo van Moergestel", VokeyUser.VokeyUserType.Student));
            t1.addUser(new VokeyUser("MartenWensink", "mw", "Marten Wensink", VokeyUser.VokeyUserType.Student));
            t1.addUser(new VokeyUser("GeraldOvink", "unityiszocool", VokeyUser.VokeyUserType.Student));
            t1.addUser(new VokeyUser("dylan", "dylan", "Dylan Snel", VokeyUser.VokeyUserType.Student));
            t1.addUser(new VokeyUser("duncan", "duncan", "Duncan Jenkins", VokeyUser.VokeyUserType.Student));
            t1.addUser(new VokeyUser("Daniel", "Plopjes", VokeyUser.VokeyUserType.Student));
            t1.addUser(new VokeyUser("LonelyIsland", "LikeABaws", VokeyUser.VokeyUserType.Student));
            VokeyUser studentUser = new VokeyUser("student", "student", VokeyUser.VokeyUserType.Student);
            studentUser.addAssignment(new Assignment("Demo Assignment", "Find all the objects in the room.", Guid.NewGuid(), new List<Guid>()));
            t1.addUser(studentUser);
            
            t1.addUser(new VokeyUser("alex", "student", "Alexander Streng", VokeyUser.VokeyUserType.Student));
            t1.addUser(new VokeyUser("rscheefh", "student", "Roy Scheefhals", VokeyUser.VokeyUserType.Student));
            t1.addUser(new VokeyUser("aniek", "student", "Aniek Zandleven", VokeyUser.VokeyUserType.Student));

            TownList.Add(t1);

            Users.Add(new VokeyUser("teacher", "teacher", VokeyUser.VokeyUserType.Teacher));
            Users.Add(new VokeyUser("pascal", "pascal", VokeyUser.VokeyUserType.Teacher));
						/*StreamReader townReader = new StreamReader (AssetRoot + Path.DirectorySeparatorChar + "AssetBundles" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + "towns.xml");
						_townList = MySerializerOfLists.FromXml<Town> (townReader.ReadToEnd());
						townReader.Close ();
        	
						StreamReader userReader = new StreamReader (AssetRoot + Path.DirectorySeparatorChar + "AssetBundles" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + "users.xml");
						AssetServerUsers = MySerializerOfLists.FromXml<User> (userReader.ReadToEnd());
						userReader.Close ();
        	
						StreamReader eduReader = new StreamReader (AssetRoot + Path.DirectorySeparatorChar + "AssetBundles" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + "eduStreets.xml");
						EducationalStreets = MySerializerOfLists.FromXml<Street> (eduReader.ReadToEnd());
						eduReader.Close ();*/
        				
        				/*foreach(Street s in EducationalStreets){
							foreach (Town t in _townList) {
								t.addStreet (s);
							}
							}*/
        }
        
        public void writeUserData ()
		{
			StreamWriter townWriter = new StreamWriter (AssetRoot + Path.DirectorySeparatorChar + "AssetBundles" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + "towns.xml", false);
			townWriter.Write (TownList.ToXml());
			townWriter.Flush ();
			townWriter.Close ();
			
			StreamWriter userWriter = new StreamWriter (AssetRoot + Path.DirectorySeparatorChar + "AssetBundles" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + "users.xml", false);
			userWriter.Write (AssetServerUsers.ToXml());
			userWriter.Flush ();
			userWriter.Close ();
			
			StreamWriter eduWriter = new StreamWriter (AssetRoot + Path.DirectorySeparatorChar + "AssetBundles" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + "eduStreets.xml", false);
			eduWriter.Write (EducationalStreets.ToXml());
			eduWriter.Flush ();
			eduWriter.Close ();
		}

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="u">The user t add.</param>
        void addUser(User u)
        {
            AssetServerUsers.Add(u);
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
        public void scanForAssets ()
				{
					try {
					int numBefore = assetBundles.Count;
			            Log("location to scan: " + AssetRoot);
			            string[] xmlBundleMetaFiles = Directory.GetFiles(AssetRoot + System.IO.Path.DirectorySeparatorChar + "AssetBundles", "vab*.xml");
			            foreach (string s in xmlBundleMetaFiles)
			            {
			                VokeyAssetBundle vob = MySerializerOfItems.FromXml<VokeyAssetBundle>(new StreamReader(s).ReadToEnd ());
			                if (!assetBundles.Contains(vob))
			                    assetBundles.Add(vob);
			            }
            			Log("Number of assets in the bundle after scan: " + assetBundles.Count + "before was: " + numBefore);
					} catch (Exception e) {
						UnityEngine.Debug.Log(e.GetBaseException());
					}
					
            
        }


        public Town getTown(Guid id)
        {
            foreach (Town t in TownList) if (t.id == id) return t;
            return null;
        }
        
        public VokeyAssetBundle getVokeyAssetBundle(Guid id)
        {
            foreach (VokeyAssetBundle vab in AssetBundles) if (vab.modelId == id) return vab;
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

        public VokeyUser getUser(Guid id)
        {
            foreach (Town t in TownList)
            {
                foreach (VokeyUser u in t.pupils)
                {
                    if (u.userGuid == id)
                        return u;
                }
            }

            foreach (VokeyUser u in AssetServerUsers)
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

        public VokeyUser getUser(string username)
        {
            foreach (Town t in TownList)
            {
                foreach (VokeyUser u in t.pupils) if (u.username == username) return u;
            }
            foreach (VokeyUser u in AssetServerUsers)
            {
                if (u.username == username) return u;
            }
            return null;
        }

        public VokeySession getVokeySession(string hash)
        {
            return mySessions.getSession(hash);
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
		
		/// <summary>
		/// Creates a vokey session.
		/// </summary>
		/// <returns>The vokey session.</returns>
		/// <param name="u">U.</param>
        public string CreateVokeySession(VokeyUser u)
        {
            return mySessions.CreateVokeySession(u);
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


        /// <summary>
        /// Tries the login.
        /// </summary>
        /// <returns><c>true</c>, if login was correct, <c>false</c> otherwise.</returns>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public static VokeyUser TryVokeyLogin(string username, string password)
        {
            foreach (Town t in AssetServer.getInstance().TownList)
            {
                if (t.ContainsUser(username))
                {
                    UnityEngine.Debug.Log("Username found in town: " + t.name);
                    VokeyUser u = t.getUser(username);
                    if (u.PasswordHash == EncryptionUtilities.GenerateSaltedSHA1(password))
                    {
                        UnityEngine.Debug.Log("Password match");
                        return u;
                    }
                }
            }

            foreach (VokeyUser u in AssetServer.getInstance().AssetServerUsers)
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


    }
}