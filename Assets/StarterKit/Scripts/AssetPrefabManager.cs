using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AssetPrefabManager : MonoBehaviour
{
    [Header("Asset Details")]
    public Asset NFT;

    [Header("UI Inputs")]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public Image image;
    public TextMeshProUGUI TemplateID;

    //Initialize the UI content
    public void Initialize(Asset asset)
    {
        #region Setting Asset Details
        NFT = asset;
        #endregion

        Name.text = asset.AssetName;
        Description.text = asset.Description;
        TemplateID.text = asset.TemplateID;
        StartCoroutine(FetchImage(asset.AssetUrl));
    }

    //Fetching Image Texture from URL
    IEnumerator FetchImage(string ImageLink)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(ImageLink);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(.5f, .5f));

            image.overrideSprite = newSprite;
        }
    }
}