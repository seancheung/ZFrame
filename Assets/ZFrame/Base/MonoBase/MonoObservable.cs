using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public abstract class MonoObservable : MonoBehaviour
{
    public event Action<string, object> PropertyChangedHandler;

    protected virtual void OnPropertyChanged(string propName, object value)
    {
        if (PropertyChangedHandler != null) PropertyChangedHandler.Invoke(propName, value);
    }

    protected void OnPropertyChanged<T>(Expression<Func<T>> exp, object oldvalue)
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

        OnPropertyChanged(propInfo.Name, propInfo.GetValue(this, null));
    }
}