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
        //protected Dictionary<int, string> _nameHash = new Dictionary<int, string>();
        protected Dictionary<Type, List<UnityEngine.Object>> _allTypeAssets=new Dictionary<Type, List<UnityEngine.Object>>();

        protected Dictionary<System.Type, System.Type> TypeToLookup = new Dictionary<System.Type, System.Type>()
        {
        { (typeof(SlotDataAsset)),(typeof(SlotDataAsset)) },
        { (typeof(OverlayDataAsset)),(typeof(OverlayDataAsset)) },
        { (typeof(RaceData)),(typeof(RaceData)) },
        { (typeof(UMATextRecipe)),(typeof(UMATextRecipe)) },
        { (typeof(UMAWardrobeRecipe)),(typeof(UMAWardrobeRecipe)) },
        { (typeof(UMAWardrobeCollection)),(typeof(UMAWardrobeCollection)) },
        { (typeof(RuntimeAnimatorController)),(typeof(RuntimeAnimatorController)) },
        { (typeof(AnimatorOverrideController)),(typeof(RuntimeAnimatorController)) },
//#if UNITY_EDITOR
//        { (typeof(AnimatorController)),(typeof(RuntimeAnimatorController)) },
//#endif
        {  typeof(TextAsset), typeof(TextAsset) },
        { (typeof(DynamicUMADnaAsset)), (typeof(DynamicUMADnaAsset)) },
        {(typeof(UMAMaterial)), (typeof(UMAMaterial)) }
        };

        public static Type[] AssetTypes { get; } = new Type[] {typeof(RaceData), typeof(SlotDataAsset), typeof(UMARecipeBase),
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

        public virtual T GetAsset<T>(int nameHash) where T : UnityEngine.Object
        {
            if (_allTypeAssets.TryGetValue(typeof(T), out List<UnityEngine.Object> resultObjects))
            {
                foreach (var item in resultObjects)
                {
                    INameProvider nameProvider = item as INameProvider;
                    if (nameProvider != null)
                    {
                        if (nameProvider.GetAssetName().GetHashCode().Equals(nameHash))
                        {
                            return item as T;
                        }
                    }
                    else
                    {
                        if (item.name.GetHashCode().Equals(nameHash))
                        {
                            return item as T;
                        }
                    }
                }
            }

            Debug.LogWarning($"[EditorAdapterResource] The corresponding Object could not be found! name hashcode: {nameHash} type: {typeof(T)}");

            return null;
        }

        public virtual AssetItem GetAssetItem<T>(string name) where T : UnityEngine.Object
        {
            var @object= GetAsset<T>(name);
            if (@object == null)
                return null;

            string path = "";
            foreach (var item in _allAssets)
            {
                if (@object == item.Value)
                {
                    path = item.Key;
                    break;
                }
            }
            if (string.IsNullOrEmpty(path))
                return null;

            //string guid = UnityEditor.AssetDatabase.GetAssetPath(@object);
            //path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var ai = new AssetItem(typeof(T), name, path, @object);
            /*
            foreach (AssetItem ai in TypeDic.Values)
            {
                if (Name == ai.EvilName)
                {
                    RebuildIndex();
                    return ai;
                }
            }*/
            return ai;
        }

        public virtual void ReleaseReference(UnityEngine.Object obj)
        {
            
        }

    }

}
