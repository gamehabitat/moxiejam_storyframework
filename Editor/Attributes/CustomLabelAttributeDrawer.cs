using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor
{
	//[CustomPropertyDrawer(typeof(CustomLabelAttribute))]
	public class CustomLabelAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var typedAttribute = (CustomLabelAttribute)attribute;
			if (string.IsNullOrEmpty(typedAttribute.Label))
			{
				EditorGUI.PropertyField(position, property, label, property.hasVisibleChildren);
			}
			else
			{
				EditorGUI.PropertyField(position, property, new GUIContent(typedAttribute.Label), property.hasVisibleChildren);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var typedAttribute = (CustomLabelAttribute)attribute;
			if (string.IsNullOrEmpty(typedAttribute.Label))
			{
				return EditorGUI.GetPropertyHeight(property, label, property.hasVisibleChildren);
			}

			return EditorGUI.GetPropertyHeight(property, new GUIContent(typedAttribute.Label), property.hasVisibleChildren);
		}
	}
}