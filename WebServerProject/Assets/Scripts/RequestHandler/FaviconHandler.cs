using AssemblyCSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Vokey;

/// <summary>
/// This handler handles Favicons.
/// </summary>
public class FaviconHandler : RequestHandler
{
    private static string[] acceptableCommands = { "favicon.ico" };

    public FaviconHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {

    }

    public override void handleSimpleRequest(string action)
    {
        MemoryStream ms = null;
        try
        {
            using (FileStream fileStream = File.OpenRead(AssetServer.AssetRoot + Path.DirectorySeparatorChar + "AssetBundles/favicon.ico"))
            {
                ms.SetLength(fileStream.Length);
                fileStream.Read(ms.GetBuffer(), 0, (int)fileStream.Length);
            }
            //System.Drawing.Image img = System.Drawing.Image.FromFile ("AssetBundles/favicon.ico");
            //img.Save (ms, System.Drawing.Imaging.ImageFormat.Gif);
        }
        catch
        {
        }
        HttpFunctions.sendFileWithContentType(context, "image/vnd.microsoft.icon", ms.ToArray());
    }
}

