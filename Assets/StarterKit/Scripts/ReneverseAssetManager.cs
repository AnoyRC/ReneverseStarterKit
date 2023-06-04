using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ReneverseAssetManager : MonoBehaviour
{
    [Header("Parent where prefab will spawn")]
    public GameObject Panel;
    [Header("UI Prefab of the asset")]
    public GameObject AssetPrefab;

    private bool isAssetListed = false;

    public static Asset SelectedAsset;

    [Header("Selected Asset")]
    [SerializeField]
    private Asset selectedAsset;

    public static ReneverseAssetManager Instance;

    void Awake()
    {
        Instance = this;
    }

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

    //Selected Asset
    public void SelectAsset(string assetName, string description, string assetUrl, string templateId, string nftId)
    {
        Asset thisAsset = new(assetName, description, assetUrl, templateId, nftId);
        SelectedAsset = thisAsset;
        Serialize();

        foreach(Transform child in Panel.transform)
        {
            child.GetComponent<AssetPrefabManager>().SelectedIndicator.SetActive(false);
        }
    }

    #region Show Selected Asset in Inspector
    public void Serialize()
    {
        selectedAsset.AssetName = SelectedAsset.AssetName;
        selectedAsset.Description = SelectedAsset.Description;
        selectedAsset.AssetUrl = SelectedAsset.AssetUrl;
        selectedAsset.TemplateID = SelectedAsset.TemplateID;
        selectedAsset.NFTId = SelectedAsset.NFTId;
    }
    #endregion
}
