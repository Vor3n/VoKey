using UnityEngine;
using System.Collections;
using System.Net;
using System.Collections.Generic;
using System.IO;
using AssemblyCSharp;
using Vokey;

public class FileHandler : RequestHandler
{
    private static string[] acceptableCommands = { "file" };
    private static Dictionary<string, string> extensionMimeTypes;
    private static void fillDictionary()
    {
        if (extensionMimeTypes == null)
        {
            extensionMimeTypes = new Dictionary<string, string>();
            extensionMimeTypes.Add(".bin", "application/octet-stream");
            extensionMimeTypes.Add(".unity3d", "application/octet-stream");
            extensionMimeTypes.Add(".ico", "image/vnd.microsoft.icon");
        }
    }

    public FileHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {
        if (hlc == null) FileHandler.fillDictionary();
    }

    public override void handleComplexRequest(string action)
    {
        MemoryStream ms = new MemoryStream();
        string filename = splitArrayFromHandlableAction(context.Request.Url.ToString())[1];
        string extension = filename.Substring(filename.LastIndexOf('.'));
        if (extensionMimeTypes.ContainsKey(extension))
        {
            string mime = null;
            extensionMimeTypes.TryGetValue(extension, out mime);
            using (FileStream fileStream = File.OpenRead(AssetServer.AssetRoot + Path.DirectorySeparatorChar + "AssetBundles" + Path.DirectorySeparatorChar + filename))
            {
                ms.SetLength(fileStream.Length);
                fileStream.Read(ms.GetBuffer(), 0, (int)fileStream.Length);
            }
            //System.Drawing.Image img = System.Drawing.Image.FromFile ("AssetBundles/favicon.ico");
            //img.Save (ms, System.Drawing.Imaging.ImageFormat.Gif);

            HttpFunctions.sendFileWithContentType(context, mime, ms.ToArray());
        }
        else
        {
            throw new System.Exception("File type not allowed!");
        }
    }
}