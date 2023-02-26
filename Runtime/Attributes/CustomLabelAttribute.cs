using UnityEngine;

namespace StoryFramework
{
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class CustomLabelAttribute : PropertyAttribute
	{
		public readonly string Label;

		public CustomLabelAttribute(string label)
		{
			Label = label;
		}
	}
}