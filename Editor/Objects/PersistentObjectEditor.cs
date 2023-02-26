using StoryFramework.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor
{
	//[CustomEditor(typeof(PersistentObject)), CanEditMultipleObjects]
	public class PersistentObjectEditor : UnityEditor.Editor
	{
		static readonly GUIContent customIdentifierLabel = new("Custom Identifier");
		static readonly GUIContent oldCustomIdentifierLabel = new("Old Custom Identifier");

		public override void OnInspectorGUI()
		{
			var activeOnStartProp = serializedObject.FindProperty("activeOnStart");
			var oldCustomIdentifierProp = serializedObject.FindProperty("customIdentifier");
			var useCustomIdentifierProp = serializedObject.FindProperty("useCustomIdentifier");
			var isActiveStateProp = serializedObject.FindProperty("isActiveState");
			var customIdentifierProp =
				isActiveStateProp.GetGameStateIdentifierProperty().FindPropertyRelative("Identifier");

			serializedObject.Update();

			EditorGUILayout.PropertyField(activeOnStartProp);
			EditorGUILayout.PropertyField(useCustomIdentifierProp);
			if (useCustomIdentifierProp.boolValue)
			{
				EditorGUILayout.PropertyField(customIdentifierProp, customIdentifierLabel);
				EditorGUI.BeginDisabledGroup(true);
				EditorGUILayout.PropertyField(oldCustomIdentifierProp, oldCustomIdentifierLabel);
				EditorGUI.EndDisabledGroup();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}