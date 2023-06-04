using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ReneverseTransfer : MonoBehaviour
{
    ReneverseAssetManager assetManager;
    void Start()
    {
        assetManager = ReneverseAssetManager.Instance;
        gameObject.GetComponent<Button>().onClick.AddListener(Transfer);
    }

    #region Button Action
    public async void Transfer()
    {
        await TransferAsset();
    }
    #endregion

    //Transfer Asset by Fetching Selected User and Selected Asset 
    //Dependencies : Reneverse User Manager , Renverse Asset Manager && Reneverse Manager
    public async Task TransferAsset()
    {
        try
        {
            if (ReneverseAssetManager.SelectedAsset.NFTId == null || ReneverseUserManager.SelectedUser.UserId == null) return;

            var response = await ReneverseManager.ReneAPI.Game().AssetTransfer(ReneverseAssetManager.SelectedAsset.NFTId, ReneverseUserManager.SelectedUser.UserId);
            Debug.Log(response);

            #region Destroy the Asset
            Destroy(AssetPrefabManager.SelectedAsset);

            foreach(Asset asset in ReneverseManager.NFTCounter)
            {
                if(asset.NFTId == ReneverseAssetManager.SelectedAsset.NFTId)
                {
                    ReneverseManager.NFTCounter.Remove(asset);
                    break;
                }
            }

            assetManager.SelectAsset(null, null, null, null, null);
            #endregion

            Debug.Log("Transfer in Progress");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
