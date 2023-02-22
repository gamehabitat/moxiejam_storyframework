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
		/// Returns a stringified version of game state identifier.
		/// </summary>
		public static string GetGameStateIdentifier(this SerializedProperty identifierProp)
		{
			return GameStateIdentifier.MakeIdentifier(
				identifierProp.FindPropertyRelative("Identifier").stringValue,
				identifierProp.FindPropertyRelative("Property").stringValue);
		}

		/// <summary>
		/// Copies the game state identifier from one serialized property to another.
		/// </summary>
		public static void CopyGameStateIdentifier(this SerializedProperty targetProp, SerializedProperty identifierProp)
		{
			targetProp.FindPropertyRelative("Identifier").stringValue =
				identifierProp.FindPropertyRelative("Property").stringValue;
			targetProp.FindPropertyRelative("Property").stringValue =
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