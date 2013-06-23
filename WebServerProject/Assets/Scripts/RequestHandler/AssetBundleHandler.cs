using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using VokeySharedEntities;
using Vokey;

public class AssetBundleHandler : RequestHandler {

    private static string[] acceptableCommands = { "assetbundle" };

    public AssetBundleHandler(HttpListenerContext hlc)
        : base(hlc, acceptableCommands)
    {
        
    }

    public override void handleSimpleRequest(string command)
    {
        HttpFunctions.returnXmlStringToHttpClient(context, AssetServer.getInstance().AssetBundles.ToXml());
    }
}
