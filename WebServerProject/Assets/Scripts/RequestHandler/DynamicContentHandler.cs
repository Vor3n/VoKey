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
using Thisiswhytheinternetexists.WebCore.Entities.Body.TwitterBootstrap;
using Thisiswhytheinternetexists.WebCore.Entities.Body.Forms;

public class DynamicContentHandler : RequestHandler
{
	private static string[] acceptableCommands = { "dynamic" };
	private string levelPrefix = "";
	private Thisiswhytheinternetexists.WebCore.HttpResponsePage currentPage;
	
	public DynamicContentHandler (HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
	{
		currentPage = new Thisiswhytheinternetexists.WebCore.HttpResponsePage ("Hallo Wereld.", true);
	}

	
    
	public override void handleSimpleRequest (string action)
	{
		//Debug.Log (context.Request.UserAgent);

		currentPage.AddElement (new CssLinkElement ("Welloe.css"));
		Table t = new Table ();
		t.ElementClass = "table table-bordered table-hover";
		t.addRow (new TableRow ("tableheader", new TableCell ("Name"), new TableCell ("Username"), new TableCell ("Awesomeness"))); 
		t.addRow (new TableRow (new TableCell ("Adolf"), new TableCell ("Jantje"), new TableCell ("Nein"))); 
		t.addRow (new TableRow (new TableCell ("Felix"), new TableCell ("Mann"), new TableCell ("Ja"))); 
		t.addRow (new TableRow (new TableCell ("Pascal"), new TableCell ("Schotman"), new TableCell ("Ja"))); 
		t.addRow (new TableRow (new TableCell ("Dylan"), new TableCell ("Snel"), new TableCell ("Ja"))); 
		t.addRow (new TableRow (new TableCell ("Duncan"), new TableCell ("Jenkins"), new TableCell ("Ja"))); 
		t.addRow (new TableRow (new TableCell ("Roy"), new TableCell ("Scheefhals"), new TableCell ("Ja"))); 
		t.addRow (new TableRow (new TableCell ("Bak"), new TableCell ("Steen"), new TableCell ("Ja")));
		currentPage.AddElement (t);
		currentPage.AddElement (new ImageElement ("Blue_morpho_butterfly.jpg", 807, 730, "A Butterfly"));
		HttpFunctions.sendStandardResponse (context, currentPage.getHtmlRepresentation (), 200);
	}
	
	public override void handleComplexRequest (string action)
	{
		string[] arguments = splitArrayFromHandlableAction (context.Request.Url.ToString ());
		if (arguments.Length <= 1)
			throw new Exception ("Not enough arguments to handle the complex request");
		switch (arguments [1]) {
			
		case "towns":
			//currentPage.AddElement (new Javascriptlet (""));
			currentPage.AddElement (new CssLinkElement ("Welloe.css"));
			
			currentPage.AddElement (new Javascriptlet ("var sendCloseWindowCommand = function(){ Unity.invoke('SwitchCommand','CloseWindow'); }"));
			Hyperlink closeButton = new Hyperlink ("#", "sendCloseWindowCommand()", "Close Window");
			closeButton.ElementClass = "largeUiButton";
			currentPage.AddElement (closeButton);
			
			
			currentPage.AddElement(new FieldSet("Towns"));
			Table townsTable = getTownsTable();
			DivElement left = new DivElement (townsTable);
			left.Style = "width: 500px; overflow: hidden;";
			currentPage.AddElement (left);
			currentPage.AddElement(new LinkButton ("Add New Town", "../addtown/"));
			
			HttpFunctions.sendStandardResponse (context, currentPage.getHtmlRepresentation (), 200);
			break;
			
		case "addtown":
			if (Content.Length == 0) {
				Form addtownform = new Form ("../addtown/", "POST", 
	               	new FieldSet ("Add Town", 
					new DivElement ("controls docs-input-sizes", new DivElement ("controls docs-input-sizes", new Input ("townname", InputType.Text, "Town Name")),
	              	new DivElement ("controls docs-input-sizes", new Input ("classname", InputType.Text, "Class Name")),
	               	new DivElement ("form-actions", new LinkButton ("Back", "../towns/"), new Input ("Add", InputType.Submit)))));
				currentPage.AddElement (addtownform);
			}
			else
			{
					string[] towntoadd = Content.Split('&');
					string townname = towntoadd[0];
					string classname = towntoadd[1];
					string[] tnsplit = townname.Split('=');
					string[] cnsplit = townname.Split('=');
					Town t = new Town(cnsplit[1], cnsplit[1]);
					AssetServer.getInstance().TownList.Add(t);
					currentPage.AddElement(new FieldSet("Town added succesfully!"));
					currentPage.AddElement(new LinkButton ("Okay", "../towns/"));
				
			}
			
			HttpFunctions.sendStandardResponse (context, currentPage.getHtmlRepresentation (), 200);
			break;
		
		case "removetown":
			currentPage.AddElement(new FieldSet("Succesfully removed town!"));
			Town towntoremove = AssetServer.getInstance().getTown(new Guid(arguments[2]));
			AssetServer.getInstance().TownList.Remove(towntoremove);
			currentPage.AddElement(new LinkButton ("Okay", "../towns/") );
			
			HttpFunctions.sendStandardResponse (context, currentPage.getHtmlRepresentation (), 200);
			break;
			
		case "edittown":
			Town town = null;
			try {
				town = AssetServer.getInstance ().getTown (new Guid (arguments [2]));
			} catch {
			}
			if (town != null) {
				int numberOfArgs = splitArrayFromHandlableAction (context.Request.Url.ToString ()).Length;
				for (int i = 0; i <= numberOfArgs; i++) {
					levelPrefix += "../";
				}
				currentPage.AddElement (new CssLinkElement ("Welloe.css"));
				currentPage.AddElement (new Javascriptlet ("var townGuid = \"" + town.id.ToString () + "\""));
				
					
				Table usersTable = getUserTable (town.pupils);
				List<House> eduRooms = new List<House> ();
				foreach (Street s in town.educationalStreets) {
					foreach (House h in s.houses) {
						eduRooms.Add (h);
					}
				}
				Table eduRoomsTable = getEduRoomTable (eduRooms);
						
				DivElement leftDiv = new DivElement (usersTable);
				leftDiv.Style = "width: 450px; float:left;";
				DivElement rightDiv = new DivElement (eduRoomsTable);
				rightDiv.Style = "width: 450px; float:left;";
				DivElement containerDiv = new DivElement (leftDiv, rightDiv);
				containerDiv.Style = "width: 900px; overflow: hidden;";
				currentPage.AddElement (containerDiv);
				currentPage.AddElement(new LinkButton("Back","../towns/"));
				currentPage.AddElement (new LinkButton ("Add User", "../adduser/" + town.id.ToString ()));
				currentPage.AddElement (new LinkButton ("Add Shop", "../addshop/" + town.id.ToString ()));
				HttpFunctions.sendStandardResponse (context, currentPage.getHtmlRepresentation (), 200);
				
			} else {
				string myList = "<ul>";
				string listEnd = "</ul>";
				foreach (Town t in AssetServer.getInstance().TownList) {
					myList += "<li><a href=\"" + t.id + "\">" + t.classroomName + "</a></li>";
				}
				throw new Exception ("No town specified to edit! <br />" + myList + listEnd);        
			}
			break;
			

			
		
			
		case "adduser":
				Town towntje = null;
				try {
					towntje = AssetServer.getInstance ().getTown (new Guid (arguments [2]));
				} catch {
					throw new Exception ("No town specified to add the user to! <br />");
				}
			if (Content.Length == 0) {
				Form f = new Form ("../adduser/"+towntje.id.ToString(), "POST", 
               		new FieldSet ("Add User", new DivElement ("controls docs-input-sizes", new Input ("fullname", InputType.Text, "Full Name")),
               		new DivElement ("controls docs-input-sizes", new Input ("username", InputType.Text, "Username")),
              		new DivElement ("controls docs-input-sizes", new Input ("password", InputType.Password, "Password")),
               		new DivElement ("form-actions", new JavascriptButton ("Back", "window.location = document.referrer"), new Input ("Add", InputType.Submit))));
				currentPage.AddElement (f);
				HttpFunctions.sendStandardResponse (context, currentPage.getHtmlRepresentation (), 200);
			} else {
				if (towntje != null) {
					//if (IsFormRequest) {
						//Dictionary<string, string> formData = GetFormDataFromRequest (context.Request);
						// Keys & Values uit formData halen
					//}
					
					string[] user = Content.Split('&');
					string username = user[1];
					string[] splituser = username.Split('=');
					string password = user[2];
					string[] splitpw = password.Split('=');
					string fullname = user[0];
					fullname = fullname.Replace('+',' ');
					string[] splitfn = fullname.Split('=');
					try
					{
						towntje.addUser(new VokeyUser(splituser[1], splitpw[1], splitfn[1], VokeyUser.VokeyUserType.Student));
					}
					catch
					{
						throw new Exception("Could the username already exist maybe?");
					}
					currentPage.AddElement(new FieldSet("User added succesfully!"));
					currentPage.AddElement (new LinkButton ("Okay", "../edittown/" + towntje.id.ToString ()));
					HttpFunctions.sendStandardResponse (context, currentPage.getHtmlRepresentation (), 200);
				}
			}
			break;
			
		case "removeuser":
			foreach(Town t in AssetServer.getInstance().TownList)
			{
				try
				{
					t.removeUser(new Guid(arguments[2]));
					currentPage.AddElement(new FieldSet("User Removed Succesfully!"));
				}
				catch{}
			}
			
			currentPage.AddElement(new JavascriptButton ("Okay", "window.location = document.referrer") );
			HttpFunctions.sendStandardResponse (context, currentPage.getHtmlRepresentation (), 200);
			break;
			
		case "edituser":
			VokeyUser u = AssetServer.getInstance ().getUser (new Guid (arguments [2]));
			if (Content.Length == 0)
			{
				currentPage.AddElement (new CssLinkElement ("Welloe.css"));
				Input usernameinput = new Input("username", InputType.Text, u.username);
				usernameinput.Value = u.username;
				Input fullnameinput = new Input("", InputType.Text, u.FullName);
				fullnameinput.Value = u.FullName;
				Form edituserform = new Form ("../edituser/"+u.userGuid.ToString(), "POST", 
	               	new FieldSet ("Edit User", 
					new DivElement ("controls docs-input-sizes", 
					new DivElement ("controls docs-input-sizes", fullnameinput), 
					new DivElement ("controls docs-input-sizes", usernameinput),
					new DivElement(new Input("Edit", InputType.Submit)))));
				currentPage.AddElement (edituserform);
				
				Table usertable = new Table ();
				usertable.ElementClass = "table table-bordered table-hover";
				DivElement studentleft = new DivElement (usertable);
				studentleft.Style = "width: 225px; overflow: hidden;";			
				TableHeaderCell thc = new TableHeaderCell ("STATS");
				thc.Colspan = 2;
				thc.Style = "text-align:center;";
				usertable.addRow (new TableRow ("tableheader", thc)); 
				usertable.addRow (new TableRow (new TableCell ("Assignments done:"), new TableCell ((u.assignments == null) ? "0" : "" + u.assignments.CompletedAssignments.Count)));
				usertable.addRow (new TableRow (new TableCell ("Assignments left:"), new TableCell ((u.assignments == null) ? "0" : "" + u.assignments.TodoAssignments.Count)));
				currentPage.AddElement (studentleft);
				
				
				currentPage.AddElement(new JavascriptButton ("Back", "window.location = document.referrer"));
			}
			else
			{
				string[] user = Content.Split('&');
				string username = user[1];
				string[] splituser = username.Split('=');
				string fullname = user[0];
				fullname = fullname.Replace('+',' ');
				string[] splitfn = fullname.Split('=');
				u.username = splituser[1];
				u.FullName = splitfn[1];
				currentPage.AddElement(new FieldSet("Updated User Succesfully!"));
				foreach(Town t in AssetServer.getInstance().TownList)
				{
					if (t.ContainsUser(u.userGuid))
					{
						currentPage.AddElement(new LinkButton("Okay","../edittown/"+t.id.ToString()));	
					}
				}
				
			}
			
			HttpFunctions.sendStandardResponse (context, currentPage.getHtmlRepresentation (), 200);
			break;
			
		default:
			throw new Exception("Your argument is invalid.");
			break;
		}
		
	}
	/// <summary>
	/// Returns a table of users in the provided town.
	/// </summary>
	/// <returns>The user table.</returns>
	private Table getUserTable (List<VokeyUser> userList)
	{
		currentPage.AddElement (new Javascriptlet ("var sendEditUserCommand = function(uguid){ Unity.invoke('SwitchCommand','EditUser',uguid); }"));
		currentPage.AddElement (new Javascriptlet ("var sendDeleteUserCommand = function(uguid, tguid){ Unity.invoke('SwitchCommand','DeleteUser',tguid, uguid); }"));
						
		Table usersTable = new Table ();
		usersTable.ElementClass = "table table-bordered table-hover";
		TableHeaderCell tc = new TableHeaderCell ("Action");
		usersTable.addRow (new TableRow ("tableheader", new TableHeaderCell ("Name"), new TableHeaderCell ("Username"), tc/*, new TableCell("Assignments to do"), new TableCell("Assignments done")*/)); 
		foreach (User user in userList) {
			usersTable.addRow (
				new TableRow (
					new TableCell (user.FullName), 
					new TableCell (user.username), 
					new TableCell (new LinkButton ("Edit", "../edituser/"+user.userGuid), new LinkButton ("Remove", "../removeuser/"+user.userGuid))));
		}
		
		return usersTable;
	}
	
	private Table getTownsTable()
	{
		Table townTable = new Table();
		townTable.ElementClass = "table table-bordered table-hover";
		townTable.addRow(new TableRow("tableheader", new TableHeaderCell("Town Name"), new TableHeaderCell("Class Name"), new TableHeaderCell("Action")));
		foreach(Town t in AssetServer.getInstance().TownList)
		{
			townTable.addRow(
				new TableRow(
					new TableCell(t.name),
					new TableCell(t.classroomName),
					new TableCell(new LinkButton ("Edit", "../edittown/" + t.id.ToString ()), new LinkButton("Remove","../removetown/"+ t.id.ToString ()))));
		}
		return townTable;
	}
		
	
	private ImageElement getImageIconElement (string prefix, string filename, string alt)
	{
		return new ImageElement (filename, 16, 16, alt);
	}
	
	private Table getEduRoomTable (List<House> houses)
	{
		currentPage.AddElement (new Javascriptlet ("var sendEditRoomCommand = function(rguid, tguid){ Unity.invoke('SwitchCommand','EditRoom',rguid, tguid); }"));
		currentPage.AddElement (new Javascriptlet ("var sendDeleteRoomCommand = function(tguid, rguid){ Unity.invoke('SwitchCommand','DeleteUser',tguid, rguid); }"));
		
		Table result = new Table ();
		result.ElementClass = "table table-bordered table-hover";
		TableHeaderCell a1 = new TableHeaderCell ("House name");
		TableHeaderCell a2 = new TableHeaderCell ("Room name");
		TableHeaderCell a3 = new TableHeaderCell ("Action");
		result.addRow (new TableRow ("tableheader", a1, a2, a3));
		currentPage.AddElement (new Javascriptlet ("var sendCloseWindowCommand = function(){ Unity.invoke('SwitchCommand','CloseWindow'); }"));
		foreach (House h in houses) {
			foreach (Room r in h.rooms) {
				if (!r.name.Equals(h.description))
				{
				result.addRow (new TableRow (
					new TableCell (h.description), 
					new TableCell (r.name),
					new TableCell (
						//new Hyperlink ("#","sendEditRoomCommand('" + r.id + "', townGuid)", "Edit"),
						new JavascriptButton("Edit", "sendEditRoomCommand('" + r.id + "', townGuid)"),
						new LinkButton("Remove", "../removeshop/"+r.id.ToString()))));
				}
			}
		}
		return result;
	}

}