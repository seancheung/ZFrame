using System;

namespace ZFrame.IO.CSV
{
	/// <summary>
	/// CSV column mapping
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class CSVColumnAttribute : Attribute
	{
		/// <summary>
		/// Name of this property/field in csv file(default is property name)
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Column of this property/field in csv file(if column is assigned, key will be ignored)
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		/// Default value(if reading NULL or failed;deault: -1 for number value, null for class, false for bool)
		/// </summary>
		public object DefaultValue { get; set; }

		/// <summary>
		/// Separator for parsing if it's an array(',' by default)
		/// </summary>
		public char ArraySeparator { get; set; }


		public CSVColumnAttribute()
		{
			Column = -1;
			ArraySeparator = '#';
		}

		public CSVColumnAttribute(string key)
		{
			Key = key;
			Column = -1;
			ArraySeparator = '#';
		}

		public CSVColumnAttribute(int column)
		{
			Column = column;
			ArraySeparator = '#';
		}
	}

	/// <summary>
	/// CSV Mapping class or struct(Try avoid struct as possible. Struct is boxed then unboxed in reflection)
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class CSVMapperAttribute : Attribute
	{
		/// <summary>
		/// Path of the CSV file(without file extension). Base directory is Assets/Resources/
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// Mapping key row(0 by default)
		/// </summary>
		public int KeyRow { get; set; }

		/// <summary>
		/// Description row(1 by default. Will be skipped in decoding. If no desc in file, assign -1)
		/// </summary>
		public int DescRow { get; set; }

		/// <summary>
		/// Separator for csv parsing(',' by default)
		/// </summary>
		public char Separator { get; set; }

		/// <summary>
		/// Starting index of data rows(2 by default)
		/// </summary>
		public int StartRow { get; set; }

		public CSVMapperAttribute()
		{
			KeyRow = 0;
			DescRow = 1;
			Separator = ',';
		}

		public CSVMapperAttribute(string name)
		{
			Path = name;
			KeyRow = 0;
			DescRow = 1;
			Separator = ',';
		}
	}

}