
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UMA;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.Networking;

public class DynamicLoading : MonoBehaviour
{

    [SerializeField]
    private GameObject _target;

    [SerializeField]
    private string _remoteUrl;

    private string _log;

#if !UNITY_EDITOR

    private void Awake()
    {
        //_target.SetActive(false);
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _log = "开始远程加载asset.";
        string assetAB = Path.Combine(_remoteUrl,"asset");
        UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(assetAB);
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            _log = "asset 下载失败";
        }
        else
        {
            _log = "asset下载完成.";
            var ab = DownloadHandlerAssetBundle.GetContent(uwr);
            AssetBundleManifest abm = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            List<string> allABNames = new List<string>() { "asset" };
            allABNames.AddRange(abm.GetAllAssetBundles());
            ab.Unload(true);
            yield return new WaitForEndOfFrame();
            foreach (var item in allABNames)
            {
                string downloadUrl = Path.Combine(_remoteUrl, item);
                string downloadPath = Path.Combine(Application.persistentDataPath, item);

                _log = $"正在下载:\n{downloadUrl}\n{downloadPath}";

                UnityWebRequest uwrDownload = UnityWebRequest.Get(downloadUrl);
                uwrDownload.downloadHandler = new DownloadHandlerFile(downloadPath);
                yield return uwrDownload.SendWebRequest();
            }
            _log = $"资源全部下载完成";
            yield return new WaitForEndOfFrame();
            var i = UMA.UMAAssetIndexer.Instance;
             _log = $"本地加载Assetbundle资源";
            yield return new WaitForEndOfFrame();
            _log = $"生成物体";
            var t =GameObject.Instantiate(_target);
            var dca = t.GetComponent<DynamicCharacterAvatar>();
            dca.activeRace.data = UMAAssetIndexer.Instance.GetAsset<RaceData>("HumanMale");
            dca.BuildCharacter();
            yield return new WaitForEndOfFrame();
            t.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUI.Button(new Rect(Screen.width*0.5f,Screen.height*0.5f,400,300),_log);
    }
#else
    private IEnumerator Start()
    {
        var i = UMAAssetIndexer.Instance;
        yield return new WaitForEndOfFrame();
        UMAContextBase.Instance = GameObject.FindObjectOfType<UMAGlobalContext>();
        yield return new WaitForEndOfFrame();

        var hm = UMAAssetIndexer.Instance.GetAsset<RaceData>("HumanMale");

        var t = GameObject.Instantiate(_target);
        var dca = t.GetComponent<DynamicCharacterAvatar>();
        dca.activeRace.data = UMAAssetIndexer.Instance.GetAsset<RaceData>("HumanMale");
        dca.BuildCharacter();
        t.SetActive(true);



    }
#endif

}


