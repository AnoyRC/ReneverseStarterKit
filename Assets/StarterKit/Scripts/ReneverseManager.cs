using Rene.Sdk;
using Rene.Sdk.Api.Game.Data;
using ReneVerse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReneverseManager : MonoBehaviour
{
    #region Static Fields
    public static bool LoginStatus = false;
    public static bool IsAssetLoaded = false;

    public static List<Asset> NFTCounter = new();
    public static API ReneAPI;
    #endregion

    [Header("Sign In Properties")]
    public GameObject SignInPanel;
    public GameObject Email;
    public GameObject SignInButton;
    public Button SignUpButton;

    [Header("Verification Properties")]
    public GameObject CountdownPanel;
    public TextMeshProUGUI Timer;
    public int TimeToWait = 30;

    void Start()
    {
        SignInButton.GetComponent<Button>().onClick.AddListener(SignIn);
        if(SignUpButton) SignUpButton.onClick.AddListener(SignUp);

        if (LoginStatus)
        {
            SignInPanel.SetActive(false);
            CountdownPanel.SetActive(false);
        }
    }

    #region Button Actions
    public async void SignIn()
    {
        await ConnectUser();
    }

    public void SignUp()
    {
        Application.OpenURL("https://app.reneverse.io/register");
    }
    #endregion

    //Connect User function
    async Task ConnectUser()
    {
        SignInButton.SetActive(false);

        ReneAPI = ReneAPIManager.API();
        
        string EmailHandler = Email.GetComponent<TMP_InputField>().text;

        if (!EmailHandler.IsEmail())
        {
            Debug.Log("Please provide a valid Email");
            SignInButton.SetActive(true);
            return;
        }

        bool connected = await ReneAPI.Game().Connect(EmailHandler);
        Debug.Log(connected);

        if (!connected)
        {
            Debug.Log("Error Connecting");
            SignInButton.SetActive(true);
            return;
        }

        StartCoroutine(ConnectReneService(ReneAPI));
    }

    //Function to check, if the user is authorizing through Reneverse Dashboard
    private IEnumerator ConnectReneService(API reneApi)
    {
        CountdownPanel.SetActive(true);
        var counter = TimeToWait;
        var userConnected = false;
        //Interval how often the code checks that user accepted to log in
        var secondsToDecrement = 1;
        while (counter >= 0 && !userConnected)
        {
            Timer.text = counter.ToString();
            if (reneApi.IsAuthorized())
            {

                CountdownPanel.SetActive(false);
                SignInPanel.SetActive(false);

                yield return GetUserAssetsAsync(reneApi);


                userConnected = true;
                LoginStatus = true;
            }

            yield return new WaitForSeconds(secondsToDecrement);
            counter -= secondsToDecrement;
        }
        CountdownPanel.SetActive(false);
        SignInButton.SetActive(true);
    }

    //Get all the NFTs owned by the user in this game
    private async Task GetUserAssetsAsync(API reneApi)
    {
        AssetsResponse.AssetsData userAssets = await reneApi.Game().Assets();
        userAssets?.Items.ForEach(asset =>
        {
            Asset thisAsset = new(asset.Metadata.Name, asset.Metadata.Description, asset.Metadata.Image, asset.AssetTemplateId);

            NFTCounter.Add(thisAsset);

        });
        IsAssetLoaded = true;
    }
}

//Asset Class
[Serializable]
public class Asset
{
    public string AssetName;
    public string Description;
    public string AssetUrl;
    public string TemplateID;
    public Asset(string assetName, string description, string assetUrl, string templateID)
    {
        AssetName = assetName;
        Description = description;
        AssetUrl = assetUrl;
        TemplateID = templateID;
    }
}