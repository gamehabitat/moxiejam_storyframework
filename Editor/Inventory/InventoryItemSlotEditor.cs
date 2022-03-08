using UnityEditor;
using UnityEditor.UI;

namespace StoryFramework.Editor.Inventory
{
    /// <summary>
    /// Custom editor for inventory item slot.
    /// </summary>
    [CustomEditor(typeof(InventoryItemSlot), true)]
    [CanEditMultipleObjects]
    public class InventoryItemSlotEditor : SelectableEditor
    {
        SerializedProperty iconProp;
        SerializedProperty amountProp;
        SerializedProperty onClickProp;
        SerializedProperty onBeginDragProp;
        SerializedProperty onEndDragProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            iconProp = serializedObject.FindProperty("icon");
            amountProp = serializedObject.FindProperty("amount");
            onClickProp = serializedObject.FindProperty("onClick");
            onBeginDragProp = serializedObject.FindProperty("onBeginDrag");
            onEndDragProp = serializedObject.FindProperty("onEndDrag");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(iconProp);
            EditorGUILayout.PropertyField(amountProp);
            EditorGUILayout.PropertyField(onClickProp);
            EditorGUILayout.PropertyField(onBeginDragProp);
            EditorGUILayout.PropertyField(onEndDragProp);
            serializedObject.ApplyModifiedProperties();
        }
    }
}