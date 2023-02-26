using StoryFramework.Editor.Core;
using StoryFramework.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor
{
	[CustomPropertyDrawer(typeof(EnableIfAttribute))]
	public class EnableIfAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var typedAttribute = (EnableIfAttribute)attribute;
			var comparedProp = property.serializedObject.FindProperty(typedAttribute.ComparedPropertyName);
			bool isEnabled =  Test(comparedProp, typedAttribute.ComparisonType);

			if (isEnabled)
			{
				EditorGUI.PropertyField(position, property, label, property.hasVisibleChildren);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var typedAttribute = (EnableIfAttribute)attribute;
			var comparedProp = property.serializedObject.FindProperty(typedAttribute.ComparedPropertyName);
			bool isEnabled =  Test(comparedProp, typedAttribute.ComparisonType);

			return isEnabled ? EditorGUI.GetPropertyHeight(property) : 0.0f;
		}

		bool Test(SerializedProperty booleanProp, ComparisonTypes comparisonType)
		{
			if ((booleanProp == null) || (booleanProp.propertyType != SerializedPropertyType.Boolean))
			{
				return true;
			}

			return booleanProp.boolValue && comparisonType == ComparisonTypes.True;
		}
	}
}