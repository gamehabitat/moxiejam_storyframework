using System;
using UnityEditor;
using GameStateSetter = StoryFramework.Utilities.GameStateSetter;

namespace StoryFramework.Editor.Utilities
{
    [CustomEditor(typeof(GameStateSetter))]
    [CanEditMultipleObjects]
    public class GameStateSetterEditor : UnityEditor.Editor
    {
        SerializedProperty setStateOnStartProp;
        SerializedProperty stateTypeProp;
        SerializedProperty idProp;
        SerializedProperty boolValueProp;
        SerializedProperty intValueProp;
        SerializedProperty floatValueProp;
        SerializedProperty stringValueProp;

        protected void OnEnable()
        {
            setStateOnStartProp = serializedObject.FindProperty("setStateOnStart");
            stateTypeProp = serializedObject.FindProperty("stateType");
            idProp = serializedObject.FindProperty("id");
            boolValueProp = serializedObject.FindProperty("boolValue");
            intValueProp = serializedObject.FindProperty("intValue");
            floatValueProp = serializedObject.FindProperty("floatValue");
            stringValueProp = serializedObject.FindProperty("stringValue");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(setStateOnStartProp);
            EditorGUILayout.PropertyField(stateTypeProp);
            EditorGUILayout.PropertyField(idProp);
            switch ((GameStateSetter.Types)stateTypeProp.enumValueIndex)
            {
            case GameStateSetter.Types.Bool:
                EditorGUILayout.PropertyField(boolValueProp);
                break;
            case GameStateSetter.Types.Int:
                EditorGUILayout.PropertyField(intValueProp);
                break;
            case GameStateSetter.Types.Float:
                EditorGUILayout.PropertyField(floatValueProp);
                break;
            case GameStateSetter.Types.String:
                EditorGUILayout.PropertyField(stringValueProp);
                break;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}