using UnityEditor;
using UnityEngine;

namespace ReneVerse
{
    public class ReneAPICreds : ScriptableObject
    {
        [ReadOnlyInspector]public string APIKey;
        [ReadOnlyInspector]public string PrivateKey;
        [ReadOnlyInspector]public string GameID;

        private class ReadOnlyInspectorAttribute : PropertyAttribute {}
        /// <summary>
        /// Allows ReadOnly public fields in inspector
        /// </summary>
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(ReadOnlyInspectorAttribute))]
        private class ReadOnlyInspectorDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label);
                GUI.enabled = true;
            }
        }
#endif
    
    }
}

