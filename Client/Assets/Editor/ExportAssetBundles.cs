// C# Example
// Builds an asset bundle from the selected objects in the project view.
// Once compiled go to "Menu" -> "Assets" and select one of the choices
// to build the Asset Bundle

using UnityEngine;
using UnityEditor;
using VokeySharedEntities;


[InitializeOnLoad]
public class ExportAssetBundles
{
	static ExportAssetBundles()
    {
        Debug.Log("ExportAssetBundles is ready");
    }
	
    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies - Vokey")]
    static void ExportResource()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource Bundle", "", "AssetBundle", "bin");
		Debug.Log ("path: " + path);
		string folder = path.Substring(0, path.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1);
		Debug.Log ("folder: " + folder);
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			VokeyAssetBundle vab = VokeyAssetBundle.FromObjectsArray(selection);
			string[] pathParts = path.Split (System.IO.Path.DirectorySeparatorChar);
			string pathWithExtension = pathParts[pathParts.Length - 1];
			string filename = pathWithExtension.Substring(0, pathWithExtension.LastIndexOf('.'));
			vab.resourceFilename = filename + ".bin";
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(folder + "vab_" + filename + ".xml", true))
            {
                file.Write(vab.ToXml());
            }

            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);
            Selection.objects = selection;
        }
    }
    [MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking")]
    static void ExportResourceNoTrack()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "bin");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path);
        }
    }
}