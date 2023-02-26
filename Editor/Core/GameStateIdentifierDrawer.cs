using System;
using System.Collections.Generic;
using System.Linq;
using StoryFramework.Editor.Core;
using StoryFramework.Editor.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StoryFramework.Editor
{
	[CustomPropertyDrawer(typeof(GameStateIdentifier))]
	public class GameStateIdentifierDrawer : PropertyDrawerBase
	{
		const float popupWidth = 80.0f;
		const float spacing = 5.0f;
		
		//List<int> m_Indices = new();
		//List<GUIContent> m_Labels = new();
		GUIContent m_ValueContent = new("Value");

		protected override void OnGuiInternal(Rect position, SerializedProperty property, GUIContent label)
		{
			DrawIdentifier(this, position, property, label);
		}

		public static void DrawIdentifier(PropertyDrawerBase drawer, Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var identifierProp = property.FindPropertyRelative("Identifier");
			var propertyProp = property.FindPropertyRelative("Property");
			var typeProp = property.FindPropertyRelative("Type");

			bool haveGameStateType = drawer.TryGetAttribute<GameStateTypeAttribute>(out var gameStateType);  
			if (haveGameStateType)
			{
				typeProp.enumValueIndex = (int)gameStateType.Type;
			}

			if (drawer.TryGetAttribute<GameStateRefAttribute>(out var gameStateRef))
			{
				DrawRef(gameStateRef, position, property, label);
			}
			else
			{
				var rect = position;

				// Draw fields. 
				rect.height = EditorGUI.GetPropertyHeight(identifierProp);  
				EditorGUI.PropertyField(rect, identifierProp);
				rect.y += rect.height;

				rect.height = EditorGUI.GetPropertyHeight(propertyProp);  
				EditorGUI.PropertyField(rect, propertyProp);
				rect.y += rect.height;

				if (!haveGameStateType)
				{
					rect.height = EditorGUI.GetPropertyHeight(typeProp);  
					EditorGUI.PropertyField(rect, typeProp);
					rect.y += rect.height;
				}
			}

			EditorGUI.EndProperty();
		}

		protected override float GetPropertyHeightInternal(SerializedProperty property, GUIContent label)
		{
			return GetIdentifierHeight(this, property, label);
		}

		public static float GetIdentifierHeight(PropertyDrawerBase drawer, SerializedProperty property, GUIContent label)
		{
			var identifierProp = property.FindPropertyRelative("Identifier");
			var propertyProp = property.FindPropertyRelative("Property");
			var typeProp = property.FindPropertyRelative("Type");

			bool haveGameStateType = drawer.TryGetAttribute<GameStateTypeAttribute>(out _);  

			if (!drawer.TryGetAttribute<GameStateRefAttribute>(out _))
			{
				float height = EditorGUI.GetPropertyHeight(identifierProp);
				height += EditorGUI.GetPropertyHeight(propertyProp);
				if (!haveGameStateType)
				{
					height += EditorGUI.GetPropertyHeight(typeProp);
				}

				return height;
			}

			return EditorGUIUtility.singleLineHeight;
		}

		static void DrawRef(GameStateRefAttribute gameStateRef, Rect position, SerializedProperty property, GUIContent label)
		{
			string value = property.GetGameStateIdentifierValue().ToString();

			var settingsObject = GameSettings.GetSerializedSettings();
			settingsObject.Update();
			var globalStatesProp = settingsObject.FindProperty("GlobalGameStates");

			// Create labels for all global states.
			List<int> m_Indices = new();
			List<GUIContent> m_Labels = new();
			property.GetGameStates(in gameStateRef, ref m_Indices, ref m_Labels);
			property.GetLocalGameStates(in gameStateRef, ref m_Indices, ref m_Labels);
			int selected = m_Labels.FindIndex(x => x.text.Equals(value));
			
			EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var rect = position;
			rect.width -= popupWidth;
			rect.height = EditorGUIUtility.singleLineHeight;

			if (m_Labels.Count > 0)
			{
				// Do we have any previous selection?
				if (selected < 0)
				{
					// Check if we had a valid selection previously.
					if (!string.IsNullOrEmpty(value))
					{
						// Warn the user that the previously referenced game state have disappeared.
						Debug.LogWarning(
							$"The previously selected state {value} on {property.name} in game object " +
							$"{property.serializedObject.targetObject.name} is invalid. Resetting state reference.",
							property.serializedObject.targetObject);
					}

					// Reset selection to first identifier.
					selected = 0;
				}
				
				// Show possible game states.
				selected = EditorGUI.Popup(rect, GUIContent.none, selected, m_Labels.ToArray());

				// Update identifier of game state ref to use the identifier from the selected global state.
				if (selected < globalStatesProp.arraySize)
				{
					var stateProp = globalStatesProp.GetArrayElementAtIndex(m_Indices[selected]);
					property.SetGameStateIdentifierValue(stateProp.GetGameStateIdentifierValue());
				}
			}
			else
			{
				// No global states exist yet.
				EditorGUI.LabelField(rect, GUIContent.none, new GUIContent("No global states exist."));
			}

			rect.x += rect.width + spacing;
			rect.width = popupWidth - spacing;

			if (GUI.Button(rect, "Edit"))
			{
				SettingsService.OpenProjectSettings(GameSettingsProvider.SettingsPath);
			}

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
}