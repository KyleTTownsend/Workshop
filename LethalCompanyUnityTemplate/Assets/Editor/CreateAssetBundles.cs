using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build-AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDir = "Assets/AssetBundles";
        if(!Directory.Exists(assetBundleDir))
        {
            Directory.CreateDirectory(assetBundleDir);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDir, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}

