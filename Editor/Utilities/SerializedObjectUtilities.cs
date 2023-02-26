using System;
using System.Reflection;
using UnityEditor;

namespace StoryFramework.Editor.Utilities
{
	public static class SerializedObjectUtilities
	{
		public static bool TryGetFieldType(this SerializedProperty property, out Type fieldType)
		{
			var containerType = property.serializedObject.targetObject.GetType();
			if (TryGetFieldFromPropertyPath(containerType, property.propertyPath, out var fieldInfo))
			{
				fieldType = fieldInfo.FieldType;
				return true;
			}

			fieldType = default;
			return false;
		}
		
		public static bool TryGetFieldFromPropertyPath(this Type parentType, string path, out FieldInfo fieldInfo)
		{
			BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Default;
			fieldInfo = parentType.GetField(path, flags);
			var pathComponents = path.Split('.');
			for (var i = 0; i < pathComponents.Length; i++)
			{
				string fieldName = pathComponents[i];
				fieldInfo = parentType.GetField(fieldName, flags);

				if (fieldInfo != null)
				{
					parentType = fieldInfo.FieldType;
				}
				else
				{
					fieldInfo = default;
					return false;
				}
			}

			return fieldInfo != null;
		}
	}
}