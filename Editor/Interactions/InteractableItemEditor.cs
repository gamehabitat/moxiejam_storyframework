using UnityEditor;

namespace StoryFramework.Editor
{
    //[CustomEditor(typeof(InteractableItem))]
    [CanEditMultipleObjects]
    public class InteractableItemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var itemProp = serializedObject.FindProperty("item");
            var customDescriptionProp = serializedObject.FindProperty("customDescription");
            var showDescriptionOnClickProp = serializedObject.FindProperty("showDescriptionOnClick");
            var showDescriptionOnMouseOverProp = serializedObject.FindProperty("showDescriptionOnMouseOver");
            var pickUpOnClickProp = serializedObject.FindProperty("pickUpOnClick");
            var onMouseEnterProp = serializedObject.FindProperty("onMouseEnter");
            var onMouseExitProp = serializedObject.FindProperty("onMouseExit");
            var onMouseClickProp = serializedObject.FindProperty("onMouseClick");
            var onPickUpProp = serializedObject.FindProperty("onPickUp");

            serializedObject.Update();

            EditorGUILayout.PropertyField(itemProp);
            EditorGUILayout.PropertyField(customDescriptionProp);
            EditorGUILayout.PropertyField(showDescriptionOnClickProp);
            EditorGUILayout.PropertyField(showDescriptionOnMouseOverProp);
            EditorGUILayout.PropertyField(pickUpOnClickProp);
            EditorGUILayout.PropertyField(onMouseEnterProp);
            EditorGUILayout.PropertyField(onMouseExitProp);
            EditorGUILayout.PropertyField(onMouseClickProp);
            EditorGUILayout.PropertyField(onPickUpProp);

            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}