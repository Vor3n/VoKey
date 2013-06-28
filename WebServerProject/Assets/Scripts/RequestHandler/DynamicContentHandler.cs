using UnityEngine;
using System.Collections;
using System.Net;
using WebCore.HtmlEntities;
using AssemblyCSharp;
using System;
using GuiTest;
using Vokey;
using VokeySharedEntities;

public class DynamicContentHandler : RequestHandler
{
    private static string[] acceptableCommands = { "dynamic" };

    public DynamicContentHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {
		
    }
    
    public override void handleSimpleRequest (string action)
	{
					Debug.Log (context.Request.UserAgent);

		WebCore.HttpResponsePage hrp = new WebCore.HttpResponsePage("Hallo Wereld.");
		hrp.AddElementToHead(new CssLinkElement("../file/Welloe.css"));
		Table t = new Table();
		t.addRow (new TableRow("tableheader", new TableCell("Name"), new TableCell("Username"), new TableCell("Awesomeness"))); 
		t.addRow (new TableRow(new TableCell("Adolf"), new TableCell("Jantje"), new TableCell("Nein"))); 
		t.addRow (new TableRow(new TableCell("Felix"), new TableCell("Mann"), new TableCell("Ja"))); 
		t.addRow (new TableRow(new TableCell("Pascal"), new TableCell("Schotman"), new TableCell("Ja"))); 
		t.addRow (new TableRow(new TableCell("Dylan"), new TableCell("Snel"), new TableCell("Ja"))); 
		t.addRow (new TableRow(new TableCell("Duncan"), new TableCell("Jenkins"), new TableCell("Ja"))); 
		t.addRow (new TableRow(new TableCell("Roy"), new TableCell("Scheefhals"), new TableCell("Ja"))); 
		t.addRow (new TableRow(new TableCell("Bak"), new TableCell("Steen"), new TableCell("Ja"))); 
		hrp.AddElementToBody(t);
		hrp.AddElementToBody (new ImageElement("../file/Blue_morpho_butterfly.jpg", 807, 730, "A Butterfly"));
		HttpFunctions.sendStandardResponse(context, hrp.getHtmlRepresentation(), 200);
	}
	
	public override void handleComplexRequest (string action)
		{
				string[] arguments = splitArrayFromHandlableAction (context.Request.Url.ToString());
				if (arguments.Length <= 2)
						throw new Exception ("Not enough arguments to handle the complex request");
				switch (arguments [1]) {
				case "edituser":
						User u = null;
						try {
								u = AssetServer.getInstance ().getUser (new Guid(arguments[2]));
						} catch {
						}
						if (u != null) {
								WebCore.HttpResponsePage hrp = new WebCore.HttpResponsePage ("Hallo Wereld.");
								hrp.AddElementToHead (new CssLinkElement("../../file/Welloe.css"));
								hrp.AddElementToHead(new Javascriptlet("var sendShowTownCommand = function(){ Unity.invoke('SwitchCommand','ShowTown'); }"));
								Table t = new Table ();
								
								Hyperlink backButton = new Hyperlink ("#", "sendShowTownCommand()", "Back");
								backButton.ElementClass = "largeUiButton";
								hrp.AddElementToBody (backButton);
								
								TableCell tc = new TableCell("Edit User");
								tc.Colspan = 2;
								tc.Style = "text-align:center;";
								
								t.addRow (new TableRow("tableheader", tc)); 
								t.addRow (new TableRow(new TableCell("Full Name"), new TableCell(u.FullName))); 
								t.addRow (new TableRow(new TableCell("Username"), new TableCell(u.username)));
								t.addRow (new TableRow(new TableCell("Assignments done:"), new TableCell((u.assignments == null) ? "0" : "" + u.assignments.CompletedAssignments.Count)));
								t.addRow (new TableRow(new TableCell("Assignments left:"), new TableCell((u.assignments == null) ? "0" : "" + u.assignments.TodoAssignments.Count)));
							
								hrp.AddElementToBody (t);
								HttpFunctions.sendStandardResponse (context, hrp.getHtmlRepresentation (), 200);
						}
						break;
				case "edittown":
						Town town = null;
						try {
								town = AssetServer.getInstance ().getTown (new Guid(arguments[2]));
						} catch {
						}
						if (town != null) {
							string levelPrefix = "";
							int numberOfArgs = splitArrayFromHandlableAction (context.Request.Url.ToString()).Length;
							for (int i = 0; i <= numberOfArgs; i++) {
								levelPrefix += "../";
							}
						WebCore.HttpResponsePage hrp = new WebCore.HttpResponsePage ("Hallo Wereld.");
						hrp.AddElementToHead (new CssLinkElement(levelPrefix + "file/Welloe.css"));
						hrp.AddElementToHead(new Javascriptlet("var townGuid = \"" + town.id.ToString() + "\""));
						hrp.AddElementToHead(new Javascriptlet("var sendEditUserCommand = function(uguid){ Unity.invoke('SwitchCommand','EditUser',uguid); }"));
						hrp.AddElementToHead(new Javascriptlet("var sendDeleteUserCommand = function(uguid, tguid){ Unity.invoke('SwitchCommand','DeleteUser',tguid, uguid); }"));
						hrp.AddElementToHead(new Javascriptlet("var sendCloseWindowCommand = function(){ Unity.invoke('SwitchCommand','CloseWindow'); }"));
						
						
						Hyperlink closeButton = new Hyperlink ("#", "sendCloseWindowCommand()", "Back");
						closeButton.ElementClass = "largeUiButton";
						
						ImageElement editUserImageElement = new ImageElement (levelPrefix + "file/user_edit.png", 16, 16, "Edit User");
						ImageElement deleteUserImageElement = new ImageElement (levelPrefix + "file/user_delete.png", 16, 16, "Remove User");
						
						hrp.AddElementToBody (closeButton);
						Table t = new Table ();
						t.Width = 600;
						TableCell tc = new TableCell("");
						tc.Width = 40;
						t.addRow (new TableRow("tableheader", new TableCell("Name"), new TableCell("Username"), tc/*, new TableCell("Assignments to do"), new TableCell("Assignments done")*/)); 
						foreach (User user in town.pupils) {
							t.addRow (new TableRow(
							new TableCell(user.FullName), 
							new TableCell(user.username), 
							new TableCell(
								new Hyperlink("#", "sendEditUserCommand('" + user.userGuid + "')", editUserImageElement),
								new Hyperlink("#", "sendDeleteUserCommand('" + user.userGuid + "', townGuid)", deleteUserImageElement)
							)));
						}
						hrp.AddElementToBody (t);
						HttpFunctions.sendStandardResponse (context, hrp.getHtmlRepresentation (), 200);
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

}