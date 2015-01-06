using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace ZFrame.Frame.MVVM
{
	public class MVVMEngine : MonoSingleton<MVVMEngine>, IZDisposable
	{
		private class BindingGroup
		{
			public object Source { get; set; }
			public PropertyInfo SourceProp { get; set; }
			public object Target { get; set; }
			public PropertyInfo TargetProp { get; set; }

			public bool CanUpdate
			{
				get { return Source != null && SourceProp != null && Target != null && TargetProp != null; }
			}
		}

		private readonly List<BindingGroup> _groups = new List<BindingGroup>();
		private readonly Dictionary<Type, object> _sources = new Dictionary<Type, object>();
		private readonly List<object> _targets = new List<object>();

		public void Register<T>(T bindable) where T : class
		{
			if (Attribute.IsDefined(bindable.GetType(), typeof (BindingSourceAttribute)))
			{
				if (!_sources.TryAdd(bindable.GetType(), bindable))
				{
					Debug.LogError(string.Format("Register mvvm binding failed. type: {0}, object: {1}", bindable.GetType(), bindable));
				}
			}
			if (Attribute.IsDefined(bindable.GetType(), typeof (BindingTargetAttribute)))
			{
				if (!_targets.SafeAdd(bindable))
				{
					Debug.LogError(string.Format("Register mvvm binding failed. type: {0}, object: {1}", bindable.GetType(), bindable));
				}
			}

			_groups.Clear();

			foreach (object target in _targets)
			{
				Type sourceType =
					((BindingTargetAttribute) Attribute.GetCustomAttribute(target.GetType(), typeof (BindingTargetAttribute)))
						.SourceType;
				if (sourceType == null)
				{
					Debug.LogError(string.Format("Binding source class has no target class! source type: {0}", target.GetType()));
					continue;
				}
				object source = _sources.TryGet(sourceType);

				foreach (
					PropertyInfo property in
						target.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof (BindingPropertyAttribute))))
				{
					BindingGroup group = new BindingGroup();
					group.TargetProp = property;
					group.Target = target;
					group.Source = source;

					string sourceKey =
						((BindingPropertyAttribute) Attribute.GetCustomAttribute(property, typeof (BindingPropertyAttribute)))
							.SourceKey;
					group.SourceProp = sourceType.GetProperty(sourceKey);

					if (group.CanUpdate)
					{
						_groups.Add(group);
					}
				}
			}
		}

		private void Notify(PropertyInfo prop)
		{
			foreach (BindingGroup group in _groups.Where(g => g.CanUpdate))
			{
				if (group.SourceProp == prop)
				{
					group.TargetProp.SetValue(group.Target, group.SourceProp.GetValue(group.Source, null), null);
				}
			}
		}

		public void Notify<T>(T target, string propName) where T : class
		{
			if (target == null)
			{
				Debug.LogError(string.Format("Notify property changed target class is null! type: {0}, property: {1}", typeof (T),
					propName));
				return;
			}
			Notify(target.GetType().GetProperty(propName));
		}

		public void Notify<T>(Expression<Func<T>> exp)
		{
			MemberExpression bodyExpr = exp.Body as MemberExpression;
			if (bodyExpr == null)
			{
				throw new ArgumentException("Expression must be a MemberExpression!", "exp");
			}
			PropertyInfo propInfo = bodyExpr.Member as PropertyInfo;
			if (propInfo == null)
			{
				throw new ArgumentException("Expression must be a PropertyExpression!", "exp");
			}

			Notify(propInfo);
		}

		public bool DisposeOnApplicationQuit()
		{
			return false;
		}

		public bool Dispose()
		{
			ReleaseInstance();
			return true;
		}
	}
}