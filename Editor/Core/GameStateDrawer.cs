using System;
using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor
{
	[CustomPropertyDrawer(typeof(GameState))]
	public class GameStateDrawer : UnityEditor.PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var rect = position;
			var identifierProp = property.FindPropertyRelative("Identifier");
			var valueProp = property.FindPropertyRelative("Value");

			// Draw fields. 
			rect.height = EditorGUI.GetPropertyHeight(identifierProp);  
			EditorGUI.PropertyField(rect, identifierProp);
			rect.y += rect.height;

			rect.height = EditorGUI.GetPropertyHeight(valueProp);  
			EditorGUI.PropertyField(rect, valueProp);
			rect.y += rect.height;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var identifierProp = property.FindPropertyRelative("Identifier");
			var valueProp = property.FindPropertyRelative("Value");

			float height = EditorGUI.GetPropertyHeight(identifierProp);
			height += EditorGUI.GetPropertyHeight(valueProp);
			return height; 
		}
	}
}