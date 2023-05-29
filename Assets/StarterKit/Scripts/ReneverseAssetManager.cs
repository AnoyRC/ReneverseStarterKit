using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReneverseAssetManager : MonoBehaviour
{
    [Header("Parent where prefab will spawn")]
    public GameObject Panel;
    [Header("UI Prefab of the asset")]
    public GameObject AssetPrefab;

    private bool isAssetListed = false;
   
    void Update()
    {
        //Check if the assets are loaded
        if (ReneverseManager.LoginStatus && ReneverseManager.IsAssetLoaded && !isAssetListed)
        {
            LoadAssets();
            isAssetListed = true;
        }
    }

    //Load the asset class in the UI
    public void LoadAssets()
    {
        foreach (var entry in ReneverseManager.NFTCounter)
        {
            var asset = Instantiate(AssetPrefab);
            asset.transform.SetParent(Panel.transform, false);
            asset.GetComponent<AssetPrefabManager>().Initialize(entry);
        }
    }
}
