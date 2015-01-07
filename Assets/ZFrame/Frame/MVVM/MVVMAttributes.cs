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
	public class ViewModelAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class ViewAttribute : Attribute
	{
		public Type SourceType { get; set; }

		public ViewAttribute(Type sourceType)
		{
			SourceType = sourceType;
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Event)]
	public class BindingMemberAttribute : Attribute
	{
		public string BindingKey { get; set; }

		public BindingMemberAttribute(string sourceKey)
		{
			BindingKey = sourceKey;
		}
	}
}