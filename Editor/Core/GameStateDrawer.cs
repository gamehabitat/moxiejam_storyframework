using System;
using StoryFramework.Editor.Core;
using StoryFramework.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor
{
	[CustomPropertyDrawer(typeof(GameState))]
	public class GameStateDrawer : PropertyDrawerBase
	{
		GUIContent m_ValueContent = new("Value");
		
		protected override void OnGuiInternal(Rect position, SerializedProperty property, GUIContent label)
		{
			DrawIdentifier(position, property, label);
		}

		public void DrawIdentifier(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var rect = position;
			var identifierProp = property.FindPropertyRelative("Identifier");
			var identifier = identifierProp.GetGameStateIdentifierValue();
			var valueProp = property.GetGameStateValueProp(identifier.Type);

			bool haveGameStateType = TryGetAttribute<GameStateTypeAttribute>(out var gameStateType);  
			if (haveGameStateType)
			{
				identifier.Type = gameStateType.Type;
				identifierProp.SetGameStateIdentifierValue(in identifier);
			}

			// Draw fields. 
			rect.height = GameStateIdentifierDrawer.GetIdentifierHeight(this, identifierProp, new GUIContent(identifierProp.displayName));
			GameStateIdentifierDrawer.DrawIdentifier(this, rect, identifierProp, new GUIContent(identifierProp.displayName));
			//rect.height = EditorGUI.GetPropertyHeight(identifierProp);
			//EditorGUI.PropertyField(rect, identifierProp);
			rect.y += rect.height;

			rect.height = EditorGUI.GetPropertyHeight(valueProp);
			if (haveGameStateType && gameStateType.hasCustomLabel)
			{
				EditorGUI.PropertyField(rect, valueProp, new GUIContent(gameStateType.Label));
			}
			else
			{
				EditorGUI.PropertyField(rect, valueProp, m_ValueContent);
			}
			rect.y += rect.height;

			EditorGUI.EndProperty();
		}

		protected override float GetPropertyHeightInternal(SerializedProperty property, GUIContent label)
		{
			return GetIdentifierHeight(property, label); 
		}

		public float GetIdentifierHeight(SerializedProperty property, GUIContent label)
		{
			var identifierProp = property.FindPropertyRelative("Identifier");
			var identifier = identifierProp.GetGameStateIdentifierValue();
			var valueProp = property.GetGameStateValueProp(identifier.Type);

			float height = 0.0f;
			height += GameStateIdentifierDrawer.GetIdentifierHeight(this, identifierProp, new GUIContent(identifierProp.displayName));
			//height += EditorGUI.GetPropertyHeight(identifierProp);
			height += EditorGUI.GetPropertyHeight(valueProp);

			return height;
		}
	}
}