using UnityEngine;
using System.Collections;

public class BundleLoader : MonoBehaviour{
    public string url;
    public int version;
	public Vector3 position;
    public IEnumerator LoadBundle(){
        using(WWW www = WWW.LoadFromCacheOrDownload(url, version)){
            yield return www;
            AssetBundle assetBundle = www.assetBundle;
            GameObject gameObject2 = assetBundle.mainAsset as GameObject;
			gameObject2.transform.position = transform.position;			
            Instantiate(gameObject2);
            assetBundle.Unload(false);
			Destroy (gameObject);
        }
    }
    void Start(){
        StartCoroutine(LoadBundle());
		
    }
}
