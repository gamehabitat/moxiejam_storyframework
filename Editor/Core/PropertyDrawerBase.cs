using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor.Core
{
	public abstract class PropertyDrawerBase : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			bool disabled = false;

			if (TryGetAttribute<EnableIfAttribute>(out var enableIfAttribute))
			{
				var comparedProp = property.serializedObject.FindProperty(enableIfAttribute.ComparedPropertyName);
				if (!Test(comparedProp, enableIfAttribute.ComparisonType))
				{
					switch (enableIfAttribute.DisableType)
					{
					case DisableTypes.ReadOnly:
						disabled = true;
						break;
					case DisableTypes.Hide:
						return;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
			}

			using (new EditorGUI.DisabledScope(disabled: disabled))
			{
				OnGuiInternal(position, property, label);
			}
		}
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			bool disabled = false;

			if (TryGetAttribute<EnableIfAttribute>(out var enableIfAttribute))
			{
				var comparedProp = property.serializedObject.FindProperty(enableIfAttribute.ComparedPropertyName);
				if (!Test(comparedProp, enableIfAttribute.ComparisonType))
				{
					switch (enableIfAttribute.DisableType)
					{
					case DisableTypes.ReadOnly:
						disabled = true;
						break;
					case DisableTypes.Hide:
						return 0.0f;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
			}

			float height = 0.0f;
			using (new EditorGUI.DisabledScope(disabled: disabled))
			{
				height = GetPropertyHeightInternal(property, label);
			}

			return height;
		}

		protected abstract void OnGuiInternal(Rect position, SerializedProperty property, GUIContent label);
		protected abstract float GetPropertyHeightInternal(SerializedProperty property, GUIContent label);

		public bool TryGetAttribute<T>(out T attribute) where T : PropertyAttribute
		{
			var attribs = fieldInfo.GetCustomAttributes(typeof(T), true);
			if (attribs.Length > 0)
			{
				attribute = attribs[0] as T;
				return true;
			}

			attribute = default;
			return false;
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