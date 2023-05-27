using System;
using System.Collections;
using System.Threading.Tasks;
using Rene.Sdk;
using Rene.Sdk.Api.Game.Data;
using ReneVerse;
using UnityEngine;

namespace ReneVerse.Demo
{
    public class ReneVerseServiceExample : MonoBehaviour
    {
        [SerializeField] [Range(0, 60)] private int secondsToWait = 30;
        

        public async void ReneVerseConnectClicked(string email, Action<string> onConnectedReneverse = null)
        {
            var reneApi = ReneAPIManager.API();

            var connected = await reneApi.Game().Connect(email);
            StartCoroutine(ConnectReneService(reneApi, onConnectedReneverse));
        }

        private IEnumerator ConnectReneService(API reneApi, Action<string> onConnectedReneverse = null)
        {
            var counter = 0;
            var userConnected = false;
            var secondsToIncrement = 1;
            while (counter <= secondsToWait && !userConnected)
            {
                print($"{counter}");
                if (reneApi.IsAuthorized())
                {
                    print("User successfully connected!");
                    yield return GetUserAssetsAsync(reneApi, onConnectedReneverse);
                    userConnected = true;
                }

                yield return new WaitForSeconds(secondsToIncrement);
                counter += secondsToIncrement;
            }
        }

        private async Task GetUserAssetsAsync(API reneApi, Action<string> onConnectedReneverse = null)
        {
            AssetsResponse.AssetsData userAssets = await reneApi.Game().Assets();
            /*userAssets?.Items.ForEach
            (asset => Debug.Log
                ($" - Asset Id '{asset.NftId}' Name '{asset.Metadata.Name}"));*/


            userAssets?.Items.ForEach(asset =>
            {
                string assetName = asset.Metadata.Name;
                string assetImageUrl = asset.Metadata.Image;
                string assetStyle = "";
                asset.Metadata?.Attributes?.ForEach(attribute =>
                {
                    onConnectedReneverse?.Invoke
                        ($"TraitType {attribute.TraitType} has value of {attribute.Value}");
                });
            });
        }
    }
}