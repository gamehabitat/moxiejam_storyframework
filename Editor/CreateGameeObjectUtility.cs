using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor
{
    public class CreateGameeObjectUtility : UnityEditor.Editor
    {
        [MenuItem("GameObject/MoxieJam/StoryFramework/Create Interactable Item", false)]
        public static void CreateInteractableItem()
        {
            var settingsWindow = CreateInstance<InteractableItemSettings>();
            settingsWindow.position = new Rect(Screen.width / 2.0f, Screen.height / 2.0f, 300.0f, 300.0f);
            settingsWindow.ShowPopup();
        }
        
        [MenuItem("GameObject/MoxieJam/StoryFramework/Create Drop Item target", false)]
        public static void CreateDropItemTarget()
        {
            SerializedObject serializedObject;

            var obj = new GameObject("DropItemTarget");
            var boxCollider2D = obj.AddComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = true;
            var spriteRenderer = obj.AddComponent<SpriteRenderer>();
            var persistentObject = obj.AddComponent<PersistentObject>();

            var dropItemTarget = obj.AddComponent<DropItemTarget>();
            serializedObject = new SerializedObject(dropItemTarget);
            serializedObject.Update();
            serializedObject.FindProperty("requiredItem");
            serializedObject.FindProperty("requiredAmount").intValue = 1;
            serializedObject.ApplyModifiedProperties();
            
            var descriptionOnClick = obj.AddComponent<ShowDescriptionOnMouseOver>();
            serializedObject = new SerializedObject(descriptionOnClick);
            serializedObject.Update();
            serializedObject.FindProperty("description").stringValue = "Try dropping an item on me.";
            serializedObject.ApplyModifiedProperties();

            /*var lockableObject = obj.AddComponent<LockableObject>();
            serializedObject = new SerializedObject(lockableObject);
            serializedObject.Update();
            serializedObject.FindProperty("isLockedOnStart").boolValue = true;
            serializedObject.ApplyModifiedProperties();*/
        }
        
        [MenuItem("GameObject/MoxieJam/StoryFramework/Create Object with description", false)]
        public static void CreateObjectWithDescription()
        {
            SerializedObject serializedObject;

            var obj = new GameObject("ObjectWithDescription");
            var boxCollider2D = obj.AddComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = true;
            var spriteRenderer = obj.AddComponent<SpriteRenderer>();

            /*var descriptionOnClick = obj.AddComponent<ShowDescriptionOnClick>();
            serializedObject = new SerializedObject(descriptionOnClick);
            serializedObject.Update();
            serializedObject.FindProperty("description").stringValue = "A object with description.";
            serializedObject.ApplyModifiedProperties();*/

            var descriptionOnMouseOver = obj.AddComponent<ShowDescriptionOnMouseOver>();
            serializedObject = new SerializedObject(descriptionOnMouseOver);
            serializedObject.Update();
            serializedObject.FindProperty("description").stringValue = "A object with description.";
            serializedObject.ApplyModifiedProperties();
        }
    }

    public class InteractableItemSettings : UnityEditor.EditorWindow
    {
        private Sprite sprite;
        private bool pickUpItem;
        private InventoryItem inventoryItem;
        private Vector2 scrollPosition;

        private GameObject editGameObject;
        private BoxCollider2D boxCollider2D;
        private SpriteRenderer spriteRenderer;
        private PersistentObject persistentObject;
        private InteractableItem interactableItem;

        private void OnEnable()
        {
            var window = GetWindow<InteractableItemSettings>();
            window.titleContent = new GUIContent("Interactable Item Settings");
        }

        private void OnDisable()
        {
            if (editGameObject)
            {
                DestroyImmediate(editGameObject);
                editGameObject = null;
            }
        }

        private void OnGUI()
        {
            if (!editGameObject)
            {
                // If no game object exist, create it.
                editGameObject = new GameObject("InteractableItem");
                boxCollider2D = editGameObject.AddComponent<BoxCollider2D>();
                boxCollider2D.isTrigger = true;
                spriteRenderer = editGameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
                persistentObject = editGameObject.AddComponent<PersistentObject>();
                interactableItem = editGameObject.AddComponent<InteractableItem>();
            }
            SerializedObject serializedObject;

            // Set game object name.
            editGameObject.name = EditorGUILayout.TextField("Name", editGameObject.name);

            // Setup sprite.
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Sprite settings");
            spriteRenderer.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", spriteRenderer.sprite, typeof(Sprite), false);
            EditorGUILayout.Separator();

            // Setup item.
            EditorGUILayout.LabelField("Item settings");
            serializedObject = new SerializedObject(interactableItem);
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pickUpOnClick"), new GUIContent("Can pick up Item"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("item"), new GUIContent("Item"));
            serializedObject.ApplyModifiedProperties();
            
            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create"))
            {
                var createdObject = editGameObject;

                // We don't want to destroy the edited game object on close.
                editGameObject = null;

                // Close window.
                Close();

                // Set the created gameobject to the selected.
                Selection.activeGameObject = createdObject;
            }

            if (GUILayout.Button("Cancel"))
            {
                // Close window.
                Close();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}