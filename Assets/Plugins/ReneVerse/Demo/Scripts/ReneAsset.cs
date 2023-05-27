using System;
using UnityEngine;

namespace ReneVerse.Demo
{
    [Serializable]
    public class ReneAsset
    {
        [SerializeField] private string _name;
        [SerializeField] private string _class;
        [SerializeField] private float _speedBoost;
        [SerializeField] private int _color;
        [SerializeField] private string _productPlacement;
        [SerializeField] private string _carImageURL;
    
        public string CarImageURL => _carImageURL;
        public string Class => _class;

        public float SpeedBoost => _speedBoost;

        public string ProductPlacement => _productPlacement;

        public int Color => _color;

        public string Name => _name;

        public ReneAsset(string name, string productPlacement, string @class, float speedBoost, int color,
            string carImageURL)
        {
            _name = name;
            _productPlacement = productPlacement;
            _class = @class;
            _speedBoost = speedBoost;
            _color = color;
            _carImageURL = carImageURL;
        }
    }
}