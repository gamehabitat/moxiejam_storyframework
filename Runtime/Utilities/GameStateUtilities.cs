using System;
using UnityEngine;

namespace StoryFramework.Utilities
{
	public static class GameStateUtilities
	{
		public static bool TryGetIdentifier<T>(T dataContainer, string property, GameStateTypes type, out GameStateIdentifier identifier) where T : MonoBehaviour, IPersistentComponent
		{
			if (!dataContainer.TryGetComponent<PersistentObject>(out var persistentObject))
			{
				identifier = default;
				Debug.LogError($"No persistent object found on the game object {dataContainer.name}. Please add a PersistentObject to it.");
				return false;
			}

			string id = persistentObject.Identifier;
			if (!persistentObject.HasCustomIdentifier)
			{
				// Append component index to identifier.
				var persistentComponents = persistentObject.GetComponents<IPersistentComponent>();
				int dataIndex = Array.FindIndex(persistentComponents, x => x == dataContainer);
				if (dataIndex < 0)
				{
					identifier = default;
					Debug.LogError($"Data container not registered to persistent object in {dataContainer.name}. Please add it.");
					return false;
				}

				id += $"[{dataIndex.ToString()}]";
			}

			identifier = new GameStateIdentifier(id, property, type);
			return true;
		}

		public static GameStateIdentifier GetIdentifier<T>(T dataContainer, string property, GameStateTypes type) where T : MonoBehaviour, IPersistentComponent
		{
			if (!TryGetIdentifier(dataContainer, property, type, out var identifier))
			{
				Debug.LogError($"Unable to find the identifier on the game object {dataContainer.name}. Please add a PersistentObject to it.");
				return default;
			}

			return identifier;
		}

		public static GameStateIdentifier GetIdentifier<T>(T dataContainer, in GameStateProperty property) where T : MonoBehaviour, IPersistentComponent
		{
			if (!TryGetIdentifier(dataContainer, property.Name, property.Type, out var identifier))
			{
				Debug.LogError($"Unable to find the identifier on the game object {dataContainer.name}. Please add a PersistentObject to it.");
				return default;
			}

			return identifier;
		}
	}
}