using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UMA.CharacterSystem;
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
            var abs = manifest.GetAllAssetBundles();

            foreach (var assetType in AssetTypes)
            {
                List<UnityEngine.Object> @objects = new List<UnityEngine.Object>();
                _allTypeAssets.Add(assetType, @objects);
            }

            foreach (var abName in abs)
            {
                string abPath = Path.Combine(Application.persistentDataPath, abName);
                var ab = AssetBundle.LoadFromFile(abPath);
                //List<UnityEngine.Object> @objects = new List<UnityEngine.Object>();
                //_allTypeAssets.Add(assetType, @objects);

                var findAssets = ab.GetAllAssetNames();
                foreach (var item in findAssets)
                {
                    var @object = ab.LoadAsset<UnityEngine.Object>(item);
                    if (!_allAssets.ContainsKey(item))
                    {
                        _allAssets.Add(item, @object);
                        _allAssetPath.Add(item);
                    }

                    var types = CheckObjectType(@object);

                    foreach (var itemType in types)
                    {
                        if (_allTypeAssets.TryGetValue(itemType, out List<UnityEngine.Object> objs))
                        {
                            _allTypeAssets[itemType].Add(@object);
                        }
                        else
                        {
                            List<UnityEngine.Object> newobjs = new List<UnityEngine.Object>() { @object };
                            _allTypeAssets.Add(itemType, newobjs);
                        }
                    }

                    //@objects.Add(@object);

                    //Debug.Log($"[Find asset]: {assetPath} ### [type]: {@object.GetType()}");
                }
            }

            //foreach (var assetType in AssetTypes)
            //{
            //    if (assetType == typeof(TextAsset) || assetType==typeof(AnimatorOverrideController))
            //        continue;
            //    string abPath = Path.Combine(Application.persistentDataPath, $"109{assetType.Name.ToLower()}");

            //    //if (!File.Exists(abPath))
            //    //{
            //    //    continue;
            //    //}
            //    var ab = AssetBundle.LoadFromFile(abPath);

            //    List<UnityEngine.Object> @objects = new List<UnityEngine.Object>();
            //    _allTypeAssets.Add(assetType, @objects);

            //    var findAssets = ab.GetAllAssetNames();
            //    foreach (var item in findAssets)
            //    {
            //        var @object = ab.LoadAsset<UnityEngine.Object>(item);
            //        if (!_allAssets.ContainsKey(item))
            //        {
            //            _allAssets.Add(item, @object);
            //            _allAssetPath.Add(item);
            //        }
            //        @objects.Add(@object);
            //        //Debug.Log($"[Find asset]: {assetPath} ### [type]: {@object.GetType()}");
            //    }
            //}


        }



        private HashSet<Type> CheckObjectType(UnityEngine.Object @object)
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(@object.GetType());
            if (@object is RaceData)
            {
                types.Add(typeof(RaceData));
            }
            if (@object is SlotDataAsset)
            {
                types.Add(typeof(SlotDataAsset));
            }
            if (@object is UMAMaterial)
            {
                types.Add(typeof(UMAMaterial));
            }
            if (@object is OverlayDataAsset)
            {
                types.Add(typeof(OverlayDataAsset));
            }
            if (@object is DynamicUMADnaAsset)
            {
                types.Add(typeof(DynamicUMADnaAsset));
            }
            if (@object is RuntimeAnimatorController)
            {
                types.Add(typeof(RuntimeAnimatorController));
            }
            if (@object is AnimatorOverrideController)
            {
                types.Add(typeof(AnimatorOverrideController));
            }

            if (@object is UMAWardrobeRecipe)
            {
                types.Add(typeof(UMAWardrobeRecipe));
            }
            if (@object is UMAWardrobeCollection)
            {
                types.Add(typeof(UMAWardrobeCollection));
            }

            if (@object is UMATextRecipe)
            {
                types.Add(typeof(UMATextRecipe));
            }
            if (@object is TextAsset)
            {
                types.Add(typeof(TextAsset));
            }

            if (@object is UMARecipeBase)
            {
                types.Add(typeof(UMARecipeBase));
            }

            return types;
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
            HashSet<string> assetPaths = new HashSet<string>();

            foreach (var assetType in _assetTypes)
            {
                if (assetType == typeof(TextAsset))
                    continue;

                var findAssets = AssetDatabase.FindAssets($"t:{assetType.Name}", new string[] { "Assets/UMA" });
                if (findAssets.Length > 0)
                {
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

                  
                }
            }

         //   if (assetPaths.Count > 0)
            {
                AssetBundleBuild abb = new AssetBundleBuild();
                abb.assetBundleName = $"dddddata";
                abb.assetNames = new string[assetPaths.Count];
                assetPaths.CopyTo(abb.assetNames);
                abbs.Add(abb);
            }

            string buildPath = "build/asset/";
            if (!Directory.Exists(buildPath))
            {
                Directory.CreateDirectory(buildPath);
            }
            BuildAssetBundleOptions options = BuildAssetBundleOptions.None;
            options |= BuildAssetBundleOptions.UncompressedAssetBundle;
            BuildPipeline.BuildAssetBundles(buildPath, abbs.ToArray(), options,EditorUserBuildSettings.activeBuildTarget);

            //UnityEditor.BuildPipeline
        }
    }


#endif

}