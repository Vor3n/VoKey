using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using VokeySharedEntities;
using Vokey;
using System.IO;
using System;
using Thisiswhytheinternetexists.WebCore;

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
    
    public override void handleComplexRequest (string command)
	{
        Guid bundle = new Guid(splitArrayFromHandlableAction(context.Request.Url.ToString())[1]);
        VokeyAssetBundle vab = null;
        vab = AssetServer.getInstance().getVokeyAssetBundle(bundle);
        FileHandler.returnFile(context, vab.name, ".bin");
	}
}
