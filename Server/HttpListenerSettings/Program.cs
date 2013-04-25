using System;
using System.Diagnostics;
using System.Security.Principal;

namespace HttpListenerSettings
{
    class Program
    {
        private static int port;
        private static string[] args;
        static void Main(string[] _args)
        {
            args = _args;
            Console.WriteLine("number of arguments: " + args.Length);
            if (args.Length == 1)
            {
                bool validNumber = int.TryParse(args[0], out port);
                if(!validNumber){
                    Console.WriteLine("Invalid port specified: " + args[0]);
                    printUsageAndExit();
                }
                if (IsElevated) setNetShHttpListeningPort(8080);
                else
                {
                    if (UserWantsElevation) Elevate();
                    else Environment.Exit(0);
                }
            }
            else if (args.Length == 2 && args[0] == "-d")
            {
                bool validNumber = int.TryParse(args[1], out port);
                if (!validNumber)
                {
                    Console.WriteLine("Invalid port specified: " + args[0]);
                    printUsageAndExit();
                }
                if (IsElevated) removeNetShHttpListeningPort(port);
                else
                {
                    if (UserWantsElevation) Elevate();
                    else Environment.Exit(0);
                }
                
            }
            else printUsageAndExit();
            
        }

        private static void printUsageAndExit()
        {
            Console.WriteLine("HttpListenerSettingsTool v1.0.0.1" + Environment.NewLine + 
                Environment.NewLine +
                "usage:                        " + Environment.NewLine +
                "      httplistenersettingstool [port]" + Environment.NewLine + Environment.NewLine + 
                "Where [port] is the port you want to open." + 
                Environment.NewLine + "Please note that this tool requires elevation!");
            Environment.Exit(0);
        }

        private static bool UserWantsElevation 
        {
            get
            {
                Console.Write("We are currently not elevated." + Environment.NewLine + "To run commands we will be needing elevation." + Environment.NewLine + "Press Y to allow or N to cancel. [Y/N]? ");
                char c = Console.ReadKey().KeyChar;
                return (c == 'y' || c == 'Y');
            }
        }

        private static void setNetShHttpListeningPort(int port)
        {
            string command = "http add urlacl url=http://+:" + port + "/ user=" + Environment.GetEnvironmentVariable("USERNAME");
            ProcessStartInfo p = new ProcessStartInfo("netsh", command);
            p.RedirectStandardOutput = true;
            p.UseShellExecute = false;
            p.CreateNoWindow = false;
            Process.Start(p);
            Console.WriteLine("Ran command: " + command);
        }

        private static void removeNetShHttpListeningPort(int port)
        {
            string command = "http delete urlacl url=http://+:" + port + "/";
            ProcessStartInfo p = new ProcessStartInfo("netsh", command);
            p.RedirectStandardOutput = true;
            p.UseShellExecute = false;
            p.CreateNoWindow = false;
            Process.Start(p);
            Console.WriteLine("Ran command: " + command);
        }

        private static bool IsElevated
        {
            get
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private static void Elevate()
        {
            string s;
            if (args.Length == 2)
            {
                s = args[0] + " " + args[1];
            }
            else if (args.Length == 1)
            {
                s = args[0];
            }
            else
            {
                throw new Exception("Invalid arguments supplied!");
            }
            var startInfo = new ProcessStartInfo(System.Environment.GetCommandLineArgs()[0], s) { Verb = "runas" };
            Process.Start(startInfo);
            Environment.Exit(0);
        }
    }
}
