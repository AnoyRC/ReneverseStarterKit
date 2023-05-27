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
    public static Dictionary<Asset, int> NFTCounter = new();
    public static string EmailHandler;
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

    API ReneAPI;
    
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

    public async void SignIn()
    {
        await ConnectUser();
    }

    public void SignUp()
    {
        Application.OpenURL("https://app.reneverse.io/register");
    }

    //Connect User function
    async Task ConnectUser()
    {
        SignInButton.SetActive(false);

        ReneAPI = ReneAPIManager.API();
        
        EmailHandler = Email.GetComponent<TMP_InputField>().text;

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
            Asset thisAsset = new()
            {
                AssetName = asset.Metadata.Name,
                Description = asset.Metadata.Description,
                AssetUrl = asset.Metadata.Image,
                TemplateID = asset.AssetTemplateId
            };

            NFTCounter[thisAsset]++;
        });
    }
}

public class Asset
{
    public string AssetName { get; set; }
    public string Description { get; set; }
    public string AssetUrl { get; set; }
    public string TemplateID { get; set; }
}