using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using UnityEngine;

public class PropertyBind : MonoBehaviour
{
	[SerializeField]private List<MonoProperty> bindingGroups = new List<MonoProperty>();

    private void Awake()
    {
        bindingGroups.ForEach(g => g.GetMembers());
    }

	private void Start()
	{
		bindingGroups.ForEach(g => g.Update());
	}

    private void Update()
    {
        bindingGroups.ForEach(g => g.Update());
    }

    [Serializable]
    public class MonoProperty
    {
		[SerializeField]private Component source;
		[SerializeField]private  string sourceProp;
		[SerializeField]private  Component target;
		[SerializeField]private  string targetProp;
		[SerializeField]private  BindType bindingDirection;

		private string sourceMember;
		private string targetMember;

        private object _sourceVal;
        private object _targetVal;

		private UnityEngine.Object safeSource;
		private UnityEngine.Object safeTarget;

        public void GetMembers()
        {
            if (!source || !target || string.IsNullOrEmpty(sourceProp) || string.IsNullOrEmpty(targetProp))
            {
                canUpdate = false;
                return;
            }

			var sourceParams = sourceProp.Split (new[] { '/', '(' });
			var targetParams = targetProp.Split (new[] { '/', '(' });
			sourceMember = sourceParams[1];
			targetMember = targetParams[1];

			if(sourceParams [0] == typeof(GameObject).Name())
				safeSource = source.gameObject;
			else
				safeSource = source;

			if(targetParams [0] == typeof(GameObject).Name ())
				safeTarget = target.gameObject;
			else
				safeTarget = target;

			MemberInfo sm = safeSource.GetType()
                .FieldsAndProperties(BindingFlags.Instance | BindingFlags.Public)
				.FirstOrDefault(m => m.Name == sourceMember);

			MemberInfo tm = safeTarget.GetType()
                .FieldsAndProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(m => m.Name == targetMember);

			if (sm == null || tm == null || sm.Type() != tm.Type())
            {
                canUpdate = false;
                return;
            }

            canUpdate = true;
        }

        public void Update()
        {
            if (canUpdate)
            {
                switch (bindingDirection)
                {
                    case BindType.SourceUpdateTarget:
                    {
						object value = safeSource.TryGetValue(sourceMember, BindingFlags.Instance | BindingFlags.Public);
						safeTarget.TrySetValue(targetMember, value, BindingFlags.Instance | BindingFlags.Public);
                    }
                        break;
                    case BindType.TargetUpdateSource:
                    {
						object value = safeTarget.TryGetValue(targetMember, BindingFlags.Instance | BindingFlags.Public);
						safeSource.TrySetValue(sourceMember, value, BindingFlags.Instance | BindingFlags.Public);
                    }
                        break;
                    case BindType.Both:
                    {
						object sVal = safeSource.TryGetValue(sourceMember, BindingFlags.Instance | BindingFlags.Public);
						object tVal = safeTarget.TryGetValue(targetMember, BindingFlags.Instance | BindingFlags.Public);
                        if (!string.Equals(sVal,_sourceVal))
							safeTarget.TrySetValue(targetMember, sVal, BindingFlags.Instance | BindingFlags.Public);
                        else if (!string.Equals(tVal, _targetVal))
							safeSource.TrySetValue(sourceMember, tVal, BindingFlags.Instance | BindingFlags.Public);

                        _sourceVal = sVal;
                        _targetVal = tVal;
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [SerializeField] private bool canUpdate;
    }

    public enum BindType
    {
        SourceUpdateTarget,
        TargetUpdateSource,
        Both
    }
}