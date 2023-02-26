using UnityEngine;

namespace StoryFramework
{
	/// <summary>
	/// Types of comparison.
	/// </summary>
	public enum ComparisonTypes
	{
		True,
		False,
		//Equals,
		//NotEqual,
		//GreaterThan,
		//SmallerThan,
		//SmallerOrEqual,
		//GreaterOrEqual
	}	

	/// <summary>
	/// Disable types.
	/// </summary>
	public enum DisableTypes
	{
		/// <summary>
		/// Mark the field as read only. 
		/// </summary>
		ReadOnly,
		
		/// <summary>
		/// Hides the field.
		/// </summary>
		Hide
	}
	
	/// <summary>
	/// Compares a property with value.
	/// If the comparision fails, the field is affected by a action specified by the disable type.  
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
	public class EnableIfAttribute : PropertyAttribute
	{
		public readonly string ComparedPropertyName;
		//public readonly object ComparedValue;
		public readonly ComparisonTypes ComparisonType;
		public readonly DisableTypes DisableType;
		
		public EnableIfAttribute(
			string comparedPropertyName,
			//object comparedValue,
			ComparisonTypes comparisonType = ComparisonTypes.True,
			DisableTypes disablingType = DisableTypes.Hide)
		{
			ComparedPropertyName = comparedPropertyName;
			//ComparedValue = comparedValue;
			ComparisonType = comparisonType;
			DisableType = disablingType;
		}
	}
}