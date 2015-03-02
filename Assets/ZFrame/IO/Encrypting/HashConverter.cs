using System;
using System.ComponentModel;
using System.Linq;

public static class HashConverter
{
    private const string ArraySep = "%|%";
    private static readonly string[] ContSeps = {"{%", "%}"};

    public static string ToHash(IConvertible value)
    {
        Type type = value.GetType();
        TypeConverter converter = TypeDescriptor.GetConverter(type);
        return type + ContSeps[0] + converter.ConvertToString(value) + ContSeps[1];
    }

    public static string ToHash<T>(T[] value)
    {
        Type type = value.GetType();
        TypeConverter converter = TypeDescriptor.GetConverter(typeof (T));
        string exp = string.Join(ArraySep, Array.ConvertAll(value, input => converter.ConvertToString(input)));

        return type + ContSeps[0] + exp + ContSeps[1];
    }

    public static object ToObject(string value)
    {
        string[] pms = value.Split(ContSeps, StringSplitOptions.RemoveEmptyEntries);
        if (pms.Length == 2)
        {
            Type type = Type.GetType(pms[0]);
            if (type != null)
            {
                if (type.IsArray)
                {
                    Type t = type.GetElementType();
                    TypeConverter tc = TypeDescriptor.GetConverter(t);
                    return Array.ConvertAll(pms[1].Split(new[] {ArraySep}, StringSplitOptions.None),
                        input => tc.ConvertFromString(input));
                }
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                return converter.ConvertFromString(pms[1]);
            }

            throw new ArgumentException(value);
        }

        throw new ArgumentException(value);
    }


    public static T ToObject<T>(string value) where T : IConvertible
    {
        string[] pms = value.Split(ContSeps, StringSplitOptions.RemoveEmptyEntries);
        if (pms.Length == 2)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof (T));
            return (T) converter.ConvertFromString(pms[1]);
        }

        throw new ArgumentException(value);
    }

    public static T ToObject<T>(string value, Converter<string, T> converter)
    {
        string[] pms = value.Split(ContSeps, StringSplitOptions.RemoveEmptyEntries);
        if (pms.Length == 2)
        {
            return converter.Invoke(pms[1]);
        }

        throw new ArgumentException(value);
    }

    public static T[] ToObjects<T>(string value) where T : IConvertible
    {
        string[] pms = value.Split(ContSeps, StringSplitOptions.RemoveEmptyEntries);
        if (pms.Length == 2)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(typeof (T));
            return Array.ConvertAll(pms[1].Split(new[] {ArraySep}, StringSplitOptions.None),
                input => tc.ConvertFromString(input)).Cast<T>().ToArray();
        }

        throw new ArgumentException(value);
    }

    public static T[] ToObjects<T>(string value, Converter<string, T> converter)
    {
        string[] pms = value.Split(ContSeps, StringSplitOptions.RemoveEmptyEntries);
        if (pms.Length == 2)
        {
            return Array.ConvertAll(pms[1].Split(new[] {ArraySep}, StringSplitOptions.None),
                converter);
        }

        throw new ArgumentException(value);
    }
}