using System;
using StoryFramework.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor
{
	//[CustomPropertyDrawer(typeof(GameStateValue))]
	public class GameStateValueDrawer : UnityEditor.PropertyDrawer
	{
		GUIContent m_ValueContent = new("Value");

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var rect = position;
			var typeProp = property.FindPropertyRelative("Type");
			var valueProp = property.GetGameStateValueProp((GameStateTypes)typeProp.enumValueIndex);

			// Draw fields. 
			rect.height = EditorGUI.GetPropertyHeight(typeProp);  
			EditorGUI.PropertyField(rect, typeProp);
			rect.y += rect.height;

			rect.height = EditorGUI.GetPropertyHeight(valueProp);  
			EditorGUI.PropertyField(rect, valueProp, m_ValueContent);
			rect.y += rect.height;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var typeProp = property.FindPropertyRelative("Type");
			var valueProp = property.GetGameStateValueProp((GameStateTypes)typeProp.enumValueIndex);

			float height = 0.0f;
			height += EditorGUI.GetPropertyHeight(typeProp);
			height += EditorGUI.GetPropertyHeight(valueProp);
			return height; 
		}
	}
}