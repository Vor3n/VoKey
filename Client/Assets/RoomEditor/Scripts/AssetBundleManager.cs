using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AssetBundleManager : MonoBehaviour {
	private Guid ID;
	private BundleLoader bundleLoader;
	private int AmountBundlesToFill;
	
	private VokeyAssetBundle LoadedBundle = null;
    private bool assetBundleIsLoaded = false;
	private event System.Action<UnityEngine.Object[],Guid> BundleLoaded;
	private event System.Action XMLLoaded;
	public event System.Action AllBundlesLoaded;

	void HandleXmlLoaded ()
	{
		AmountBundlesToFill = Bundles.Count;
		foreach(VokeyAssetBundle VAB in Bundles.Values){	
			StartCoroutine(DownloadBundle(VAB.modelId));
			
		}
		
		
		//Debug.Log("XML LOADED:" + Bundles.Count);
	}
	
	void HandleBundleLoaded (UnityEngine.Object[] objects, Guid id){
		AmountBundlesToFill--;
		try{
			//Bundles[id].LoadBinaryObjects(objects);
            //Debug.Log("HOERTJES. HET ZIJN ER: " + objects.Length + ", Maar we hebben:[" + Bundles[id].objects.Count + "]");
		} catch (Exception e){
			Debug.Log (e.GetBaseException().ToString());
		}
		
		Debug.Log("Bundle done! " + AmountBundlesToFill + " to go!");
		if(AmountBundlesToFill==0){
			if(AllBundlesLoaded != null)
				AllBundlesLoaded();
		}
		
	}
	
	public string url = GlobalSettings.serverURL;
	
	public Dictionary<Guid,VokeyAssetBundle> Bundles = new Dictionary<Guid,VokeyAssetBundle>();
	
	
	
	/// <summary>
	/// Loads the bundle.
	/// </summary>
	/// <returns>
	/// The bundle.
	/// </returns>
	/// <param name='id'>
	/// Identifier.
	/// </param>
	public IEnumerator DownloadBundle(Guid id){

        using (WWW www = WWW.LoadFromCacheOrDownload(url + "file/AssetBundle.bin", 0))
        {
            yield return www;
		//	Debug.Log (www.error);
            AssetBundle assetBundle = www.assetBundle;
			assetBundleIsLoaded = www.isDone;
            if (assetBundle == null)
            {
                Debug.Log("Duncan: Assetbundle is null");
            }
            else
            {
                UnityEngine.Object[] loadedObjects = assetBundle.LoadAll(typeof(GameObject));

                for (int i = 0; i < loadedObjects.Length; i++)
                {
                    AssetBundleRequest request = assetBundle.LoadAsync(loadedObjects[i].name, typeof(GameObject));

                    yield return request;

                    GameObject obj = request.asset as GameObject;
					
                    Bundles[id].objects.Find(x => x.name == obj.name).resource = obj;

                }

                if (BundleLoaded != null)
                    BundleLoaded(loadedObjects, id);
            }
        }
		
	}
	
	/// <summary>
	/// Retrieves the gameobject.
	/// </summary>
	/// <returns>
	/// The object.
	/// </returns>
	/// <param name='hash'>
	/// Hash.
	/// </param>
	public GameObject RetrieveObject( string hash){
		foreach( VokeyAssetBundle VAB in Bundles.Values){
			foreach(VokeyAsset VA in VAB.objects){
				//Debug.Log(VA.hashString+" - " + VA.name + " - "+ VA.resource);
				if(VA.hashString +"" == hash){
					return (GameObject)  VA.resource;	
				}				
			}
			
		}
		return null;
	}

    public String RetrieveObjectName(string hash)
    {
        foreach (VokeyAssetBundle VAB in Bundles.Values)
        {
            foreach (VokeyAsset VA in VAB.objects)
            {
                //Debug.Log(VA.hashString + " - " + VA.name + " - " + VA.resource);
                if (VA.hashString + "" == hash)
                {
					
                    return VA.name;
                }
            }

        }
        return string.Empty;
    }
	
	void Start(){
		url = GlobalSettings.serverURL;
		Bundles = new Dictionary<Guid,VokeyAssetBundle>();
		Caching.CleanCache ();
		BundleLoaded += HandleBundleLoaded;
		XMLLoaded += HandleXmlLoaded;
		AllBundlesLoaded += BUNDLESLOADED;
		StartCoroutine(RetrieveAssetbundleXmlArray());
	}
	
	void BUNDLESLOADED(){
		/*
		foreach(VokeyAssetBundle VAB in Bundles.Values){
			//Debug.Log (VAB.modelId+ "-"+ VAB.name );	
			foreach(VokeyAsset VA in VAB.objects){
				//Debug.Log (VA.hashString+ "-"+ VA.name + "[" +  (VA.resource != null ? VA.resource.GetHashCode().ToString() : "empty") + "]");
				//GameObject.Instantiate(VA.resource);
			}
		}
		//GameObject.Find("ItemList").GetComponent<ItemManager>().LoadFromBundles();
		*/
	}

	
	public IEnumerator RetrieveAssetbundleXmlArray(){
		using(WWW www = new WWW(url + "/assetbundle" )) {
			if(www.error != null) {
				Debug.Log("Cannot connect to server");
			} else {
				yield return www;
			}
			
			List<VokeyAssetBundle> bundles = VokeySharedEntities.MySerializerOfLists.FromXml<VokeyAssetBundle>(www.text);
			
			foreach(VokeyAssetBundle bundle in bundles){
				Bundles.Add(bundle.modelId, bundle);	
				
			}
			
			if(XMLLoaded != null) {	
				XMLLoaded();
			}
			
		}
	}
}
