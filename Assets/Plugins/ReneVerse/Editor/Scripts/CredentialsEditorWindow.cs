using System;
using System.Reflection;
using Rene.Sdk;
using UnityEditor;
using UnityEngine;

namespace ReneVerse.Editor
{
    public class CredentialsEditorWindow : EditorWindow
    {
        private Texture2D _logo;

        #region consts

        private const string DocsURL = "https://docs.reneverse.io";
        private const string TabName = "Reneverse Settings";
        private const string LogoSprite = "ReneLogo";
        private const string InstructionText = "Enter all necessary information below to get connected to ReneVerse.";
        private const string Reminder = "Reminder: Do not forget to register at our service";
        private const string RegisterButtonName = "Need To Register?";
        private const string RegisterTip = "Click here to register for ReneVerse";
        private const string DocsCheckButtonName = "Check Out Our Docs!";
        private const string DocsTip = "Click here to view ReneVerse documentation";
        private const string GreetingMessage = "ReneVerse SDK greets you!";
        private const string ReneAPIHost = "RENE_API_HOST";
        private const string ReneProdURL = "api.reneverse.io";
        private const string ReneProdURLLink = "app.reneverse.io";

        #endregion

        private ReneAPICreds _reneAPICreds;
        private static string apiHost;


        private void Awake()
        {
            _logo = (Texture2D)Resources.Load(LogoSprite, typeof(Texture2D));
            _reneAPICreds ??= Resources.Load<ReneAPICreds>(nameof(ReneAPICreds));
        }

        [MenuItem("Window/" + TabName)]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(CredentialsEditorWindow));

            window.titleContent = new GUIContent(TabName);
        }

        void OnGUI()
        {
            CenterLogo();
            GreetMessage(GreetingMessage);

            CreateLabel(InstructionText);

            CredentialTextArea(_reneAPICreds.APIKey, nameof(_reneAPICreds.APIKey));
            CredentialTextArea(_reneAPICreds.PrivateKey, nameof(_reneAPICreds.PrivateKey));
            CredentialTextArea(_reneAPICreds.GameID, nameof(_reneAPICreds.GameID));
            
            AddURLButton(RegisterButtonName, ReneProdURLLink.ToLoginUrl(), RegisterTip);
            
            
            AddURLButton(DocsCheckButtonName, DocsURL, DocsTip);

            CreateLabel(Reminder);
        }

        private static bool IsProd()
        {
            Type apiType = typeof(API);
            FieldInfo fieldInfo =
                apiType.GetField(ReneAPIHost, BindingFlags.NonPublic | BindingFlags.Static);
            apiHost = (string)fieldInfo.GetValue(null);
            return apiHost == ReneProdURL;
        }

        private static void AddURLButton(string buttonName, string urlToOpen, string tooltip)
        {
            if (GUILayout.Button(new GUIContent(buttonName, tooltip))) Application.OpenURL(urlToOpen);
        }


        private static void CreateLabel(string labelText)
        {
            GUILayout.Label(labelText, EditorStyles.label);
        }

        /// <summary>
        /// <see cref="CredentialTextArea"/> without reflection
        /// </summary>
        /// <param name="userCredential"></param>
        /// <param name="keyName"></param>
        private void CredentialTextArea(string userCredential, string keyName)
        {
            GUIStyle wordWrapStyle = new GUIStyle(GUI.skin.textArea);
            wordWrapStyle.wordWrap = true;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(keyName.UnderscoresToSpaces(), GUILayout.Width(150));

            EditorGUI.BeginChangeCheck();

            userCredential = EditorGUILayout.TextArea(userCredential, wordWrapStyle, GUILayout.ExpandWidth(true));

            if (EditorGUI.EndChangeCheck()) SaveOnTextChanged(userCredential, keyName);

            EditorGUILayout.EndHorizontal();
        }


        private void SaveOnTextChanged(string value, string fieldName)
        {
            var fieldInfo = _reneAPICreds.GetType().GetField(fieldName);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(_reneAPICreds, value);
                EditorUtility.SetDirty(_reneAPICreds);
                AssetDatabase.SaveAssets();
            }
        }


        private static void GreetMessage(string labelText)
        {
            GUILayout.Space(20f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.fontSize = 24;
            GUILayout.Label(labelText, style);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(20f);
        }

        private void CenterLogo()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical(GUILayout.Width(_logo.width));

            GUIStyle logoBackground = new GUIStyle();
            logoBackground.padding = new RectOffset(0, 0, 0, 0);
            logoBackground.margin = new RectOffset(0, 0, 0, 0);
            logoBackground.border = new RectOffset(0, 0, 0, 0);
            logoBackground.normal.background = _logo;
            logoBackground.normal.background = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            Color grayColor = new Color(56 / 255f, 56 / 255f, 56 / 255f);
            logoBackground.normal.background.SetPixel(0, 0, grayColor);
            logoBackground.normal.background.Apply();

            GUILayout.Label(_logo, logoBackground, GUILayout.MaxWidth(_logo.width), GUILayout.MaxHeight(_logo.height));
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// <see cref="CredentialPasswordField"/> foundation for password hidden textfield 
        /// </summary>
        /// <param name="userCredential"></param>
        /// <param name="keyName"></param>
        private void CredentialPasswordField(ref string userCredential, string keyName)
        {
            GUIStyle wordWrapStyle = new GUIStyle(GUI.skin.textArea);
            wordWrapStyle.wordWrap = true;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(keyName.UnderscoresToSpaces(), GUILayout.Width(150));
            userCredential = EditorGUILayout.PasswordField(
                string.IsNullOrWhiteSpace(PlayerPrefs.GetString(keyName))
                    ? keyName.AddEnterYour().UnderscoresToSpaces()
                    : PlayerPrefs.GetString(keyName), wordWrapStyle, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            SaveOnTextChanged(keyName, userCredential);
        }
    }
}