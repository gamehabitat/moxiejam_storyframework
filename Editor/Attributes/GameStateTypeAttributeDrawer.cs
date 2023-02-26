using StoryFramework.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor
{
	//[CustomPropertyDrawer(typeof(GameStateTypeAttribute))]
	public class GameStateTypeAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			/*if (!(property.get is GameState))
			{
				EditorGUI.PropertyField(position, property, label);
				return;
			}*/

			var typedAttribute = ((GameStateTypeAttribute)attribute);

			// Force type.
			var identifier = property.GetGameStateIdentifierValue();
			identifier.Type = typedAttribute.Type; 
			property.SetGameStateIdentifierValue(in identifier);

			// Draw property.
			EditorGUI.PropertyField(position, property,
				typedAttribute.hasCustomLabel ? new GUIContent(typedAttribute.Label) : label,
				property.hasVisibleChildren);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var typedAttribute = ((GameStateTypeAttribute)attribute);

			// Force type.
			var identifier = property.GetGameStateIdentifierValue();
			identifier.Type = typedAttribute.Type; 
			property.SetGameStateIdentifierValue(in identifier);

			// Get height.
			return EditorGUI.GetPropertyHeight(property,
				typedAttribute.hasCustomLabel ? new GUIContent(typedAttribute.Label) : label,
				property.hasVisibleChildren);
		}
	}
}