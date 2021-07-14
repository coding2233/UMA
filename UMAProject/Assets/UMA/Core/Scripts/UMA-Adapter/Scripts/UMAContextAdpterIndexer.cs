using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMA.CharacterSystem;
using UnityEngine;
using RaceRecipes = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<UMA.UMATextRecipe>>>;
using SlotRecipes = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<UMA.UMATextRecipe>>;

namespace UMA
{
    public class UMAContextAdpterIndexer : UMAContextBase
    {
        private static AdapterResource _adapterResource;
        public static AdapterResource AdapterResource
        {
            get
            {
                if (_adapterResource == null)
                {
#if UNITY_EDITOR
                    _adapterResource = new EditorAdapterResource();
#else
                _adapterResource = new AssetBundleAdapterResource();
#endif
                }
                return _adapterResource;
            }
        }
        //RaceRecipes _raceRecipes = new RaceRecipes();
      
#pragma warning disable 618
        public override void Start()
        {
            Instance = this;
        }

        #region Get asset
        public override List<DynamicUMADnaAsset> GetAllDNA()
        {
            return AdapterResource.GetAllAssets<DynamicUMADnaAsset>();
        }

        public override RaceData[] GetAllRaces()
        {
            return AdapterResource.GetAllAssets<RaceData>().ToArray();
        }

        public override RaceData[] GetAllRacesBase()
        {
            return GetAllRaces();
        }

        public override RuntimeAnimatorController GetAnimatorController(string Name)
        {
            return AdapterResource.GetAsset<RuntimeAnimatorController>(name);
        }

        public override UMATextRecipe GetRecipe(string filename, bool dynamicallyAdd=true)
        {
            UMATextRecipe recipe = AdapterResource.GetAsset<UMAWardrobeRecipe>(filename);
            if(recipe==null)
                recipe= AdapterResource.GetAsset<UMAWardrobeCollection>(filename);
            return recipe;

        }

        public override UMARecipeBase GetBaseRecipe(string filename, bool dynamicallyAdd)
        {
            return GetRecipe(filename,dynamicallyAdd);
        }

        public override string GetCharacterRecipe(string filename)
        {
            return "";
        }


        public override List<string> GetRecipeFiles()
        {
            return null;
        }


        public override DynamicUMADnaAsset GetDNA(string Name)
        {
            return AdapterResource.GetAsset<DynamicUMADnaAsset>(Name);
        }

        public override RaceData GetRace(string name)
        {
            return AdapterResource.GetAsset<RaceData>(name);
        }

        public override RaceData GetRace(int nameHash)
        {
            throw new System.NotImplementedException("GetRace(int nameHash)");
            //return _adapterResource.GetAsset<RaceData>(nameHash);
        }

        public override RaceData GetRaceWithUpdate(int nameHash, bool allowUpdate)
        {
            throw new System.NotImplementedException("UMAGlobalContext.GetRaceWithUpdate");
        }



        public override List<string> GetRecipeNamesForRaceSlot(string race, string slot)
        {
            //Start with recipes that are directly marked for this race.
           HashSet<string> results = internalGetRecipeNamesForRaceSlot(race, slot);

           RaceData rc = AdapterResource.GetAsset<RaceData>(race);
            if (rc != null)
            {
                foreach (string CompatRace in rc.GetCrossCompatibleRaces())
                {
                    results.UnionWith(internalGetRecipeNamesForRaceSlot(CompatRace, slot));
                }
            }

            return results.ToList();
            //throw new System.NotImplementedException();
        }

        private HashSet<string> internalGetRecipeNamesForRaceSlot(string race, string slot)
        {
            RaceRecipes raceRecipes = new RaceRecipes();
            var wardrobe = AdapterResource.GetAllAssets<UMAWardrobeRecipe>();
            foreach (var uwr in wardrobe)
            {
                foreach (string racename in uwr.compatibleRaces)
                {
                    if (!raceRecipes.ContainsKey(racename))
                    {
                        raceRecipes.Add(racename, new SlotRecipes());
                    }
                    SlotRecipes sl = raceRecipes[racename];
                    if (!sl.ContainsKey(uwr.wardrobeSlot))
                    {
                        sl.Add(uwr.wardrobeSlot, new List<UMATextRecipe>());
                    }
                    if (!sl[uwr.wardrobeSlot].Contains(uwr))//, req))
                    {
                        sl[uwr.wardrobeSlot].Add(uwr);
                    }
                }

            }
            HashSet<string> results = new HashSet<string>();

            if (raceRecipes.ContainsKey(race))
            {
                SlotRecipes sr = raceRecipes[race];
                if (sr.ContainsKey(slot))
                {
                    foreach (UMAWardrobeRecipe uwr in sr[slot])
                    {
                        results.Add(uwr.name);
                    }
                }
            }
            return results;
        }

        public override Dictionary<string, List<UMATextRecipe>> GetRecipes(string raceName)
        {
            RaceRecipes raceRecipes = new RaceRecipes();
            var wardrobe = AdapterResource.GetAllAssets<UMAWardrobeRecipe>();
            foreach (var uwr in wardrobe)
            {
                foreach (string racename in uwr.compatibleRaces)
                {
                    if (!raceRecipes.ContainsKey(racename))
                    {
                        raceRecipes.Add(racename, new SlotRecipes());
                    }
                    SlotRecipes sl = raceRecipes[racename];
                    if (!sl.ContainsKey(uwr.wardrobeSlot))
                    {
                        sl.Add(uwr.wardrobeSlot, new List<UMATextRecipe>());
                    }
                    if (!sl[uwr.wardrobeSlot].Contains(uwr))//, req))
                    {
                        sl[uwr.wardrobeSlot].Add(uwr);
                    }
                }
            }
            
            Dictionary<string, HashSet<UMATextRecipe>> aggregate = new Dictionary<string, HashSet<UMATextRecipe>>();


            if (raceRecipes.ContainsKey(raceName))
            {
                SlotRecipes sr = raceRecipes[raceName];

                foreach (KeyValuePair<string, List<UMATextRecipe>> kp in sr)
                {
                    if (!aggregate.ContainsKey(kp.Key))
                    {
                        aggregate.Add(kp.Key, new HashSet<UMATextRecipe>());
                    }
                    aggregate[kp.Key].UnionWith(kp.Value);
                }
            }


            RaceData rc = AdapterResource.GetAsset<RaceData>(raceName);
            if (rc != null)
            {
                foreach (string CompatRace in rc.GetCrossCompatibleRaces())
                {
                    if (raceRecipes.ContainsKey(raceName))
                    {
                        SlotRecipes sr = raceRecipes[raceName];

                        foreach (KeyValuePair<string, List<UMATextRecipe>> kp in sr)
                        {
                            if (!aggregate.ContainsKey(kp.Key))
                            {
                                aggregate.Add(kp.Key, new HashSet<UMATextRecipe>());
                            }
                            aggregate[kp.Key].UnionWith(kp.Value);
                        }
                    }
                }
            }

            SlotRecipes results = new SlotRecipes();
            foreach (KeyValuePair<string, HashSet<UMATextRecipe>> kp in aggregate)
            {
                results.Add(kp.Key, kp.Value.ToList());
            }

            return results;
        }

        public override List<UMARecipeBase> GetRecipesForRaceSlot(string race, string slot)
        {
            // This will get the aggregate for all compatible races with no duplicates.
            List<string> recipes = GetRecipeNamesForRaceSlot(race, slot);

            // Build a list of recipes to return.
            List<UMARecipeBase> results = new List<UMARecipeBase>();

            foreach (string recipeName in recipes)
            {
                UMAWardrobeRecipe uwr = AdapterResource.GetAsset<UMAWardrobeRecipe>(recipeName);
                if (uwr != null)
                {
                    results.Add(uwr);
                }
            }
            return results;
        }

        public override List<RuntimeAnimatorController> GetAllAnimatorControllers()
        {
            return AdapterResource.GetAllAssets<RuntimeAnimatorController>();
        }

        #endregion

        #region Has asset


        public override bool HasOverlay(string name)
        {
            return AdapterResource.GetAsset<OverlayDataAsset>(name) != null;
        }

        public override bool HasOverlay(int nameHash)
        {
            return AdapterResource.GetAsset<OverlayDataAsset>(nameHash) != null;
        }

        public override RaceData HasRace(string name)
        {
            return AdapterResource.GetAsset<RaceData>(name);
        }

        public override RaceData HasRace(int nameHash)
        {
            return AdapterResource.GetAsset<RaceData>(nameHash);
        }

        public override bool HasRecipe(string Name)
        {
            bool found = AdapterResource.GetAsset<UMAWardrobeRecipe>(Name)!=null;
            if (!found)
                found = AdapterResource.GetAsset<UMAWardrobeCollection>(Name)!=null;

            return found;
        }

        public override bool HasSlot(string name)
        {
            return AdapterResource.GetAsset<SlotDataAsset>(name)!=null;
        }

        public override bool HasSlot(int nameHash)
        {
            return AdapterResource.GetAsset<SlotDataAsset>(nameHash) != null;

        }

        public override bool CheckRecipeAvailability(string recipeName)
        {
            return AdapterResource.GetAsset<UMAWardrobeRecipe>(recipeName) != null;
        }

        #endregion

        #region Insance asset

        public override OverlayData InstantiateOverlay(string name)
        {
            OverlayDataAsset source = AdapterResource.GetAsset<OverlayDataAsset>(name);
            if (source == null)
            {
                throw new UMAResourceNotFoundException("UMAGlobalContext: Unable to find OverlayDataAsset: " + name);
            }
            return new OverlayData(source);
        }

        public override OverlayData InstantiateOverlay(int nameHash)
        {
            OverlayDataAsset source = AdapterResource.GetAsset<OverlayDataAsset>(nameHash);
            if (source == null)
            {
                throw new UMAResourceNotFoundException("UMAGlobalContext: Unable to find OverlayDataAsset: " + nameHash);
            }
            return new OverlayData(source);
        }

        public override OverlayData InstantiateOverlay(string name, Color color)
        {
            OverlayData res = InstantiateOverlay(name);
            res.colorData.color = color;
            return res;
        }

        public override OverlayData InstantiateOverlay(int nameHash, Color color)
        {
            OverlayData res = InstantiateOverlay(nameHash);
            res.colorData.color = color;
            return res;
        }

        public override SlotData InstantiateSlot(string name)
        {
            SlotDataAsset source = AdapterResource.GetAsset<SlotDataAsset>(name);
            if (source == null)
            {
                throw new UMAResourceNotFoundException("UMAGlobalContext: Unable to find SlotDataAsset: " + name);
            }
            return new SlotData(source);
        }

        public override SlotData InstantiateSlot(int nameHash)
        {
            SlotDataAsset source =  UMAContextAdpterIndexer.AdapterResource.GetAsset<SlotDataAsset>(nameHash);
            if (source == null)
            {
                throw new UMAResourceNotFoundException("UMAGlobalContext: Unable to find SlotDataAsset: " + nameHash);
            }
            return new SlotData(source);
        }

        public override SlotData InstantiateSlot(string name, List<OverlayData> overlayList)
        {
            SlotData res = InstantiateSlot(name);
            res.SetOverlayList(overlayList);
            return res;
        }

        public override SlotData InstantiateSlot(int nameHash, List<OverlayData> overlayList)
        {
            SlotData res = InstantiateSlot(nameHash);
            res.SetOverlayList(overlayList);
            return res;
        }
        #endregion

        #region Add asset

        public override void AddOverlayAsset(OverlayDataAsset overlay)
        {
            throw new System.NotImplementedException();
        }

        public override void AddRace(RaceData race)
        {
            throw new System.NotImplementedException();
        }

        public override void AddRecipe(UMATextRecipe recipe)
        {
            throw new System.NotImplementedException();
        }

        public override void AddSlotAsset(SlotDataAsset slot)
        {
            throw new System.NotImplementedException();
        }
        #endregion


        #region other
   

        public override void EnsureRaceKey(string name)
        {
            throw new System.NotImplementedException();
        }

        public override void ValidateDictionaries()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }

}