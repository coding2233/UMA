using System;
using System.Collections;
using System.Collections.Generic;
using UMA.CharacterSystem;
using UnityEngine;

namespace UMA
{
    public class UMAAssetIndexer
    {
        private static UMAAssetIndexer _instance;
        public static UMAAssetIndexer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UMAAssetIndexer();
                    if (_instance._adapterResource == null)
                    {
#if UNITY_EDITOR
                        _instance._adapterResource = new EditorAdapterResource();
#endif
                    }
                }
                
                return _instance;
            }
        }


        private AdapterResource _adapterResource;

        #region constants and static strings
        public static string SortOrder = "Name";
        public static string[] SortOrders = { "Name", "AssetName" };
        public static Dictionary<string, System.Type> TypeFromString = new Dictionary<string, System.Type>();
        public static Dictionary<string, AssetItem> GuidTypes = new Dictionary<string, AssetItem>();
        #endregion


        #region Fields
        // The names of the fully qualified types.
        public List<string> IndexedTypeNames = new List<string>();
        // These list is used so Unity will serialize the data
        public List<AssetItem> SerializedItems = new List<AssetItem>();
        // This is really where we keep the data.
        private Dictionary<System.Type, Dictionary<string, AssetItem>> TypeLookup = new Dictionary<System.Type, Dictionary<string, AssetItem>>();
        // This list tracks the types for use in iterating through the dictionaries
        private System.Type[] Types =
        {
                (typeof(SlotDataAsset)),
                (typeof(OverlayDataAsset)),
                (typeof(RaceData)),
                (typeof(UMATextRecipe)),
                (typeof(UMAWardrobeRecipe)),
                (typeof(UMAWardrobeCollection)),
                (typeof(RuntimeAnimatorController)),
                (typeof(AnimatorOverrideController)),
        #if UNITY_EDITOR
                //(typeof(AnimatorController)),
        #endif
                (typeof(DynamicUMADnaAsset)),
                (typeof(TextAsset)),
                (typeof(UMAMaterial))
         };
        #endregion

        /// <summary>
        /// Builds a list of types and a string to look them up.
        /// </summary>
        public void BuildStringTypes()
        {
            TypeFromString.Clear();
            foreach (System.Type st in Types)
            {
                TypeFromString.Add(st.Name, st);
            }
        }

        /// <summary>
        /// Rebuilds the name indexes by dumping everything back to the list, updating the name, and then rebuilding 
        /// the dictionaries.
        /// </summary>
        public void RebuildIndex()
        {
            //UpdateSerializedList();
            //foreach (AssetItem ai in SerializedItems)
            //{
            //    ai._Name = ai.EvilName;
            //}
            //UpdateSerializedDictionaryItems();
        }

        #region Timer
        public static System.Diagnostics.Stopwatch StartTimer()
        {
#if TIMEINDEXER
                    if(Debug.isDebugBuild)
                        Debug.Log("Timer started at " + Time.realtimeSinceStartup + " Sec");
                    System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
                    st.Start();

                    return st;
#else
            return null;
#endif
        }

        public static void StopTimer(System.Diagnostics.Stopwatch st, string Status)
        {
#if TIMEINDEXER
                    st.Stop();
                    if(Debug.isDebugBuild)
                        Debug.Log(Status + " Completed " + st.ElapsedMilliseconds + "ms");
                    return;
#endif
        }
        #endregion

        #region Get asset

        /// <summary>
        /// Return the asset specified, if it exists.
        /// if it can't be found by name, then we do a scan of the assets to see if 
        /// we can find the name directly on the object, and return that. 
        /// We then rebuild the index to make sure it's up to date.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Name"></param>
        /// <returns></returns>
        public AssetItem GetAssetItem<T>(string Name)
        {
            Debug.Log($"[UMAAssetIndexer] [GetAssetItems] name: {Name}");
            //System.Type ot = typeof(T);
            //System.Type theType = TypeToLookup[ot];
            //Dictionary<string, AssetItem> TypeDic = GetAssetDictionary(theType);
            //if (TypeDic.ContainsKey(Name))
            //{
            //    return TypeDic[Name];
            //}
            /*
            foreach (AssetItem ai in TypeDic.Values)
            {
                if (Name == ai.EvilName)
                {
                    RebuildIndex();
                    return ai;
                }
            }*/
            return null;
        }

        /// <summary>
        /// Return the asset specified, if it exists.
        /// if it can't be found by name, then we do a scan of the assets to see if 
        /// we can find the name directly on the object, and return that. 
        /// We then rebuild the index to make sure it's up to date.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Name"></param>
        /// <returns></returns>
        public AssetItem GetAssetItemForObject(UnityEngine.Object o)
        {
            Debug.Log($"[UMAAssetIndexer] [GetAssetItemForObject] object name: {o.name} ");

            //System.Type ot = o.GetType();
            //System.Type theType = TypeToLookup[ot];
            //Dictionary<string, AssetItem> TypeDic = GetAssetDictionary(theType);

            //string Name = AssetItem.GetEvilName(o);

            //if (TypeDic.ContainsKey(Name))
            //{
            //    return TypeDic[Name];
            //}
            return null;
        }


        public List<AssetItem> GetAssetItems(string recipe, bool LookForLODs = false)
        {
            Debug.Log($"[UMAAssetIndexer] [GetAssetItems] recipe: {recipe} LookForLODs: {LookForLODs}");

            AssetItem ai = GetAssetItem<UMAWardrobeRecipe>(recipe);
            if (ai != null)
            {
                return GetAssetItems(ai.Item as UMAWardrobeRecipe, LookForLODs);
            }
            return new List<AssetItem>();
        }

        public List<AssetItem> GetAssetItems(UMAPackedRecipeBase recipe, bool LookForLODs = false)
        {
            Debug.Log($"[UMAAssetIndexer] [GetAssetItems] recipe: {recipe} LookForLODs: {LookForLODs}");
            List<AssetItem> returnval = new List<AssetItem>();
            //if (recipe is UMAWardrobeCollection)
            //{
            //    return new List<AssetItem>();
            //}
            //UMAPackedRecipeBase.UMAPackRecipe PackRecipe = recipe.PackedLoad(UMAContextBase.Instance);

            //var Slots = PackRecipe.slotsV3;

            //if (Slots == null)
            //    return GetAssetItemsV2(PackRecipe, LookForLODs);

            //Dictionary<string, AssetItem> TypeDic = GetAssetDictionary(typeof(SlotDataAsset));
            //List<AssetItem> returnval = new List<AssetItem>();

            //foreach (var slot in Slots)
            //{
            //    // We are getting extra blank slots. That's weird. 

            //    if (string.IsNullOrWhiteSpace(slot.id)) continue;

            //    AssetItem s = GetAssetItem<SlotDataAsset>(slot.id);
            //    if (s != null)
            //    {
            //        returnval.Add(s);
            //        string LodIndicator = slot.id.Trim() + "_LOD";
            //        if (slot.id.Contains("_LOD"))
            //        {
            //            // LOD is directly in the base recipe. 
            //            LodIndicator = slot.id.Substring(0, slot.id.Length - 1);
            //        }

            //        if (slot.overlays != null)
            //        {
            //            foreach (var overlay in slot.overlays)
            //            {
            //                AssetItem o = GetAssetItem<OverlayDataAsset>(overlay.id);
            //                if (o != null)
            //                {
            //                    returnval.Add(o);
            //                }
            //            }
            //        }
            //        if (LookForLODs)
            //        {
            //            foreach (string slod in TypeDic.Keys)
            //            {
            //                if (slod.StartsWith(LodIndicator))
            //                {
            //                    AssetItem lodSlot = GetAssetItem<SlotDataAsset>(slod);
            //                    returnval.Add(lodSlot);
            //                }
            //            }
            //        }
            //    }
            //}
            return returnval;
        }




        public List<T> GetAllAssets<T>(string[] foldersToSearch = null) where T : UnityEngine.Object
        {
            Debug.Log($"[UMAAssetIndexer] [GetAllAssets] foldersToSearch:{foldersToSearch == null} type:{typeof(T)}");
            return _adapterResource.GetAllAssets<T>();

            //var st = StartTimer();

            var ret = new List<T>();
            //System.Type ot = typeof(T);
            //System.Type theType = TypeToLookup[ot];

            //Dictionary<string, AssetItem> TypeDic = GetAssetDictionary(theType);

            //foreach (KeyValuePair<string, AssetItem> kp in TypeDic)
            //{
            //    if (AssetFolderCheck(kp.Value, foldersToSearch))
            //        ret.Add((kp.Value.Item as T));
            //}
            //StopTimer(st, "GetAllAssets type=" + typeof(T).Name);
            return ret;
        }

        public T GetAsset<T>(int nameHash, string[] foldersToSearch = null) where T : UnityEngine.Object
        {
            Debug.Log($"[UMAAssetIndexer] [GetAsset] nameHash: {nameHash} foldersToSearch:{foldersToSearch==null} type:{typeof(T)}");


            //System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
            //st.Start();
            //System.Type ot = typeof(T);
            //Dictionary<string, AssetItem> TypeDic = (Dictionary<string, AssetItem>)TypeLookup[ot];
            //string assetName = "";
            //int assetHash = -1;
            //foreach (KeyValuePair<string, AssetItem> kp in TypeDic)
            //{
            //    assetName = "";
            //    assetHash = -1;
            //    GetEvilAssetNameAndHash(typeof(T), kp.Value.Item, ref assetName, assetHash);
            //    if (assetHash == nameHash)
            //    {
            //        if (AssetFolderCheck(kp.Value, foldersToSearch))
            //        {
            //            st.Stop();
            //            if (st.ElapsedMilliseconds > 2)
            //            {
            //                if (Debug.isDebugBuild)
            //                    Debug.Log("GetAsset 0 for type " + typeof(T).Name + " completed in " + st.ElapsedMilliseconds + "ms");
            //            }
            //            return (kp.Value.Item as T);
            //        }
            //        else
            //        {
            //            st.Stop();
            //            if (st.ElapsedMilliseconds > 2)
            //            {
            //                if (Debug.isDebugBuild)
            //                    Debug.Log("GetAsset 1 for type " + typeof(T).Name + " completed in " + st.ElapsedMilliseconds + "ms");
            //            }
            //            return null;
            //        }
            //    }
            //}
            //st.Stop();
            //if (st.ElapsedMilliseconds > 2)
            //{
            //    if (Debug.isDebugBuild)
            //        Debug.Log("GetAsset 2 for type " + typeof(T).Name + " completed in " + st.ElapsedMilliseconds + "ms");
            //}
            return null;
        }

        public T GetAsset<T>(string name, string[] foldersToSearch) where T : UnityEngine.Object
        {
            Debug.Log($"[UMAAssetIndexer] [GetAsset] name: {name} foldersToSearch:{foldersToSearch[0]} type:{typeof(T)}");

            //var thisAssetItem = GetAssetItem<T>(name);
            //if (thisAssetItem != null)
            //{
            //    if (AssetFolderCheck(thisAssetItem, foldersToSearch))
            //        return (thisAssetItem.Item as T);
            //    else
            //        return null;
            //}
            //else
            //{
            //    return null;
            //}
            return null;
        }

        public T GetAsset<T>(string name) where T : UnityEngine.Object
        {
            //var thisAssetItem = GetAssetItem<T>(name);
            //if (thisAssetItem != null)
            //{
            //    return (thisAssetItem.Item as T);
            //}
            //else
            //{
            //    return null;
            //}

            //Debug.Log($"[UMAAssetIndexer] [GetAsset] name: {name} type:{typeof(T)}");

            return _adapterResource.GetAsset<T>(name); 
        }

        public List<string> GetRecipeNamesForRaceSlot(string race, string slot)
        {
            // Start with recipes that are directly marked for this race.
            //HashSet<string> results = internalGetRecipeNamesForRaceSlot(race, slot);

            //RaceData rc = GetAsset<RaceData>(race);
            //if (rc != null)
            //{
            //    foreach (string CompatRace in rc.GetCrossCompatibleRaces())
            //    {
            //        results.UnionWith(internalGetRecipeNamesForRaceSlot(CompatRace, slot));
            //    }
            //}

            //return results.ToList();
            return null;
        }


        public List<UMARecipeBase> GetRecipesForRaceSlot(string race, string slot)
        {
            // This will get the aggregate for all compatible races with no duplicates.
            List<string> recipes = GetRecipeNamesForRaceSlot(race, slot);

            // Build a list of recipes to return.
            List<UMARecipeBase> results = new List<UMARecipeBase>();

            foreach (string recipeName in recipes)
            {
                UMAWardrobeRecipe uwr = GetAsset<UMAWardrobeRecipe>(recipeName);
                if (uwr != null)
                {
                    results.Add(uwr);
                }
            }
            return results;
        }

        public Dictionary<string, List<UMATextRecipe>> GetRecipes(string race)
        {
            //Dictionary<string, HashSet<UMATextRecipe>> aggregate = new Dictionary<string, HashSet<UMATextRecipe>>();

            //internalGetRecipes(race, ref aggregate);

            //RaceData rc = GetAsset<RaceData>(race);
            //if (rc != null)
            //{
            //    foreach (string CompatRace in rc.GetCrossCompatibleRaces())
            //    {
            //        internalGetRecipes(CompatRace, ref aggregate);
            //    }
            //}

            //SlotRecipes results = new SlotRecipes();
            //foreach (KeyValuePair<string, HashSet<UMATextRecipe>> kp in aggregate)
            //{
            //    results.Add(kp.Key, kp.Value.ToList());
            //}

            //return results;
            return null;
        }

        #endregion


        #region Add asset
        /// <summary>
        /// Adds an asset to the index. Does NOT save the asset! you must do that separately.
        /// </summary>
        /// <param name="type">System Type of the object to add.</param>
        /// <param name="name">Name for the object.</param>
        /// <param name="path">Path to the object.</param>
        /// <param name="o">The Object to add.</param>
        /// <param name="skipBundleCheck">Option to skip checking Asset Bundles.</param>
        public void AddAsset(System.Type type, string name, string path, UnityEngine.Object o, bool skipBundleCheck = false)
        {
            //if (o == null)
            //{
            //    if (Debug.isDebugBuild)
            //        Debug.Log("Skipping null item");

            //    return;
            //}
            //if (type == null)
            //{
            //    type = o.GetType();
            //}

            //AssetItem ai = new AssetItem(type, name, path, o);
            //AddAssetItem(ai, skipBundleCheck);
        }
        #endregion


        #region Has asset

        public bool HasAsset<T>(string Name)
        {
            //System.Type ot = typeof(T);
            //System.Type theType = TypeToLookup[ot];
            //Dictionary<string, AssetItem> TypeDic = GetAssetDictionary(theType);
            //return TypeDic.ContainsKey(Name);
            return true;
        }

        public bool HasAsset<T>(int NameHash)
        {
            //System.Type ot = typeof(T);
            //System.Type theType = TypeToLookup[ot];
            //Dictionary<string, AssetItem> TypeDic = GetAssetDictionary(theType);

            //// This honestly hurt my heart typing this.
            //// Todo: replace this loop with a dictionary.
            //foreach (string s in TypeDic.Keys)
            //{
            //    if (UMAUtils.StringToHash(s) == NameHash) return true;
            //}
            return false;
        }

        #endregion


        #region Assetbundle
        /// <summary>
        /// Load all items from the asset bundle into the index.
        /// </summary>
        /// <param name="ab"></param>
        public void AddFromAssetBundle(AssetBundle ab)
        {
            //foreach (Type t in Types)
            //{
            //    var objs = ab.LoadAllAssets(t);
            //    foreach (UnityEngine.Object o in objs)
            //    {
            //        ProcessNewItem(o, false, false);
            //    }
            //}
        }

        /// <summary>
        /// Load all items from the asset bundle into the index.
        /// </summary>
        /// <param name="ab"></param>
        public void UnloadBundle(AssetBundle ab)
        {
            //foreach (Type t in Types)
            //{
            //    var objs = ab.LoadAllAssets(t);

            //    foreach (UnityEngine.Object o in objs)
            //    {
            //        RemoveItem(o);
            //    }
            //}
        }
        #endregion

        #region other
        public void UpdateReferences()
        {
            // Rebuild the tables
            //UpdateSerializedList();
            //foreach (AssetItem ai in SerializedItems)
            //{
            //    if (!ai.IsAddressable)
            //    {
            //        ai.CacheSerializedItem();
            //    }
            //    else
            //    {
            //        ai.FreeReference();
            //    }
            //}
            //ForceSave();
        }

        /// <summary>
        /// releases an asset an asset reference
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Name"></param>
        public void ReleaseReference(UnityEngine.Object obj)
        {
            //string Name = AssetItem.GetEvilName(obj);

            //// Leave if this is an unreferenced type - for example, a texture (etc).
            //// This can happen because these are referenced by the Overlay.
            //if (!TypeToLookup.ContainsKey(obj.GetType()))
            //    return;

            //System.Type theType = TypeToLookup[obj.GetType()];

            //Dictionary<string, AssetItem> TypeDic = GetAssetDictionary(theType);

            //if (TypeDic.ContainsKey(Name))
            //{
            //    AssetItem ai = TypeDic[Name];
            //    ai.ReleaseItem();
            //}
        }

        /// <summary>
        /// This is the evil version of AddAsset. This version cares not for the good of the project, nor
        /// does it care about readability, expandibility, and indeed, hates goodness with every beat of it's 
        /// tiny evil shrivelled heart. 
        /// I started going down the good path - I created an interface to get the name info, added it to all the
        /// classes. Then we ran into RuntimeAnimatorController. I would have had to wrap it. And Visual Studio kept
        /// complaining about the interface, even though Unity thought it was OK.
        /// 
        /// So in the end, good was defeated. And would never raise it's sword in the pursuit of chivalry again.
        /// 
        /// And EvilAddAsset doesn't save either. You have to do that manually. 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="o"></param>
        /// <returns>Whether the Asset was added or not.</returns>
        public bool EvilAddAsset(System.Type type, UnityEngine.Object o)
        {
            //AssetItem ai = null;
            //ai = new AssetItem(TypeToLookup[type], o);
            //ai._Path = AssetDatabase.GetAssetPath(o.GetInstanceID());
            //return AddAssetItem(ai);
            return false;
        }

        public bool IsAdditionalIndexedType(string QualifiedName)
        {
            foreach (string s in IndexedTypeNames)
            {
                if (s == QualifiedName)
                    return true;
            }
            return false;
        }

        public void RemoveType(System.Type sType)
        {
            //string QualifiedName = sType.AssemblyQualifiedName;
            //if (!IsAdditionalIndexedType(QualifiedName)) return;

            //TypeToLookup.Remove(sType);

            //List<System.Type> newTypes = new List<System.Type>();
            //newTypes.AddRange(Types);
            //newTypes.Remove(sType);
            //Types = newTypes.ToArray();
            //TypeLookup.Remove(sType);
            //IndexedTypeNames.Remove(sType.AssemblyQualifiedName);
            //BuildStringTypes();
        }

        /// <summary>
        /// returns the entire lookup dictionary for a specific type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Dictionary<string, AssetItem> GetAssetDictionary(System.Type type)
        {
            //System.Type LookupType = TypeToLookup[type];
            //if (TypeLookup.ContainsKey(LookupType) == false)
            //{
            //    TypeLookup[LookupType] = new Dictionary<string, AssetItem>();
            //}
            //return TypeLookup[LookupType];

            return null;
        }


        public bool IsIndexedType(System.Type type)
        {
            //foreach (System.Type check in TypeToLookup.Keys)
            //{
            //    if (check == type)
            //        return true;
            //}
            return false;
        }


        #endregion

#if UNITY_EDITOR
        public void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            //bool changed = false;

            //// Build a dictionary of the items by path.
            //Dictionary<string, AssetItem> ItemsByPath = new Dictionary<string, AssetItem>();
            //UpdateSerializedList();
            //foreach (AssetItem ai in SerializedItems)
            //{
            //    if (ItemsByPath.ContainsKey(ai._Path))
            //    {
            //        if (Debug.isDebugBuild)
            //            Debug.Log("Duplicate path for item: " + ai._Path);
            //        continue;
            //    }
            //    ItemsByPath.Add(ai._Path, ai);
            //}

            //// see if they moved it in the editor.
            //for (int i = 0; i < movedAssets.Length; i++)
            //{
            //    string NewPath = movedAssets[i];
            //    string OldPath = movedFromAssetPaths[i];

            //    // Check to see if this is an indexed asset.
            //    if (ItemsByPath.ContainsKey(OldPath))
            //    {
            //        changed = true;
            //        ItemsByPath[OldPath]._Path = NewPath;
            //    }
            //}

            //// Rebuild the tables
            //SerializedItems.Clear();
            //foreach (AssetItem ai in ItemsByPath.Values)
            //{
            //    // We null things out when we want to delete them. This prevents it from going back into 
            //    // the dictionary when rebuilt.
            //    if (ai == null)
            //        continue;
            //    SerializedItems.Add(ai);
            //}

            //UpdateSerializedDictionaryItems();
            //if (changed)
            //{
            //    ForceSave();
            //}
        }

        /// <summary>
        /// Force the Index to save and reload
        /// </summary>
        public void ForceSave()
        {
            //var st = StartTimer();
            //EditorUtility.SetDirty(this);
            //AssetDatabase.SaveAssets();
            //StopTimer(st, "ForceSave");
        }
#endif
    }

}