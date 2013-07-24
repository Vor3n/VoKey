using System.Collections;
using System.Net;
using Vokey;
using VokeySharedEntities;
using AssemblyCSharp;
using System;
using GuiTest;
using Thisiswhytheinternetexists.WebCore;

public class TownHandler : RequestHandler {
        private static string[] acceptableCommands = { "town" };

        public TownHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {
        
    }

    public override void handleSimpleRequest(string command)
    {
        AssetServer instance = AssetServer.getInstance();
        if (session != null)
        {
            if (session.IsStudent)
            {
                Town t = instance.getTown(session.User.townGuid);
                HttpFunctions.returnXmlStringToHttpClient(context, t.ToXml());
            }
            else
            {
                HttpFunctions.returnXmlStringToHttpClient(context, instance.TownList.ToXml());
            }
        }
        else
        {
            HttpFunctions.returnXmlStringToHttpClient(context, instance.TownList.ToXml());
        }
    }
    
    public override void handleComplexRequest (string command)
		{
				AssetServer instance = AssetServer.getInstance ();
				Town t = null;
				string[] arguments = splitArrayFromHandlableAction (context.Request.Url.ToString());
				try {
						t = instance.getTown (new Guid(arguments[1]));
				} catch {
				}
				if (t != null) {
						if (arguments.Length > 2) {
								switch (arguments [2]) {
								case "street":
									StreetHandler s = new StreetHandler (context);
									s.setTown (t);
									s.handleRequest ();
									break;
								case "room":
									throw new Exception ("NOT IMPLEMENTED");
								case "user":
									if (arguments.Length >= 4) {
                                        VokeyUser sentUser = MySerializerOfItems.FromXml<VokeyUser>(Content);
										switch (arguments [3]) {
											case "create":
													t.addUser (sentUser);
													HttpFunctions.sendStandardResponse(context, "OK", 200);
													break;
											case "delete":
													t.removeUser (sentUser.userGuid);
													HttpFunctions.sendStandardResponse(context, "OK", 200);
													break;
											case "update":
													t.removeUser (sentUser.userGuid);
													HttpFunctions.sendStandardResponse(context, "OK", 200);
													t.addUser (sentUser);
													break;
											default:
												throw new Exception("Cannot handle action: " + arguments[3]);
										}
									} else {
										throw new Exception("Not enough parameters");
									}
									break;
								}
						} else {
								HttpFunctions.returnXmlStringToHttpClient (context, t.ToXml ());
						}
				
				} else {
						switch (arguments [1]) {
						case "create":
								if (session != null && session.IsTeacher && session.isValid) {
										t = MySerializerOfItems.FromXml<Town> (Content);
										UnityEngine.Debug.Log(Content);
										if (instance.townClassNameExists (t.classroomName) == Guid.Empty) {
												instance.TownList.Add (t);
												HttpFunctions.sendStandardResponse (context, "OK", 200);
												return;
										} else {
											HttpFunctions.sendStandardResponse (context, "DUPLICATE_CLASS_NAME", 403);
											return;
										}

								} else {
										HttpFunctions.sendStandardResponse (context, "YOU HAVE NO RIGHTS TO CREATE A TOWN", 403);
										return;
								}
						case "update":
								if (session != null && session.IsTeacher && session.isValid) {
										t = MySerializerOfItems.FromXml<Town> (Content);
										if (instance.getTown (t.id) != null) {
											Town townToUpdate = instance.getTown(instance.townClassNameExists (t.classroomName));
											instance.TownList.Remove (townToUpdate);
											instance.TownList.Add (t);
											UnityEngine.Debug.Log ("Updates Town: " + t.name);
											HttpFunctions.sendStandardResponse (context, "OK", 200);
											return;
										} else {
											HttpFunctions.sendStandardResponse (context, "TOWN_DOES_NOT_EXIST", 403);
											return;
										}

								} else {
										HttpFunctions.sendStandardResponse (context, "YOU HAVE NO RIGHTS TO UPDATE A TOWN", 403);
										return;
								}
						case "delete":
								if (session != null && session.IsTeacher && session.isValid) {
										t = instance.getTown (new Guid(arguments[2]));
										if (t != null) {
												instance.Delete<Town> (new Guid(arguments[2]));
												HttpFunctions.sendStandardResponse(context, "Town " + arguments[2] + " removed.", 200);
												return;
										}
								} else {
									HttpFunctions.sendStandardResponse(context, "YOU HAVE NO RIGHTS TO DELETE A TOWN", 403);
									return;
								}
								break;
								case "count":
									HttpFunctions.sendStandardResponse(context, "" + instance.TownList.Count, 200);
								break;
						default:
							throw new Exception("Parameter not understood: " + arguments[1]);
						
				} 
		
		}
    }
}
