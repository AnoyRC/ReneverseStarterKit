using Rene.Sdk;
using UnityEngine;

namespace ReneVerse
{
    public class ReneAPIManager
    {
        private static API _api;
        private static ReneAPICreds _reneAPICreds;
        
        public static API API()
        {
            _reneAPICreds ??= (ReneAPICreds)Resources.Load(nameof(ReneAPICreds), typeof(ScriptableObject));
            return _api ??= Rene.Sdk.API.Init(_reneAPICreds.APIKey, _reneAPICreds.PrivateKey, _reneAPICreds.GameID);
        }
    }
}