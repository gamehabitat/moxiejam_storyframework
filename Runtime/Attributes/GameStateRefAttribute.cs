using UnityEngine;

namespace StoryFramework
{
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class GameStateRefAttribute : PropertyAttribute
	{
		public readonly bool IsTypeConstrained;
		public readonly GameStateTypes Type;
		public readonly string PropertyConstraint;
		public bool IsPropertyConstrained => !string.IsNullOrEmpty(PropertyConstraint);

		public GameStateRefAttribute()
		{
			IsTypeConstrained = false;
			Type = default;
			PropertyConstraint = default;
		}

		public GameStateRefAttribute(GameStateTypes type, string propertyConstraint = "")
		{
			IsTypeConstrained = true;
			Type = type;
			PropertyConstraint = propertyConstraint;
		}

		public GameStateRefAttribute(string propertyConstraint = "")
		{
			IsTypeConstrained = false;
			Type = default;
			PropertyConstraint = propertyConstraint;
		}
	}
}