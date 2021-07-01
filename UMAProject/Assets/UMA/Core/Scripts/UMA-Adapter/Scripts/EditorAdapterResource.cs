#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UMA.CharacterSystem;
using UnityEditor;
using UnityEngine;

namespace UMA
{
    public class EditorAdapterResource : AdapterResource
    {
        private const string _assetRootPath = "Assets/UMA";

        private Dictionary<string, UnityEngine.Object> _allAssets;
        private List<string> _allAssetPath;
        private Dictionary<Type, List<UnityEngine.Object>> _allTypeAssets;

        public EditorAdapterResource()
        {
            foreach (var assetType in _assetTypes)
            {
                List<UnityEngine.Object> @objects = new List<UnityEngine.Object>();
                _allTypeAssets.Add(assetType, @objects);

                var findAssets = AssetDatabase.FindAssets($"t:{assetType.Name}", new string[] { _assetRootPath });
                foreach (var item in findAssets)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(item);
                    var @object = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                    if (!_allAssets.ContainsKey(assetPath))
                    {
                        _allAssets.Add(assetPath, @object);
                        _allAssetPath.Add(assetPath);
                    }
                    @objects.Add(@object);
                    //Debug.Log($"[Find asset]: {assetPath} ### [type]: {@object.GetType()}");
                }
            }
        }

        public override List<T> GetAllAssets<T>(string[] foldersToSearch = null)
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

        public override T GetAsset<T>(string name) 
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning($"The resource name cannot be NULL! name: {name} type: {typeof(T).Name}");
                return null;
            }
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
                    else
                    {
                        if (item.name.Equals(name))
                        {
                            return item as T;
                        }
                    }
                }
                //Debug.LogWarning($"Cannot get the object's INameProvider! name: {name} type: {typeof(T).Name}");
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