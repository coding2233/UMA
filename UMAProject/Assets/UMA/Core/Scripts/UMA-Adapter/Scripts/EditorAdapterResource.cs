#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UMA
{
    public class EditorAdapterResource : IAdapterResource
    {
        private const string _assetRootPath = "Assets/UMA";

        private Dictionary<string, UnityEngine.Object> _allAssets;
        private List<string> _allAssetPath;

        private Type[] _assetTypes = new Type[] {typeof(RaceData), typeof(SlotDataAsset), typeof(UMATextRecipe), typeof(UMAMaterial), typeof(OverlayDataAsset), typeof(DynamicUMADnaAsset)};
        private Dictionary<Type, List<UnityEngine.Object>> _allTypeAssets;

        public EditorAdapterResource()
        {
            _allAssets = new Dictionary<string, UnityEngine.Object>();
            _allAssetPath = new List<string>();
            _allTypeAssets = new Dictionary<Type, List<UnityEngine.Object>>();

            foreach (var assetType in _assetTypes)
            {
                List<UnityEngine.Object> @objects = new List<UnityEngine.Object>();
                _allTypeAssets.Add(assetType, @objects);

                var findAssets = AssetDatabase.FindAssets($"t:{assetType.Name}", new string[] { _assetRootPath });
                foreach (var item in findAssets)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(item);
                    var @object = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                    _allAssets.Add(assetPath, @object);
                    _allAssetPath.Add(assetPath);
                    @objects.Add(@object);
                    //Debug.Log($"[Find asset]: {assetPath} ### [type]: {@object.GetType()}");
                }
            }
        }

        public List<T> GetAllAssets<T>(string[] foldersToSearch = null) where T : UnityEngine.Object
        {
            List<T> result = new List<T>();
            if (_allTypeAssets.TryGetValue(typeof(T), out List<UnityEngine.Object> resultObjects))
            {
                foreach (var item in resultObjects)
                {
                    result.Add(item as T);
                }
            }

            if (result.Count==0)
            {
                Debug.Log($"[EditorAdapterResource] [GetAllAssets] Object of corresponding type was not found! type: {typeof(T)}");
            }
            
            return result;
        }

        public T GetAsset<T>(string name) where T : UnityEngine.Object
        {
            if (_allTypeAssets.TryGetValue(typeof(T), out List<UnityEngine.Object> resultObjects))
            {
                foreach (var item in resultObjects)
                {
                    INameProvider nameProvider = item as INameProvider;
                    if (nameProvider != null)
                    {
                        if (nameProvider.GetAssetName().Equals(name))
                        {
                            return item as T;
                        }
                    }
                    
                }
            }
            //    var matchAssetPaths = _allAssetPath.FindAll((assetPath) => {
            //    string fileName = Path.GetFileNameWithoutExtension(assetPath).Replace(" ","").ToLower();
            //    //Debug.Log($"[EditorAdapterResource] {fileName}");
            //    if (fileName.Equals(name.ToLower()))
            //    {
            //        return true;
            //    }
            //    return false;
            //});

            //if (matchAssetPaths != null)
            //{
            //    foreach (var item in matchAssetPaths)
            //    {
            //        var @object = AssetDatabase.LoadAssetAtPath<T>(item);
            //        if (@object != null)
            //        {
            //            return @object;
            //        }
            //    }
            //}

            Debug.LogWarning($"[EditorAdapterResource] The corresponding Object could not be found! name: {name} type: {typeof(T)}");

            return null;
        }
    
    }
}
#endif