using UnityEngine;

namespace StoryFramework
{
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class GameStateTypeAttribute : PropertyAttribute
	{
		public readonly GameStateTypes Type;
		public readonly string Label;

		public bool hasCustomLabel => !string.IsNullOrEmpty(Label);

		public GameStateTypeAttribute(string label, GameStateTypes type)
		{
			Type = type;
			Label = label;
		}

		public GameStateTypeAttribute(GameStateTypes type)
		{
			Type = type;
			Label = default;
		}
	}
}