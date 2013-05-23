using UnityEngine;
using System.Collections;
using System.Xml;

public class BundleLoader : MonoBehaviour{
    public string url;
    public int version;
	public Vector3 position;
    public string roomName;
    public bool instantiateMainAsset = false;
    private bool assetBundleIsLoaded = false;
   // public bool unloadAfter = false;
    public IEnumerator LoadBundle(){
        using(WWW www = WWW.LoadFromCacheOrDownload(url, version)){
            yield return www;
            AssetBundle assetBundle = www.assetBundle;
            if (instantiateMainAsset)
            {
                GameObject gameObject2 = assetBundle.mainAsset as GameObject;
                gameObject2.transform.position = transform.position;
                Instantiate(gameObject2);
            }
            Object[] loadedObjects = assetBundle.LoadAll();
            VokeyAssetBundle ab = VokeyAssetBundle.FromBundle(loadedObjects);
            ab.resourceUrl = new System.Uri(url);

            Room r = new Room(roomName);
            int i = 0;
            float p = 0;
            foreach (VokeyAsset o in ab.objects)
            {
                if (o.GetType() == typeof(GameObject))
                {
                    //Debug.Log("" + i + ": " + o.name);
                    //r.AddGameObject(o.resource, new Vector3(p++, 0f, 0f));
                }
                i++;
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("room_" + roomName + ".xml", true))
            {
                file.Write(Room.ToXml(r));
            }
            
            //assetBundle.Unload(false);
			//sDestroy (gameObject);
        }
    }
    void Start(){
        StartCoroutine(LoadBundle());
		
    }
}
