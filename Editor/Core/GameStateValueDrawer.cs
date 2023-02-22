using System;
using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor
{
	[CustomPropertyDrawer(typeof(GameStateValue))]
	public class GameStateValueDrawer : UnityEditor.PropertyDrawer
	{
		GUIContent m_ValueContent = new("Value");

		static SerializedProperty GetValueProp(SerializedProperty property, GameStateTypes type)
		{
			switch (type)
			{
			case GameStateTypes.BooleanFlag:
				return property.FindPropertyRelative("m_BooleanValue");
			case GameStateTypes.IntegerNumber:
				return property.FindPropertyRelative("m_IntegerValue");
			case GameStateTypes.FloatNumber:
				return property.FindPropertyRelative("m_FloatValue");
			case GameStateTypes.Text:
				return property.FindPropertyRelative("m_TextValue");
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var rect = position;
			var typeProp = property.FindPropertyRelative("Type");

			// Draw fields. 
			rect.height = EditorGUI.GetPropertyHeight(typeProp);  
			EditorGUI.PropertyField(rect, typeProp);
			rect.y += rect.height;

			var valueProp = GetValueProp(property, (GameStateTypes)typeProp.enumValueIndex);
			rect.height = EditorGUI.GetPropertyHeight(valueProp);  
			EditorGUI.PropertyField(rect, valueProp, m_ValueContent);
			rect.y += rect.height;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var typeProp = property.FindPropertyRelative("Type");
			var valueProp = GetValueProp(property, (GameStateTypes)typeProp.enumValueIndex);

			float height = EditorGUI.GetPropertyHeight(typeProp);
			height += EditorGUI.GetPropertyHeight(valueProp);
			return height; 
		}
	}
}