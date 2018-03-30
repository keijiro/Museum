using UnityEngine;
using UnityEditor;

namespace Museum
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PostProcessOut))]
    public class PostProcessOutEditor : Editor
    {
        SerializedProperty _volume;

        void OnEnable()
        {
            _volume = serializedObject.FindProperty("_volume");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_volume);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
