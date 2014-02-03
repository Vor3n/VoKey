using UnityEngine;
using System.Collections;

public class RoomLoader : MonoBehaviour
{
    public string url;
    public int version;
    public Vector3 position;
    public bool instantiateMainAsset = false;
    //private bool assetBundleIsLoaded = false;
    // public bool unloadAfter = false;
    public IEnumerator LoadBundle()
    {
        using (WWW www = WWW.LoadFromCacheOrDownload(url, version))
        {
            yield return www;
            UnityEngine.AssetBundle assetBundle = www.assetBundle;
            if (instantiateMainAsset)
            {
                GameObject gameObject2 = assetBundle.mainAsset as GameObject;
                gameObject2.transform.position = transform.position;
                Instantiate(gameObject2);
            }
            Object[] loadedObjects = assetBundle.LoadAll();

            int i = 0;
            foreach (Object o in loadedObjects)
            {
                if (o.GetType() == typeof(GameObject))
                {
                    Debug.Log("" + i + ": " + o.name);

                }
                i++;
            }
        }
    }
    void Start()
    {
        StartCoroutine(LoadBundle());

    }
}
