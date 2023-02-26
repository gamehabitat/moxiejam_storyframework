using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StoryFramework.Editor.Utilities
{
	public struct CachedGameStateRef
	{
		public GameStateIdentifier Identifier;
		public GUIContent Label;
	}
	
	public static class GameStateEditorUtilities
	{
		static Dictionary<string, CachedGameStateRef> CachedGameStateRefs = new();

		public static void UpdateGameStateRefCache()
		{
		}

		/// <summary>
		/// Returns the identifier property of a game state property.
		/// </summary>
		public static SerializedProperty GetGameStateIdentifierProperty(this SerializedProperty property)
		{
			// TODO: Verify property type.
			if (property.type.Equals(typeof(GameStateIdentifier).Name))
			{
				// Already on the identifier property of game state.
				return property;
			}
				
			// Find identifier property.
			return property.FindPropertyRelative("Identifier");
		}
		
		/// <summary>
		/// Sets the identifier name of the game state.
		/// </summary>
		public static void SetGameStateIdentifierValue(this SerializedProperty gameStateProp, string identifier)
		{
			gameStateProp
				.FindPropertyRelative("Identifier")
				.FindPropertyRelative("Identifier")
				.stringValue = identifier;
		}
		
		/// <summary>
		/// Sets the property identifier of the game state.
		/// </summary>
		public static void SetGameStatePropertyValue(this SerializedProperty gameStateProp, string property)
		{
			gameStateProp
				.FindPropertyRelative("Identifier")
				.FindPropertyRelative("Property")
				.stringValue = property;
		}

		/// <summary>
		/// Sets the type of the game state.
		/// </summary>
		public static void SetGameStateTypeValue(this SerializedProperty gameStateProp, GameStateTypes type)
		{
			gameStateProp
				.FindPropertyRelative("Identifier")
				.FindPropertyRelative("Type")
				.enumValueIndex = (int)type;
		}

		
		/// <summary>
		/// Returns a game state identifier.
		/// </summary>
		public static GameStateIdentifier GetGameStateIdentifierValue(this SerializedProperty property)
		{
			var identifierProp = property.GetGameStateIdentifierProperty();
			return new GameStateIdentifier(
				identifierProp.FindPropertyRelative("Identifier").stringValue,
				identifierProp.FindPropertyRelative("Property").stringValue,
				(GameStateTypes)identifierProp.FindPropertyRelative("Type").enumValueIndex);
		}

		/// <summary>
		/// Copies the game state identifier from one serialized property to another.
		/// </summary>
		public static void SetGameStateIdentifierValue(this SerializedProperty property, in GameStateIdentifier identifier)
		{
			var targetIdentifierProp = property.GetGameStateIdentifierProperty();
			targetIdentifierProp.FindPropertyRelative("Identifier").stringValue = identifier.Identifier;
			targetIdentifierProp.FindPropertyRelative("Property").stringValue = identifier.Property;
			targetIdentifierProp.FindPropertyRelative("Type").enumValueIndex = (int)identifier.Type;
		}

		/// <summary>
		/// Returns the value property associated with the value type of a game state property.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if type isn't supported.</exception>
		public static SerializedProperty GetGameStateValueProp(this SerializedProperty gameStateValueProp, GameStateTypes type)
		{
			switch (type)
			{
			case GameStateTypes.BooleanFlag:
				return gameStateValueProp.FindPropertyRelative("m_BooleanValue");
			case GameStateTypes.IntegerNumber:
				return gameStateValueProp.FindPropertyRelative("m_IntegerValue");
			case GameStateTypes.FloatNumber:
				return gameStateValueProp.FindPropertyRelative("m_FloatValue");
			case GameStateTypes.Text:
				return gameStateValueProp.FindPropertyRelative("m_TextValue");
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static void GetGameStates(this SerializedProperty property, in GameStateRefAttribute gameStateRef, ref List<int> indices, ref List<GUIContent> labels)
		{
			var settingsObject = GameSettings.GetSerializedSettings();
			settingsObject.Update();
			var globalStatesProp = settingsObject.FindProperty("GlobalGameStates");

			indices.Clear();
			labels.Clear();
			for (int i = 0; i < globalStatesProp.arraySize; ++i)
			{
				var stateProp = globalStatesProp.GetArrayElementAtIndex(i);
				var stateIdentifier = stateProp.GetGameStateIdentifierValue();

				// Skip states of wrong type if this is a type constrained reference.
				if (gameStateRef.IsTypeConstrained &&
				    (gameStateRef.Type != stateIdentifier.Type))
				{
					continue;
				}

				if (gameStateRef.IsPropertyConstrained &&
				    ((gameStateRef.Type != stateIdentifier.Type) ||
				     (!gameStateRef.PropertyConstraint.Equals(stateIdentifier.Property, StringComparison.OrdinalIgnoreCase))))
				{
					continue;
				}

				string identifier = stateIdentifier.ToString();
				labels.Add(new GUIContent(identifier));
				indices.Add(i);
			}
		}

		public static void GetLocalGameStates(this SerializedProperty property, in GameStateRefAttribute gameStateRef, ref List<int> indices, ref List<GUIContent> labels)
		{
			var component = property.serializedObject.targetObject as Component;
			if (!component)
			{
				return;
			}

			var persistentObjects = component
				.gameObject
				.scene
				.GetRootGameObjects()
				.SelectMany(x => x.GetComponentsInChildren<PersistentObject>());

			// Find all identifiers.
			List<GameStateIdentifier> identifiers = new();
			List<GUIContent> paths = new();
			foreach (var persistentObject in persistentObjects)
			{
				foreach (var persistentComponent in persistentObject.GetComponents<IPersistentComponent>())
				{
					string path = ((Component)persistentComponent).gameObject.GetGameObjectPath();
					GUIContent label = new GUIContent(path);

					foreach (var gameStateProperty in persistentComponent.GameStateProperties)
					{
						identifiers.Add(new GameStateIdentifier(
							persistentObject.Identifier,
							gameStateProperty.Name,
							gameStateProperty.Type));

						paths.Add(label);
					}
				}
			}

			for(int i = 0; i < identifiers.Count; ++i)
			{
				var stateIdentifier = identifiers[i];

				// Skip states of wrong type if this is a type constrained reference.
				if (gameStateRef.IsTypeConstrained &&
				    (gameStateRef.Type != stateIdentifier.Type))
				{
					continue;
				}

				if (gameStateRef.IsPropertyConstrained &&
				    ((gameStateRef.Type != stateIdentifier.Type) ||
				     (!gameStateRef.PropertyConstraint.Equals(stateIdentifier.Property, StringComparison.OrdinalIgnoreCase))))
				{
					continue;
				}

				string identifier = stateIdentifier.ToString();
				//labels.Add(new GUIContent(identifier));
				labels.Add(paths[i]);
				indices.Add(i);
			}
		}

		public static string GetGameObjectPath(this GameObject gameObject)
		{
			Transform transform = gameObject.transform;
			string path = transform.name;
			while (transform.parent != null)
			{
				transform = transform.parent;
				path = transform.name + "/" + path;
			}

			return path;
		}
	}
}