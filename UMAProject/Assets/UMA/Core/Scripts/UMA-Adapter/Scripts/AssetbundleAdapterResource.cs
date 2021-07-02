using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UMA
{
    public class AssetBundleAdapterResource : AdapterResource
    {
        public AssetBundleAdapterResource()
        { 
            AssetBundle.UnloadAllAssetBundles(true);
            var manifestAB = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath,"asset"));
            AssetBundleManifest manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            foreach (var assetType in AssetTypes)
            {
                if (assetType == typeof(TextAsset))
                    continue;
                string abPath = Path.Combine(Application.persistentDataPath, $"109{assetType.Name.ToLower()}");

                if (!File.Exists(abPath))
                {
                    continue;
                }
                 var ab = AssetBundle.LoadFromFile(abPath);
                
                List<UnityEngine.Object> @objects = new List<UnityEngine.Object>();
                _allTypeAssets.Add(assetType, @objects);

                var findAssets = ab.GetAllAssetNames();
                foreach (var item in findAssets)
                {
                    var @object = ab.LoadAsset<UnityEngine.Object>(item);
                    if (!_allAssets.ContainsKey(item))
                    {
                        _allAssets.Add(item, @object);
                        _allAssetPath.Add(item);
                    }
                    @objects.Add(@object);
                    //Debug.Log($"[Find asset]: {assetPath} ### [type]: {@object.GetType()}");
                }
            }


        }

        //public override List<T> GetAllAssets<T>(string[] foldersToSearch = null)
        //{
        //    return null;
        //}

        //public override T GetAsset<T>(string name)
        //{
        //    return null;
        //}
    }


#if UNITY_EDITOR

    public class AssetBundleAdapterResourceEditor
    {
        [MenuItem("UMA-Adapter/Build-AssetBundle")]
        static void main()
        {
            var _assetTypes = AdapterResource.AssetTypes;
            List<AssetBundleBuild> abbs = new List<AssetBundleBuild>();

            Dictionary<string, Type> allAssetPaths = new Dictionary<string, Type>();

            foreach (var assetType in _assetTypes)
            {
                if (assetType == typeof(TextAsset))
                    continue;

                var findAssets = AssetDatabase.FindAssets($"t:{assetType.Name}", new string[] { "Assets/UMA" });
                if (findAssets.Length > 0)
                {
                    HashSet<string> assetPaths = new HashSet<string>();
                    foreach (var item in findAssets)
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(item);
                        
                        if (allAssetPaths.ContainsKey(assetPath))
                        {
                            Debug.LogWarning($"Repeat resources£º{assetPath} type: {assetType.Name} <-> oldType: {allAssetPaths[assetPath].Name} ");
                        }
                        else
                        {
                            assetPaths.Add(assetPath);
                            allAssetPaths.Add(assetPath,assetType);
                        }
                    }

                    if (assetPaths.Count > 0)
                    {
                        AssetBundleBuild abb = new AssetBundleBuild();
                        abb.assetBundleName = $"109{assetType.Name.ToLower()}";
                        abb.assetNames = new string[assetPaths.Count];
                        assetPaths.CopyTo(abb.assetNames);
                        abbs.Add(abb);
                    }
                }
            }

            string buildPath = "build/asset/";
            if (!Directory.Exists(buildPath))
            {
                Directory.CreateDirectory(buildPath);
            }
            BuildAssetBundleOptions options = BuildAssetBundleOptions.None;
            options |= BuildAssetBundleOptions.UncompressedAssetBundle;
            BuildPipeline.BuildAssetBundles(buildPath, abbs.ToArray(), options,BuildTarget.Android);

            //UnityEditor.BuildPipeline
        }
    }


#endif

}