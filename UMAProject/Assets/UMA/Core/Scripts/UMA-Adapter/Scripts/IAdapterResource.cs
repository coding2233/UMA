using System;
using System.Collections;
using System.Collections.Generic;
using UMA.CharacterSystem;
using UnityEngine;

namespace UMA
{
    public abstract class AdapterResource
    {
        protected Dictionary<string, UnityEngine.Object> _allAssets=new Dictionary<string, UnityEngine.Object>();
        protected List<string> _allAssetPath=new List<string>();
        protected Dictionary<Type, List<UnityEngine.Object>> _allTypeAssets=new Dictionary<Type, List<UnityEngine.Object>>();

        public static Type[] AssetTypes { get; } = new Type[] {typeof(RaceData), typeof(SlotDataAsset), 
            typeof(UMAMaterial), typeof(OverlayDataAsset), typeof(DynamicUMADnaAsset),
            typeof(RuntimeAnimatorController),typeof(AnimatorOverrideController),
            typeof(UMAWardrobeRecipe),typeof(UMAWardrobeCollection),typeof(UMATextRecipe),typeof(TextAsset),
            //
        };

        public virtual List<T> GetAllAssets<T>(string[] foldersToSearch = null) where T : UnityEngine.Object
        {
            List<T> result = new List<T>();
            if (_allTypeAssets.TryGetValue(typeof(T), out List<UnityEngine.Object> resultObjects))
            {
                foreach (var item in resultObjects)
                {
                    result.Add(item as T);
                }
            }

            if (result.Count == 0)
            {
                Debug.Log($"[EditorAdapterResource] [GetAllAssets] Object of corresponding type was not found! type: {typeof(T)}");
            }

            return result;
        }

        public virtual T GetAsset<T>(string name) where T : UnityEngine.Object
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
            }

            Debug.LogWarning($"[EditorAdapterResource] The corresponding Object could not be found! name: {name} type: {typeof(T)}");

            return null;
        }
    }
}
