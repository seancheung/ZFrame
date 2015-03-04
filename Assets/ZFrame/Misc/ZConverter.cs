using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public static class ZConverter
{
    public static object ToObject(string type, string value)
    {
        Type t = Type.GetType(type);
        return ToObject(t, value);
    }

    public static object ToObject(Type type, string value)
    {
        if (type == null)
            throw new ArgumentNullException("type");
        if (type != typeof (IConvertible))
            throw new ArgumentException("type must be IConvertible", type.ToString());
        TypeConverter converter = TypeDescriptor.GetConverter(type);
        return converter.ConvertFromString(value);
    }

    public static T ToObject<T>(string value) where T : IConvertible
    {
        return (T) ToObject(typeof (T), value);
    }

    public static T ToObject<T>(string value, Converter<string, T> converter)
    {
        if (converter == null)
            throw new ArgumentNullException("converter");
        return (T) ToObject(value, input => (object) converter.Invoke(value));
    }

    public static object ToObject(string value, Converter<string, object> converter)
    {
        if (converter == null)
            throw new ArgumentNullException("converter");
        return converter.Invoke(value);
    }

    public static IEnumerable<object> ToObjects(Type type, IEnumerable<string> values)
    {
        return values.Select(value => ToObject(type, value));
    }

    public static IEnumerable<object> ToObjects(string type, IEnumerable<string> values)
    {
        return values.Select(value => ToObject(type, value));
    }

    public static IEnumerable<T> ToObjects<T>(IEnumerable<string> values) where T : IConvertible
    {
        return values.Select(value => ToObject<T>(value));
    }

    public static IEnumerable<object> ToObjects(IEnumerable<string> values, Converter<string, object> converter)
    {
        return values.Select(value => ToObject(value, converter));
    }

    public static IEnumerable<T> ToObjects<T>(IEnumerable<string> values, Converter<string, T> converter)
    {
        return values.Select(value => ToObject(value, converter));
    }
}