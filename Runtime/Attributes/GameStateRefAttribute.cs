using UnityEngine;

namespace StoryFramework
{
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class GameStateRefAttribute : PropertyAttribute
	{
		public readonly bool IsTypeConstrained;
		public readonly GameStateTypes Type;

		public GameStateRefAttribute()
		{
			IsTypeConstrained = false;
			Type = default;
		}

		public GameStateRefAttribute(GameStateTypes type)
		{
			IsTypeConstrained = true;
			Type = type;
		}
	}
}