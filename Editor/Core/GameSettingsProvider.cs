using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace StoryFramework.Editor
{
	public class GameSettingsProvider : SettingsProvider
	{
		public const string SettingsPath = "Project/MoxieJam/StorySettings";
				
		SerializedObject m_Settings;

		GameSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
			: base(path, scopes, keywords)
		{
		}

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			m_Settings = GameSettings.GetSerializedSettings();
		}

		public override void OnGUI(string searchContext)
		{
			m_Settings.Update();

			EditorGUILayout.BeginHorizontal(new GUIStyle() { padding = new RectOffset(10, 10, 10, 10)});
			EditorGUILayout.BeginVertical();

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(m_Settings.FindProperty("StartScene"));
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(m_Settings.FindProperty("GlobalGameStates"));

			if (EditorGUI.EndChangeCheck())
			{
				m_Settings.ApplyModifiedProperties();
			}

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		[SettingsProvider]
		public static SettingsProvider CreateGameSettingsProvider()
		{
			return new GameSettingsProvider(SettingsPath, SettingsScope.Project)
			{
				label = "Story Settings",
				// Populate the search keywords to enable smart search filtering and label highlighting:
				//keywords = new HashSet<string>(new[] { "Number", "Some String" })
			};
		}
	}
}