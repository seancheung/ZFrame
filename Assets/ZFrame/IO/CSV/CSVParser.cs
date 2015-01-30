using System;
using System.Collections.Generic;
using System.ComponentModel;
using ZFrame;

public static class CSVParser
{
	private static readonly Dictionary<string, Type> TypeDict = new Dictionary<string, Type>
	{
		{"int", typeof (Int32)},
		{"short", typeof (Int16)},
		{"long", typeof (Int64)},
		{"float", typeof (Single)},
		{"double", typeof (Double)},
		{"string", typeof (String)},
		{"bool", typeof (Boolean)},
		{"datetime", typeof (DateTime)},
		{"enum", typeof (Enum)},
		{"object", typeof (Object)}
	};

	public static T Convert<T>(string content)
	{
		return (T) Convert(typeof (T), content);
	}

	public static object Convert(Type type, string content)
	{
		TypeConverter converter = TypeDescriptor.GetConverter(type);
		return converter.ConvertFromInvariantString(content);
	}

	public static object Convert(string type, string content)
	{
		Type tp = TypeDict.TryGet(type.ToLower());
		if (tp == null) return null;

		return Convert(tp, content);
	}
}