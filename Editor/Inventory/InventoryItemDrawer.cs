using System.IO;
using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(InventoryItem))]
    public class InventoryItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            // Draw label.
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            position.height = EditorGUI.GetPropertyHeight(property, GUIContent.none);
            EditorGUI.PropertyField(position, property, GUIContent.none);
            position.y += EditorGUI.GetPropertyHeight(property);

            if (property.objectReferenceValue == null)
            {
                position.height = EditorGUIUtility.singleLineHeight;
                if (GUI.Button(position, "Create item"))
                {
                    var path = EditorUtility.SaveFilePanelInProject("Save item as...", "Item.asset", "asset", "Please enter a file name to save the item to");
                    if (!string.IsNullOrEmpty(path))
                    {
                        var inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
                        inventoryItem.Name = Path.GetFileNameWithoutExtension(path);
                        if (string.IsNullOrEmpty(inventoryItem.Name))
                        {
                            inventoryItem.Name = "New item";
                        }
                        AssetDatabase.CreateAsset(inventoryItem, path);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        property.serializedObject.Update();
                        property.objectReferenceValue = inventoryItem;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUI.GetPropertyHeight(property);
            if (property.objectReferenceValue == null)
            {
                height += EditorGUIUtility.singleLineHeight;
            }

            return height;
        }
    }
}