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
								Table t = new Table ();
								t.addRow (new TableRow("tableheader", new TableCell("Key"), new TableCell("Value"))); 
								t.addRow (new TableRow(new TableCell(new Hyperlink("#", "Unity.ShowPupil()","Full Name")), new TableCell(u.FullName))); 
								t.addRow (new TableRow(new TableCell("Username"), new TableCell(u.username)));
								t.addRow (new TableRow(new TableCell("Assignments done:"), new TableCell("" + u.assignments.CompletedAssignments.Count)));
								t.addRow (new TableRow(new TableCell("Assignments left:"), new TableCell("" + u.assignments.TodoAssignments.Count)));
							
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
								WebCore.HttpResponsePage hrp = new WebCore.HttpResponsePage ("Hallo Wereld.");
								hrp.AddElementToHead (new CssLinkElement("../../file/Welloe.css"));
								Hyperlink backButton = new Hyperlink("#", "Unity.invoke('SwitchCommand', 'ClassView')", "Back");
								backButton.ElementClass = "largeUiButton";
								Hyperlink editButton = new Hyperlink("#", "Unity.invoke('SwitchCommand', 'ClassView')", "Edit");
								editButton.ElementClass = "largeUiButton";
								Hyperlink deleteButton = new Hyperlink("#", "Unity.invoke('SwitchCommand', 'ClassView')", "Delete");
								deleteButton.ElementClass = "largeUiButton";
								hrp.AddElementToBody(backButton);
								hrp.AddElementToBody(editButton);
								hrp.AddElementToBody(deleteButton);
								//hrp.AddElementToBody(new FormButton("BACK", "Unity.invoke('SwitchCommand', 'ClassView')"));
								Table t = new Table ();
								t.addRow (new TableRow(new TableCell("Name"), new TableCell("Username")/*, new TableCell("Assignments to do"), new TableCell("Assignments done")*/)); 
								foreach (User user in town.pupils) {
									t.addRow (new TableRow(new TableCell(user.FullName), new TableCell(new Hyperlink("#", "Unity.ShowPupil('" + user.userGuid + "')", user.username))));
								}
							hrp.AddElementToBody(t);
							HttpFunctions.sendStandardResponse(context, hrp.getHtmlRepresentation(), 200);
						}
					break;
		}
	}

}