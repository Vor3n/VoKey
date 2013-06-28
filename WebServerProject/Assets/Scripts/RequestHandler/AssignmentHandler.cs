using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using VokeySharedEntities;
using Vokey;
using GuiTest;

public class AssignmentHandler : RequestHandler {

    private static string[] acceptableCommands = { "assignment" };

    public AssignmentHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {
        
    }

    public override void handleSimpleRequest(string command)
    {
	    User studentUser = new User("student", "student", User.UserType.Student);
	    studentUser.addAssignment(new Assignment("Talk to to corporate", "LIKE A BOSS", new System.Guid("a966726a-eda0-4b40-b127-c3fc435fd98b"), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("Approve Memos", "LIKE A BOSS", new System.Guid("a966726a-eda0-4b40-b127-c3fc435fd98b"), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("Lead a workshop", "LIKE A BOSS", new System.Guid("a966726a-eda0-4b40-b127-c3fc435fd98b"), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("Remember birthdays", "LIKE A BOSS", new System.Guid("a966726a-eda0-4b40-b127-c3fc435fd98b"), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("Direct Workflow", "LIKE A BOSS", new System.Guid("a966726a-eda0-4b40-b127-c3fc435fd98b"), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("My own bathroom", "LIKE A BOSS", new System.Guid("a966726a-eda0-4b40-b127-c3fc435fd98b"), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("Micromanage", "LIKE A BOSS", new System.Guid("a966726a-eda0-4b40-b127-c3fc435fd98b"), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("Promote synergy", "Bake ALL THE CAKE. NOW.", new System.Guid("a966726a-eda0-4b40-b127-c3fc435fd98b"), new List<System.Guid>()));
	    
        HttpFunctions.returnXmlStringToHttpClient(context, studentUser.assignments.ToXml());
    }
}
