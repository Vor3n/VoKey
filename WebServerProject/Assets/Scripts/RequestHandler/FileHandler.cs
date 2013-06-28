using UnityEngine;
using System.Collections;
using System.Net;
using System.Collections.Generic;
using System.IO;
using AssemblyCSharp;
using Vokey;

public class FileHandler : RequestHandler
{
    private static string[] acceptableCommands = { "file", "favicon.ico" };
    private static Dictionary<string, string> extensionMimeTypes;
    private static void fillDictionary()
    {
        if (extensionMimeTypes == null)
        {
            extensionMimeTypes = new Dictionary<string, string>();
            extensionMimeTypes.Add(".bin", "application/octet-stream");
            extensionMimeTypes.Add(".unity3d", "application/octet-stream");
            extensionMimeTypes.Add(".ico", "image/vnd.microsoft.icon");
            extensionMimeTypes.Add(".css", "text/css");
            extensionMimeTypes.Add(".jpg", "img/jpeg");
            extensionMimeTypes.Add(".png", "img/png");
            extensionMimeTypes.Add(".js", "text/javascript");
            extensionMimeTypes.Add(".html", "text/html");
        }
    }

    public FileHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {
        if (hlc == null) FileHandler.fillDictionary();
    }

	/// <summary>
	/// Returns the file to the client.
	/// </summary>
	/// <param name="theContext">The context.</param>
	/// <param name="filename">Filename of the file to return.</param>
	/// <param name="extension">Extension of the file to return.</param>
	public static void returnFile (HttpListenerContext theContext, string filename, string extension)
	{
		string mime = null;
		MemoryStream ms = new MemoryStream ();
		extensionMimeTypes.TryGetValue (extension, out mime);
		try {
			using (FileStream fileStream = File.OpenRead(AssetServer.AssetRoot + Path.DirectorySeparatorChar + "AssetBundles" + Path.DirectorySeparatorChar + filename)) {
				ms.SetLength (fileStream.Length);
				fileStream.Read (ms.GetBuffer(), 0, (int)fileStream.Length);
			}
			HttpFunctions.sendFileWithContentType (theContext, mime, ms.ToArray ());
		} catch {
			UnityEngine.Debug.Log (filename);
			HttpFunctions.sendStandardResponse(theContext, "FILE NOT FOUND", 404);
		}
	}

	public override void handleSimpleRequest (string action)
	{
		if (action == "favicon.ico") {
			returnFile(context, action, ".ico");
		}
	}

    public override void handleComplexRequest(string action)
    {
        string filename = splitArrayFromHandlableAction(context.Request.Url.ToString())[1];
        string extension = filename.Substring(filename.LastIndexOf('.'));
        if (extensionMimeTypes.ContainsKey(extension))
        {
            returnFile(context, filename, extension);
        }
        else
        {
            throw new System.Exception("File type not allowed!");
        }
    }
}