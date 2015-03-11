using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using UnityEngine;

[ExecuteInEditMode]
public class PropertyBind : MonoBehaviour
{
    public List<MonoProperty> bindingGroups = new List<MonoProperty>();

    private void Awake()
    {
        bindingGroups.ForEach(g => g.GetMembers());
    }

    private void Update()
    {
        bindingGroups.ForEach(g => g.Update());
    }

    [Serializable]
    public class MonoProperty
    {
        public Component source;
        public string sourceProp;
        public Component target;
        public string targetProp;
        public BindType bindingDirection;

        public string sourceMember;
        public string targetMember;

        private object _sourceVal;
        private object _targetVal;

        public void GetMembers()
        {
            if (!source || !target || string.IsNullOrEmpty(sourceProp) || string.IsNullOrEmpty(targetProp))
            {
                canUpdate = false;
                return;
            }

            sourceMember = sourceProp.Split(new[] {'/', '('})[1];
            targetMember = targetProp.Split(new[] {'/', '('})[1];

            MemberInfo sm = source.GetType()
                .FieldsAndProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(m => m.Name == sourceMember);

            MemberInfo tm = target.GetType()
                .FieldsAndProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(m => m.Name == targetMember);

            if (source == null || target == null || sm.Type() != tm.Type())
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
                        object value = source.TryGetValue(sourceMember, BindingFlags.Instance | BindingFlags.Public);
                        target.TrySetValue(targetMember, value, BindingFlags.Instance | BindingFlags.Public);
                    }
                        break;
                    case BindType.TargetUpdateSource:
                    {
                        object value = target.TryGetValue(targetMember, BindingFlags.Instance | BindingFlags.Public);
                        source.TrySetValue(sourceMember, value, BindingFlags.Instance | BindingFlags.Public);
                    }
                        break;
                    case BindType.Both:
                    {
                        object sVal = source.TryGetValue(sourceMember, BindingFlags.Instance | BindingFlags.Public);
                        object tVal = target.TryGetValue(targetMember, BindingFlags.Instance | BindingFlags.Public);
                        if (!string.Equals(sVal,_sourceVal))
                            target.TrySetValue(targetMember, sVal, BindingFlags.Instance | BindingFlags.Public);
                        else if (!string.Equals(tVal, _targetVal))
                            source.TrySetValue(sourceMember, tVal, BindingFlags.Instance | BindingFlags.Public);

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