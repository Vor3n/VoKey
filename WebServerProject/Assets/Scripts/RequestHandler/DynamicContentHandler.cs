using UnityEngine;
using System.Collections;
using System.Net;
using AssemblyCSharp;
using System;
using Vokey;
using VokeySharedEntities;
using System.Collections.Generic;
using Thisiswhytheinternetexists.WebCore.Entities;
using Thisiswhytheinternetexists.WebCore;
using GuiTest;

public class DynamicContentHandler : RequestHandler
{
    private static string[] acceptableCommands = { "dynamic" };
	private string levelPrefix = "";
	
    public DynamicContentHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {
		currentPage = new WebCore.HttpResponsePage("Hallo Wereld.", true);
    }
    
    private WebCore.HttpResponsePage currentPage;
    
    public override void handleSimpleRequest (string action)
	{
		Debug.Log (context.Request.UserAgent);

		
		currentPage.AddElement(new CssLinkElement("Welloe.css"));
		Table t = new Table();
        t.ElementClass = "table table-bordered table-hover";
		t.addRow (new TableRow("tableheader", new TableCell("Name"), new TableCell("Username"), new TableCell("Awesomeness"))); 
		t.addRow (new TableRow(new TableCell("Adolf"), new TableCell("Jantje"), new TableCell("Nein"))); 
		t.addRow (new TableRow(new TableCell("Felix"), new TableCell("Mann"), new TableCell("Ja"))); 
		t.addRow (new TableRow(new TableCell("Pascal"), new TableCell("Schotman"), new TableCell("Ja"))); 
		t.addRow (new TableRow(new TableCell("Dylan"), new TableCell("Snel"), new TableCell("Ja"))); 
		t.addRow (new TableRow(new TableCell("Duncan"), new TableCell("Jenkins"), new TableCell("Ja"))); 
		t.addRow (new TableRow(new TableCell("Roy"), new TableCell("Scheefhals"), new TableCell("Ja"))); 
		t.addRow (new TableRow(new TableCell("Bak"), new TableCell("Steen"), new TableCell("Ja")));
        currentPage.AddElement(t);
        currentPage.AddElement(new ImageElement("Blue_morpho_butterfly.jpg", 807, 730, "A Butterfly"));
		HttpFunctions.sendStandardResponse(context, currentPage.getHtmlRepresentation(), 200);
	}
	
	public override void handleComplexRequest (string action)
		{
				string[] arguments = splitArrayFromHandlableAction (context.Request.Url.ToString());
				if (arguments.Length <= 2)
						throw new Exception ("Not enough arguments to handle the complex request");
				switch (arguments [1]) {
				case "edituser":
						VokeyUser u = null;
						try {
								u = AssetServer.getInstance ().getUser (new Guid(arguments[2]));
						} catch {
						}
						if (u != null) {
                            currentPage.AddElement(new CssLinkElement("../../file/Welloe.css"));
                            currentPage.AddElement(new Javascriptlet("var sendShowTownCommand = function(){ Unity.invoke('SwitchCommand','ShowTown'); }"));
								Table t = new Table ();
								
								Hyperlink backButton = new Hyperlink ("#", "sendShowTownCommand()", "Back");
								backButton.ElementClass = "largeUiButton";
                                currentPage.AddElement(backButton);
								
								TableCell tc = new TableCell ("Edit User");
								tc.Colspan = 2;
								tc.Style = "text-align:center;";
								
								t.addRow (new TableRow("tableheader", tc)); 
								t.addRow (new TableRow(new TableCell("Full Name"), new TableCell(u.FullName))); 
								t.addRow (new TableRow(new TableCell("Username"), new TableCell(u.username)));
								t.addRow (new TableRow(new TableCell("Assignments done:"), new TableCell((u.assignments == null) ? "0" : "" + u.assignments.CompletedAssignments.Count)));
								t.addRow (new TableRow(new TableCell("Assignments left:"), new TableCell((u.assignments == null) ? "0" : "" + u.assignments.TodoAssignments.Count)));
							
								currentPage.AddElement (t);
								HttpFunctions.sendStandardResponse (context, currentPage.getHtmlRepresentation (), 200);
						}
						break;
				case "edittown":
						Town town = null;
						try {
								town = AssetServer.getInstance ().getTown (new Guid(arguments[2]));
						} catch {
						}
						if (town != null) {
							int numberOfArgs = splitArrayFromHandlableAction (context.Request.Url.ToString()).Length;
							for (int i = 0; i <= numberOfArgs; i++) {
									levelPrefix += "../";
							}
							currentPage.AddElement (new CssLinkElement(levelPrefix + "file/Welloe.css"));
							currentPage.AddElement (new Javascriptlet("var townGuid = \"" + town.id.ToString() + "\""));
							currentPage.AddElement (new Javascriptlet("var sendCloseWindowCommand = function(){ Unity.invoke('SwitchCommand','CloseWindow'); }"));
					
					
					
							Hyperlink closeButton = new Hyperlink ("#", "sendCloseWindowCommand()", "Back");
							closeButton.ElementClass = "largeUiButton";
					
							ImageElement editUserImageElement = new ImageElement (levelPrefix + "file/user_edit.png", 16, 16, "Edit User");
							ImageElement deleteUserImageElement = new ImageElement (levelPrefix + "file/user_delete.png", 16, 16, "Remove User");
					
							currentPage.AddElement (closeButton);
					
							Table usersTable = getUserTable (town.pupils);
							List<House> eduRooms = new List<House> ();
							foreach (Street s in town.educationalStreets) {
								foreach (House h in s.houses) {
									eduRooms.Add (h);
								}
							}
							Table eduRoomsTable = getEduRoomTable(eduRooms);
						
						DivElement leftDiv = new DivElement(usersTable);
						leftDiv.Style = "width: 250px; float:left;";
						DivElement rightDiv = new DivElement(eduRoomsTable);
						//rightDiv.Style = "width: 250px; float:left;";
						DivElement containerDiv = new DivElement(leftDiv, rightDiv);
						containerDiv.Style = "width: 800px; overflow: hidden;";
						currentPage.AddElement (containerDiv);
						HttpFunctions.sendStandardResponse (context, currentPage.getHtmlRepresentation (), 200);
					} else {
			            string myList = "<ul>";
			            string listEnd = "</ul>";
			            foreach (Town t in AssetServer.getInstance().TownList)
			            {
			                myList += "<li><a href=\"" + t.id + "\">" + t.classroomName + "</a></li>";
			            }
			            throw new Exception("No town specified to edit! <br />" + myList + listEnd);        
					}
				break;
		}
	}
	/// <summary>
	/// Returns a table of users in the provided town.
	/// </summary>
	/// <returns>The user table.</returns>
	private Table getUserTable(List<VokeyUser> userList){
		currentPage.AddElement(new Javascriptlet("var sendEditUserCommand = function(uguid){ Unity.invoke('SwitchCommand','EditUser',uguid); }"));
		currentPage.AddElement(new Javascriptlet("var sendDeleteUserCommand = function(uguid, tguid){ Unity.invoke('SwitchCommand','DeleteUser',tguid, uguid); }"));
						
		Table usersTable = new Table ();
		TableCell tc = new TableCell("");
		usersTable.addRow (new TableRow("tableheader", new TableCell("Name"), new TableCell("Username"), tc/*, new TableCell("Assignments to do"), new TableCell("Assignments done")*/)); 
		foreach (User user in userList) {
			usersTable.addRow (new TableRow(
			new TableCell(user.FullName), 
			new TableCell(user.username), 
			new TableCell(
				new Hyperlink("#", "sendEditUserCommand('" + user.userGuid + "')", getImageIconElement(levelPrefix, "user_edit.png", "Edit User")),
				new Hyperlink("#", "sendDeleteUserCommand('" + user.userGuid + "', townGuid)", getImageIconElement(levelPrefix, "user_delete.png", "Delete User"))
			)));
		}
		
		return usersTable;
	}
	
	private ImageElement getImageIconElement (string prefix, string filename, string alt)
	{
		return new ImageElement (prefix + "file/" + filename, 16, 16, alt);
	}
	
	private Table getEduRoomTable (List<House> houses)
		{
		currentPage.AddElement(new Javascriptlet("var sendEditRoomCommand = function(rguid, tguid){ Unity.invoke('SwitchCommand','EditRoom',rguid, tguid); }"));
		currentPage.AddElement(new Javascriptlet("var sendDeleteRoomCommand = function(tguid, rguid){ Unity.invoke('SwitchCommand','DeleteUser',tguid, rguid); }"));
		
				Table result = new Table ();
				TableCell a1 = new TableCell ("House name");
				TableCell a2 = new TableCell ("Room name");
				TableCell a3 = new TableCell ("");
				result.addRow (new TableRow("tableheader", a1, a2, a3));
				foreach (House h in houses) {
					foreach (Room r in h.rooms) {
						result.addRow (new TableRow(new TableCell(h.name), new TableCell(r.name), new TableCell(new Hyperlink("#", "sendEditRoomCommand('" + r.id + "', townGuid)", getImageIconElement(levelPrefix, "user_delete.png", "Edit Room")))));
					}
				}
		return result;
	}

}