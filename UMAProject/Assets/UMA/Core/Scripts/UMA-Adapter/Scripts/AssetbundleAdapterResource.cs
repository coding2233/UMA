using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UMA
{
    public class AssetBundleAdapterResource : AdapterResource
    {
        public override List<T> GetAllAssets<T>(string[] foldersToSearch = null)
        {
            return null;
        }

        public override T GetAsset<T>(string name)
        {
            return null;
        }
    }


#if UNITY_EDITOR

    public class AssetBundleAdapterResourceEditor
    {
        [MenuItem("UMA-Adapter/Build-AssetBundle")]
        static void main()
        {
            //foreach (var assetType in _assetTypes)
            //{
            //    List<UnityEngine.Object> @objects = new List<UnityEngine.Object>();
            //    _allTypeAssets.Add(assetType, @objects);

            //    var findAssets = AssetDatabase.FindAssets($"t:{assetType.Name}", new string[] { _assetRootPath });
            //    foreach (var item in findAssets)
            //    {
            //        string assetPath = AssetDatabase.GUIDToAssetPath(item);
            //        var @object = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            //        if (!_allAssets.ContainsKey(assetPath))
            //        {
            //            _allAssets.Add(assetPath, @object);
            //            _allAssetPath.Add(assetPath);
            //        }
            //        @objects.Add(@object);
            //        //Debug.Log($"[Find asset]: {assetPath} ### [type]: {@object.GetType()}");
            //    }
            //}
            //UnityEditor.BuildPipeline
        }
    }


#endif

}