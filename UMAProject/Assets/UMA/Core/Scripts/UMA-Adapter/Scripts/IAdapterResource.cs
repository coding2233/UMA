using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UMA
{
    public interface IAdapterResource
    {


        List<T> GetAllAssets<T>(string[] foldersToSearch = null) where T : UnityEngine.Object;


        T GetAsset<T>(string name) where T : UnityEngine.Object;
    }
}
