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

        protected Type[] _assetTypes { get; } = new Type[] {typeof(RaceData), typeof(SlotDataAsset), typeof(UMATextRecipe),
            typeof(UMAMaterial), typeof(OverlayDataAsset), typeof(DynamicUMADnaAsset),
            typeof(RuntimeAnimatorController),typeof(AnimatorOverrideController),
            typeof(UMAWardrobeRecipe),typeof(UMAWardrobeCollection),typeof(TextAsset),
            //
        };

        public abstract List<T> GetAllAssets<T>(string[] foldersToSearch = null) where T : UnityEngine.Object;

        public abstract T GetAsset<T>(string name) where T : UnityEngine.Object;
    }
}
