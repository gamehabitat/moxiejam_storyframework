using System;
using UnityEditor;

namespace StoryFramework.Editor.Utilities
{
	public static class GameStateEditorUtilities
	{
		/// <summary>
		/// Returns the identifier property of a game state property.
		/// </summary>
		public static SerializedProperty GetGameStateIdentifierProperty(this SerializedProperty gameStateProp)
		{
			return gameStateProp.FindPropertyRelative("Identifier");
		}

		/// <summary>
		/// Returns the value property of a game state property.
		/// </summary>
		public static SerializedProperty GetGameStateValueProperty(this SerializedProperty gameStateProp)
		{
			return gameStateProp.FindPropertyRelative("Value");
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

		/// <summary>
		/// Returns a game state identifier.
		/// </summary>
		public static GameStateIdentifier GetGameStateIdentifier(this SerializedProperty identifierProp)
		{
			return new GameStateIdentifier(
				identifierProp.FindPropertyRelative("Identifier").stringValue,
				identifierProp.FindPropertyRelative("Property").stringValue);
		}

		/// <summary>
		/// Copies the game state identifier from one serialized property to another.
		/// </summary>
		public static void CopyGameStateIdentifier(this SerializedProperty targetIdentifierProp, SerializedProperty identifierProp)
		{
			targetIdentifierProp.FindPropertyRelative("Identifier").stringValue =
				identifierProp.FindPropertyRelative("Identifier").stringValue;
			targetIdentifierProp.FindPropertyRelative("Property").stringValue =
				identifierProp.FindPropertyRelative("Property").stringValue;
		}

		/// <summary>
		/// Returns the game state type on a game state value.
		/// </summary>
		public static GameStateTypes GetGameStateValueType(this SerializedProperty gameStateValue)
		{
			return (GameStateTypes)gameStateValue.FindPropertyRelative("Type").enumValueIndex;
		}

	}
}