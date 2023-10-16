using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[Serializable]
public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudioClip(string guid) : base(guid)
    {
    }
}

public class ObjLoader : MonoBehaviour
{
    //public GameObject prefab;
    //public string path;
    //public AssetReference assetReference;
    public AssetReferenceGameObject assetReferenceGameObject;
    
    public AssetReferenceAudioClip assetRefAudioClip;
    public AsyncOperationHandle<GameObject> handleObj;


    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(prefab); 
        //Instantiate(Resources.Load<GameObject>(path));

        //Addressables.LoadAssetAsync<GameObject>(path).Completed += LoadAsset;
        //assetReference.LoadAssetAsync<GameObject>().Completed += LoadAsset;
        //assetReferenceGameObject.LoadAssetAsync<GameObject>().Completed += LoadAsset;
        handleObj = assetReferenceGameObject.InstantiateAsync();
    }
    /*
    private void LoadAsset(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(handle.Result);
        }
        else
        {
            Debug.LogError("Asset couldn't loaded");
        }
    }*/

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Destroy(handleObj.Result);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            handleObj = assetReferenceGameObject.InstantiateAsync();
        }
    }
}
