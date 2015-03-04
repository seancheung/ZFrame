using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ZFrame.IO.CSV;

public class CSVParser
{
    public static IEnumerable<T> Parse<T>() where T : new()
    {
        //Check mapping
        if (!Attribute.IsDefined(typeof (T), typeof (CSVMapperAttribute), false))
            throw new ArgumentException("CSV mapping attribute not found", typeof (T).ToString());

        CSVMapperAttribute mapper =
            Attribute.GetCustomAttribute(typeof (T), typeof (CSVMapperAttribute), false) as CSVMapperAttribute;

        if (string.IsNullOrEmpty(mapper.Path))
            throw new ArgumentException("CSV path not found", typeof (T).ToString());

        TextAsset asset = Resources.Load<TextAsset>(mapper.Path);

        if (asset == null)
            throw new ArgumentException("CSV file not found", typeof (T).ToString());

        if (string.IsNullOrEmpty(asset.text))
            throw new ArgumentException("CSV file content empty", typeof (T).ToString());

        CSVReader reader = new CSVReader();
        CSVRecord csv = reader.Read(asset.text);

        IEnumerable<MemberInfo> members =
            typeof (T).GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field)
                .Where(m => Attribute.IsDefined(m, typeof (CSVColumnAttribute), false));

        foreach (List<string> record in csv)
        {
            T obj = new T();

            foreach (MemberInfo member in members)
            {
                CSVColumnAttribute attribute =
                    member.GetCustomAttributes(typeof (CSVColumnAttribute), false).First() as CSVColumnAttribute;
                string value = GetValue(attribute, csv[mapper.KeyRow], record, member.Name, typeof (T));

                SetValue(member, obj, value, attribute.DefaultValue, attribute.ArraySeparator, attribute.TrueValues,
                    typeof (T));
            }

            yield return obj;
        }
    }

    private static void SetValue(MemberInfo member, object obj, string value, object defaultValue, char arraySeparator,
        string[] trueValues, Type type)
    {
        if (member.MemberType == MemberTypes.Property)
        {
            MethodInfo set = ((PropertyInfo) member).GetSetMethod(true);
            if (set == null)
                throw new ArgumentException("CSV property " + member.Name + " must be writable", type.ToString());
            try
            {
                if (((PropertyInfo) member).PropertyType.IsArray)
                {
                    object[] val =
                        ZConverter.ToObjects(((PropertyInfo) member).PropertyType, value.Split(arraySeparator))
                            .ToArray();
                    ((PropertyInfo) member).SetValue(obj, val, null);
                }
                else if (((PropertyInfo) member).PropertyType == typeof (Boolean))
                {
                    ((PropertyInfo) member).SetValue(obj, trueValues.Contains(value), null);
                }
                else
                {
                    object val = ZConverter.ToObject(((PropertyInfo) member).PropertyType, value);
                    ((PropertyInfo) member).SetValue(obj, val, null);
                }
            }
            catch (NotSupportedException)
            {
                ((PropertyInfo) member).SetValue(obj, defaultValue, null);
            }
        }
        else
        {
            try
            {
                if (((FieldInfo) member).FieldType.IsArray)
                {
                    object val = ZConverter.ToObjects(((FieldInfo) member).FieldType, value.Split(arraySeparator));
                    ((FieldInfo) member).SetValue(obj, val);
                }
                else if (((FieldInfo) member).FieldType == typeof (Boolean))
                {
                    ((FieldInfo) member).SetValue(obj, trueValues.Contains(value));
                }
                else
                {
                    object val = ZConverter.ToObject(((FieldInfo) member).FieldType, value);
                    ((FieldInfo) member).SetValue(obj, val);
                }
            }
            catch (NotSupportedException)
            {
                ((FieldInfo) member).SetValue(obj, defaultValue);
            }
        }
    }

    private static string GetValue(CSVColumnAttribute attribute, IList<string> keys, List<string> record, string name,
        Type type)
    {
        if (attribute.Column >= 0 && record.Count > attribute.Column)
        {
            return record[attribute.Column];
        }
        if (!string.IsNullOrEmpty(attribute.Key) && keys.Contains(attribute.Key))
        {
            return record[keys.IndexOf(attribute.Key)];
        }
        if (keys.Contains(name))
        {
            return record[keys.IndexOf(name)];
        }

        Debug.LogError(string.Format("Mapping Error! Column: {0}, Key: {1}, Name:{2}", attribute.Column,
            attribute.Key ?? "NULL", name));

        throw new ArgumentException("Mapping key not found on member" + name, type.ToString());
    }
}