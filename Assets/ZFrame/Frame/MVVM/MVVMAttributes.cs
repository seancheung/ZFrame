using System;

namespace ZFrame.Frame.MVVM
{
	public enum BindingDirection
	{
		SourceToTarget,
		TargetToSource,
		Both
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class BindingSourceAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class BindingTargetAttribute : Attribute
	{
		public Type SourceType { get; set; }

		public BindingTargetAttribute(Type sourceType)
		{
			SourceType = sourceType;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class BindingPropertyAttribute : Attribute
	{
		public string SourceKey { get; set; }

		public BindingPropertyAttribute(string sourceKey)
		{
			SourceKey = sourceKey;
		}
	}
}