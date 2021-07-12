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

        public EditorAdapterResource()
        {
            foreach (var assetType in AssetTypes)
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
    }
}
#endif