using System;
using System.Collections;
using ReneVerse;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReneVerse.Demo
{
    public class ReneverseUIExample : MonoBehaviour
    {
        private const string EmailFail = "Type Correct Email";
        private const string Connect = "Connect";
        private const string Connected = "Connected!";

        [SerializeField] private ReneVerseServiceExample _reneVerseServiceExample;

        [Header("ReneVerse Connect Button")] [SerializeField]
        private Button reneVerseConnect;

        [SerializeField] private TextMeshProUGUI connectButtonText;
        [SerializeField] private TextMeshProUGUI reneOutput;


        [Header("ReneVerse Email Input Field")] [SerializeField]
        private TMP_InputField reneVerseEmail;

        private Coroutine _connectingDotsCoroutine;

        private bool IsNotValidEmail => !reneVerseEmail.text.IsEmail();
        private bool EmptyEmailField => string.IsNullOrEmpty(reneVerseEmail.text);

        private void Disable(Behaviour behaviour) => behaviour.enabled = false;


        #region Enable/Disable

        private void OnEnable()
        {
            reneVerseConnect.onClick.AddListener(ReneVerseConnectClicked);
        }

        private void OnDisable()
        {
            reneVerseConnect.onClick.RemoveListener(ReneVerseConnectClicked);
        }

        #endregion

        private void ReneVerseConnectClicked()
        {
            if (IsNotValidEmail)
            {
                ChangeText(connectButtonText, EmailFail);
                return;
            }

            Disable(reneVerseConnect);
            Disable(reneVerseEmail);


            _connectingDotsCoroutine = StartCoroutine(ConnectingDotsAnimation(connectButtonText));

            //Here happens the connection itself
            _reneVerseServiceExample.ReneVerseConnectClicked(reneVerseEmail.text, OnReneConnected);
        }

        private void OnReneConnected(string reneAssetsData)
        {
            StopCoroutine(_connectingDotsCoroutine);
            reneOutput.text += reneAssetsData + "\n";
            connectButtonText.text = Connected;
        }

        private void ChangeText(TextMeshProUGUI textMeshProUGUI, string text) => textMeshProUGUI.text = text;


        private IEnumerator ConnectingDotsAnimation(TextMeshProUGUI textMeshProUGUI)
        {
            while (true)
            {
                textMeshProUGUI.text = Connect;
                yield return new WaitForSeconds(0.3f);
                textMeshProUGUI.text = textMeshProUGUI.text.AddPoint();
                yield return new WaitForSeconds(0.3f);
                textMeshProUGUI.text = textMeshProUGUI.text.AddPoint();
                yield return new WaitForSeconds(0.3f);
                textMeshProUGUI.text = textMeshProUGUI.text.AddPoint();
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}