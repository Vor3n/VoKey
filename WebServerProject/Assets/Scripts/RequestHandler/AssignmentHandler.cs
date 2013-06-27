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
	    studentUser.addAssignment(new Assignment("Talk to to corporate", "LIKE A BOSS", System.Guid.NewGuid(), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("Approve Memos", "LIKE A BOSS", System.Guid.NewGuid(), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("Lead a workshop", "LIKE A BOSS", System.Guid.NewGuid(), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("Remember birthdays", "LIKE A BOSS", System.Guid.NewGuid(), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("Direct Workflow", "LIKE A BOSS", System.Guid.NewGuid(), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("My own bathroom", "LIKE A BOSS", System.Guid.NewGuid(), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("Micromanage", "LIKE A BOSS", System.Guid.NewGuid(), new List<System.Guid>()));
	    studentUser.addAssignment(new Assignment("Promote synergy", "Bake ALL THE CAKE. NOW.", System.Guid.NewGuid(), new List<System.Guid>()));
	    
        HttpFunctions.returnXmlStringToHttpClient(context, studentUser.assignments.ToXml());
    }
}
