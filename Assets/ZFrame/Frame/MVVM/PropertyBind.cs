using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Fasterflect;
using Fasterflect.Probing;
using UnityEngine;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

public class PropertyBind : MonoBehaviour
{
    [SerializeField] private List<MonoProperty> bindingGroups = new List<MonoProperty>();

    private void Awake()
    {
        bindingGroups.ForEach(g => g.SetMembers());
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
        [SerializeField] private Component source;
        [SerializeField] private string sourceProp;
        [SerializeField] private Component target;
        [SerializeField] private string targetProp;
        [SerializeField] private BindType bindingDirection;
        [SerializeField] private ConverterType sourceConverterType;
        [SerializeField] private ConverterType targetConverterType;

        private string _sourceProp;
        private string _targetProp;

        private object _sourceVal;
        private object _targetVal;

        private Object _safeSource;
        private Object _safeTarget;

        private bool _convertSource;
        private bool _convertTarget;

        private Type _sourceType;
        private Type _targetType;

        private MemberInfo _sourceMember;
        private MemberInfo _targetMember;

        public void SetMembers()
        {
            if (!source || !target || string.IsNullOrEmpty(sourceProp) || string.IsNullOrEmpty(targetProp))
            {
                canUpdate = false;
                return;
            }

            string[] sourceParams = sourceProp.Split(new[] { '/', '(', ')' });
            string[] targetParams = targetProp.Split(new[] { '/', '(', ')' });
            _sourceProp = sourceParams[1];
            _targetProp = targetParams[1];

            if (sourceParams[0] == typeof (GameObject).Name())
                _safeSource = source.gameObject;
            else
                _safeSource = source;

            if (targetParams[0] == typeof (GameObject).Name())
                _safeTarget = target.gameObject;
            else
                _safeTarget = target;

            _sourceMember = _safeSource.GetType()
                .FieldsAndProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(m => m.Name == _sourceProp);

            _targetMember = _safeTarget.GetType()
                .FieldsAndProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(m => m.Name == _targetProp);

            if (_sourceMember == null || _targetMember == null)
            {
               canUpdate = false;
                return;
            }

            _sourceType = _sourceMember.Type();
            _targetType = _targetMember.Type();

            if (_sourceType != _targetType)
            {
                var sourceCv = TypeDescriptor.GetConverter(_sourceType);
                if (sourceCv.CanConvertTo(_targetType))
                {
                    _convertSource = true;
                }
                var targetCv = TypeDescriptor.GetConverter(_targetType);
                if (targetCv.CanConvertTo(_sourceType))
                {
                    _convertTarget = true;
                }
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
                        object value = _sourceMember.Get(_safeSource);
                        if (_convertSource) value = AutoConvert(value, _targetType);
                        _targetMember.Set(_safeTarget, value);
                    }
                        break;
                    case BindType.TargetUpdateSource:
                    {
                        object value = _targetMember.Get(_safeTarget);
                        if (_convertTarget) value = AutoConvert(value, _sourceType);
                        _sourceMember.Set(_safeSource, value);
                    }
                        break;
                    case BindType.Both:
                    {
                        object sVal = _sourceMember.Get(_safeSource);
                        object tVal = _targetMember.Get(_safeTarget);
                        if (_convertSource) sVal = AutoConvert(sVal, _targetType);
                        if (_convertTarget) tVal = AutoConvert(tVal, _sourceType);
                        if (!Equals(sVal, _sourceVal))
                            _targetMember.Set(_safeTarget, sVal);
                        else if (!Equals(tVal, _targetVal))
                            _sourceMember.Set(_safeSource, tVal);

                        _sourceVal = sVal;
                        _targetVal = tVal;
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private object AutoConvert(object value, Type targetType)
        {
            return Convert.ChangeType(value, targetType);
        }

        private object GetConverted(object value, ConverterType type)
        {
            switch (type)
            {
                case ConverterType.None:
                    return value;
                case ConverterType.ToBoolean:
                    return Convert.ToBoolean(value);
                case ConverterType.ToByte:
                    return Convert.ToByte(value);
                case ConverterType.ToChar:
                    return Convert.ToChar(value);
                case ConverterType.ToDateTime:
                    return Convert.ToDateTime(value);
                case ConverterType.ToDecimal:
                    return Convert.ToDecimal(value);
                case ConverterType.ToDouble:
                    return Convert.ToDouble(value);
                case ConverterType.ToInt16:
                    return Convert.ToInt16(value);
                case ConverterType.ToInt32:
                    return Convert.ToInt32(value);
                case ConverterType.ToInt64:
                    return Convert.ToInt64(value);
                case ConverterType.ToSByte:
                    return Convert.ToSByte(value);
                case ConverterType.ToSingle:
                    return Convert.ToSingle(value);
                case ConverterType.ToString:
                    return Convert.ToString(value);
                case ConverterType.ToUInt16:
                    return Convert.ToUInt16(value);
                case ConverterType.ToUInt32:
                    return Convert.ToUInt32(value);
                case ConverterType.ToUInt64:
                    return Convert.ToUInt64(value);
                default:
                    throw new ArgumentOutOfRangeException("type");
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

    public enum ConverterType
    {
        None,
        ToBoolean,
        ToByte,
        ToChar,
        ToDateTime,
        ToDecimal,
        ToDouble,
        ToInt16,
        ToInt32,
        ToInt64,
        ToSByte,
        ToSingle,
        ToString,
        ToUInt16,
        ToUInt32,
        ToUInt64,
    }
}