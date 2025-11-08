using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class BuildAB : EditorWindow
{
    [MenuItem("Tools/BuildAB")]
    public static void BuildABWindows()
    {
        string path = "Assets/AssetBundle";

        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
#endif