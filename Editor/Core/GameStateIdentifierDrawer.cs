using System;
using System.Collections.Generic;
using StoryFramework.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor
{
	[CustomPropertyDrawer(typeof(GameStateIdentifier))]
	public class GameStateIdentifierDrawer : UnityEditor.PropertyDrawer
	{
		const float popupWidth = 80.0f;
		const float spacing = 5.0f;

		private List<GUIContent> m_Labels = new List<GUIContent>();// Array.Empty<GUIContent>();
		GUIContent m_ValueContent = new("Value");

		public GameStateIdentifierDrawer()
		{
		}

		bool IsRef(out GameStateRefAttribute gameStateRef)
		{
			var attribs = fieldInfo.GetCustomAttributes(typeof(GameStateRefAttribute), true);
			if (attribs.Length > 0)
			{
				gameStateRef = attribs[0] as GameStateRefAttribute;
				return true;
			}

			gameStateRef = default;
			return false;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var identifierProp = property.FindPropertyRelative("Identifier");
			var propertyProp = property.FindPropertyRelative("Property");

			if (!IsRef(out var gameStateRef))
			{
				var rect = position;

				// Draw fields. 
				rect.height = EditorGUI.GetPropertyHeight(identifierProp);  
				EditorGUI.PropertyField(rect, identifierProp);
				rect.y += rect.height;

				rect.height = EditorGUI.GetPropertyHeight(propertyProp);  
				EditorGUI.PropertyField(rect, propertyProp);
				rect.y += rect.height;
			}
			else
			{
				DrawRef(gameStateRef, position, property, label);
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var identifierProp = property.FindPropertyRelative("Identifier");
			var propertyProp = property.FindPropertyRelative("Property");

			if (!IsRef(out _))
			{
				float height = EditorGUI.GetPropertyHeight(identifierProp);
				height += EditorGUI.GetPropertyHeight(propertyProp);
				return height;
			}

			return EditorGUIUtility.singleLineHeight;
		}

		void DrawRef(GameStateRefAttribute gameStateRef, Rect position, SerializedProperty property, GUIContent label)
		{
			string value = property.GetGameStateIdentifier();
			int selected = -1;

			var settingsObject = GameSettings.GetSerializedSettings();
			settingsObject.Update();

			// Create labels for all global states.
			m_Labels.Clear();
			var globalStatesProp = settingsObject.FindProperty("GlobalGameStates");
			for (int i = 0; i < globalStatesProp.arraySize; ++i)
			{
				var stateProp = globalStatesProp.GetArrayElementAtIndex(i);

				// Skip states of wrong type if this is a type constrained reference.
				if (gameStateRef.IsTypeConstrained &&
				    (gameStateRef.Type != stateProp.GetGameStateValueProperty().GetGameStateValueType()))
				{
					continue;
				}

				string identifier = stateProp.GetGameStateIdentifierProperty().GetGameStateIdentifier();
				m_Labels.Add(new GUIContent(identifier));
			
				// Check if this is the currently selected value.
				if (identifier.Equals(value))
				{
					selected = i;
				}
			}

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
				var stateProp = globalStatesProp.GetArrayElementAtIndex(selected);
				property.CopyGameStateIdentifier(stateProp.FindPropertyRelative("Identifier"));
			}
			else
			{
				// No global states exist yet.
				EditorGUI.LabelField(rect, label, new GUIContent("No global states exist."));
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