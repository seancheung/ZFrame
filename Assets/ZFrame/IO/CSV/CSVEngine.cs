using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using ZFrame.Debugger;

namespace ZFrame.IO.CSV
{
	internal enum CSVValueType
	{
		String,
		Int32,
		Int64,
		Single,
		Double,
		Boolean,
		Array,
		Object
	}

	public class CSVEngine
	{
		private List<List<string>> _records;

		/// <summary>
		/// Get column count
		/// </summary>
		public int ColumnCount { get; private set; }

		/// <summary>
		/// Get row count
		/// </summary>
		public int RowCount { get; private set; }

		/// <summary>
		/// Get separator
		/// </summary>
		public char Separator { get; private set; }

		public bool CanRead
		{
			get { return _records != null && ColumnCount > 0 && RowCount > 0; }
		}

		private int _keyRow = -1;
		private int _descRow = -1;
		private int _startRow = -1;

		/// <summary>
		/// Decode CSV file to target mapped type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <returns></returns>
		public IEnumerable<T> Decode<T>() where T : new()
		{
			if (_records == null || _keyRow < 0 || _descRow < 0 || _startRow < 0)
			{
				ZDebug.LogError(string.Format("Decoding Failed: {0}", typeof (T)));
				yield break;
			}

			//Decode each row
			for (int i = _startRow; i < _records.Count; i++)
			{
				if (i == _keyRow || i == _descRow)
					continue;
				yield return DecodeRow<T>(_records[i], _records[_keyRow]);
			}
		}

		/// <summary>
		/// Decode single row
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fields"></param>
		/// <param name="keys"></param>
		/// <returns></returns>
		private T DecodeRow<T>(List<string> fields, List<string> keys) where T : new()
		{
			T result = new T();
			IEnumerable<MemberInfo> members =
				typeof (T).GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					.Where(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field)
					.Where(m => Attribute.IsDefined(m, typeof (CSVColumnAttribute), false));

			if (typeof (T).IsValueType)
			{
				object boxed = result;
				foreach (MemberInfo member in members)
				{
					CSVColumnAttribute attribute =
						member.GetCustomAttributes(typeof (CSVColumnAttribute), false).First() as CSVColumnAttribute;
					string field = GetRawValue(attribute, fields, keys, member.Name);
					if (ReferenceEquals(field, member.Name))
						return result;
					SetValue(member, boxed, field, attribute.DefaultValue, attribute.ArraySeparator, attribute.TrueValues);
				}
				return (T) boxed;
			}

			foreach (MemberInfo member in members)
			{
				CSVColumnAttribute attribute =
					member.GetCustomAttributes(typeof (CSVColumnAttribute), false).First() as CSVColumnAttribute;
				string field = GetRawValue(attribute, fields, keys, member.Name);
				if (ReferenceEquals(field, member.Name))
					return result;
				SetValue(member, result, field, attribute.DefaultValue, attribute.ArraySeparator, attribute.TrueValues);
			}
			return result;
		}

		/// <summary>
		/// Get raw value by CSVColumnAttribute or name
		/// </summary>
		/// <param name="attribute"></param>
		/// <param name="fields"></param>
		/// <param name="keys"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private string GetRawValue(CSVColumnAttribute attribute, List<string> fields, List<string> keys, string name)
		{
			if (attribute.Column >= 0 && fields.Count > attribute.Column)
			{
				return fields[attribute.Column];
			}
			if (!string.IsNullOrEmpty(attribute.Key) && keys.Contains(attribute.Key))
			{
				return fields[keys.IndexOf(attribute.Key)];
			}
			if (keys.Contains(name))
			{
				return fields[keys.IndexOf(name)];
			}
			ZDebug.LogError(string.Format("Mapping Error! Column: {0}, Key: {1}, Name:{2}", attribute.Column,
				attribute.Key ?? "NULL", name));
			return name;
		}

		/// <summary>
		/// Parse and set raw value
		/// </summary>
		/// <param name="member"></param>
		/// <param name="obj"></param>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <param name="arraySeparator"></param>
		private void SetValue(MemberInfo member, object obj, string value, object defaultValue, char arraySeparator,
			string[] trueValues)
		{
			if (member.MemberType == MemberTypes.Property)
			{
				if ((member as PropertyInfo).CanWrite)
				{
					ZDebug.LogError(string.Format("CSV property must be writable! CSVData type: {0}, Property: {1}",
						member.DeclaringType, member.Name));
					return;
				}
				(member as PropertyInfo).SetValue(obj,
					ParseRawValue(value, (member as PropertyInfo).PropertyType, defaultValue, arraySeparator, trueValues),
					null);
			}
			else
			{
				(member as FieldInfo).SetValue(obj,
					ParseRawValue(value, (member as FieldInfo).FieldType, defaultValue, arraySeparator, trueValues));
			}
		}

		/// <summary>
		/// Parse string value to specified type
		/// </summary>
		/// <param name="field"></param>
		/// <param name="type">If type is collection, use array only(e.g. int[])</param>
		/// <param name="defaultValue">If type is collection, use element default(e.g. 0 for int[])</param>
		/// <param name="arraySeparator"></param>
		/// <returns></returns>
		private object ParseRawValue(string field, Type type, object defaultValue, char arraySeparator, string[] trueValues)
		{
			try
			{
				if (type.IsArray)
				{
					IEnumerable<object> result =
						field.Split(arraySeparator)
							.Select(f => ParseRawValue(f, type.GetElementType(), defaultValue, arraySeparator, trueValues));
					if (type.GetElementType() == typeof (string))
					{
						return result.Cast<string>().ToArray();
					}
					if (type.GetElementType() == typeof (int))
					{
						return result.Cast<int>().ToArray();
					}
					if (type.GetElementType() == typeof (long))
					{
						return result.Cast<long>().ToArray();
					}
					if (type.GetElementType() == typeof (short))
					{
						return result.Cast<short>().ToArray();
					}
					if (type.GetElementType() == typeof (uint))
					{
						return result.Cast<uint>().ToArray();
					}
					if (type.GetElementType() == typeof (ulong))
					{
						return result.Cast<ulong>().ToArray();
					}
					if (type.GetElementType() == typeof (ushort))
					{
						return result.Cast<ushort>().ToArray();
					}
					if (type.GetElementType() == typeof (float))
					{
						return result.Cast<float>().ToArray();
					}
					if (type.GetElementType() == typeof (double))
					{
						return result.Cast<double>().ToArray();
					}
					if (type.GetElementType() == typeof (bool))
					{
						return result.Cast<bool>().ToArray();
					}
					return null;
				}
				if (type == typeof (string))
				{
					return field;
				}
				if (type == typeof (int))
				{
					return Convert.ToInt32(field);
				}
				if (type == typeof (long))
				{
					return Convert.ToInt64(field);
				}
				if (type == typeof (short))
				{
					return Convert.ToInt16(field);
				}
				if (type == typeof (uint))
				{
					return Convert.ToUInt32(field);
				}
				if (type == typeof (ulong))
				{
					return Convert.ToUInt64(field);
				}
				if (type == typeof (ushort))
				{
					return Convert.ToUInt16(field);
				}
				if (type == typeof (float))
				{
					return Convert.ToSingle(field);
				}
				if (type == typeof (double))
				{
					return Convert.ToDouble(field);
				}
				if (type == typeof (bool))
				{
					if (field == null)
					{
						return false;
					}
					field = field.Trim();
					return trueValues != null && trueValues.Contains(field, StringComparer.CurrentCultureIgnoreCase);
				}
			}
			catch (FormatException ex)
			{
				ZDebug.LogWarning(string.Format("{0}: {1} -> {2}", ex.Message, field, type));

				//In case default value is null but the property/field is not a reference type
				if (defaultValue == null)
				{
					if (type == typeof (int) || type == typeof (float) || type == typeof (double))
					{
						defaultValue = -1;
					}
					else if (type == typeof (bool))
					{
						defaultValue = false;
					}
				}
			}

			return defaultValue;
		}

		/// <summary>
		/// Load string content
		/// </summary>
		/// <param name="content"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public bool Load(string content, char separator = ',')
		{
			//Dispose records
			ClearRecord();
			if (string.IsNullOrEmpty(content))
			{
				ZDebug.LogError(string.Format("CSV file content empty!"));
				return false;
			}

			var check = CheckLegal(content, separator);
			if (!check) return false;

			Separator = separator;
			_records = new List<List<string>>();
			foreach (string row in content.Split('\r').Where(line => !string.IsNullOrEmpty(line.Trim())))
			{
				List<string> columns = row.Split(separator).Select(s => s.Trim()).ToList();
				//Check each row's column count. They must match
				if (ColumnCount != 0 && columns.Count != ColumnCount)
				{
					ZDebug.LogError(
						string.Format("CSV parsing error at line {0} : columns counts do not match! Separator: '{1}'",
							content.IndexOf(row), separator));
					return false;
				}
				ColumnCount = columns.Count;
				_records.Add(columns);
			}
			RowCount = _records.Count;

			if (_records == null || !_records.Any())
			{
				ZDebug.LogWarning(string.Format("CSV file parsing failed(empty records)!"));
				return false;
			}

			return true;
		}

		/// <summary>
		/// Load mapped class
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public bool Load<T>()
		{
			ClearRecord();

			//Check mapping
			if (!Attribute.IsDefined(typeof (T), typeof (CSVMapperAttribute), false))
			{
				ZDebug.LogError(string.Format("CSV mapping attribute not found in type: {0}", typeof (T)));
				return false;
			}

			CSVMapperAttribute mapper =
				Attribute.GetCustomAttribute(typeof (T), typeof (CSVMapperAttribute), false) as CSVMapperAttribute;
			_keyRow = mapper.KeyRow;
			_descRow = mapper.DescRow;
			_startRow = mapper.StartRow;

			if (string.IsNullOrEmpty(mapper.Path))
			{
				ZDebug.LogError(string.Format("CSV path not found: {0}", mapper.Path));
				return false;
			}

			//Read text
			TextAsset asset = Resources.Load<TextAsset>(mapper.Path);

			if (asset == null)
			{
				ZDebug.LogError(string.Format("CSV file not found: {0}", mapper.Path));
				return false;
			}

			string content = asset.text;
			bool result = Load(content, mapper.Separator);

			if (result)
			{
				if (_records[_keyRow].Any(string.IsNullOrEmpty))
				{
					ZDebug.LogError(
						string.Format("Encoding Error! No key column found. Make sure target file is in UTF-8 format. Path: {0}",
							mapper.Path));
					return false;
				}
			}
			return result;
		}

		/// <summary>
		/// Get string value at specified row and column. If record empty or position not found, NULL will be returned. Row/Column starts at 0
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public string this[int row, int column]
		{
			get
			{
				if (_records == null || _records.Count <= row || _records[row].Count <= column)
				{
					return null;
				}
				return _records[row][column];
			}
		}

		/// <summary>
		/// Get a converted value at specified row and column. If record empty or position not found or convertion failed, defaultValue will be returned. Row/Column starts at 0
		/// </summary>
		/// <typeparam name="T">If T is collection, use array only(e.g. int[])</typeparam>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <param name="defaultValue">If T is collection, use element default(e.g. 0 for int[])</param>
		/// <param name="arraySeparator"></param>
		/// <param name="trueValues"></param>
		/// <returns></returns>
		public T Read<T>(int row, int column, object defaultValue, char arraySeparator = '#', string[] trueValues = null)
		{
			if (trueValues == null)
			{
				trueValues = new[] {"1", "true"};
			}
			string field = this[row, column];
			if (field == null)
			{
				Debug.LogWarning("Field is null. Make sure csv is loaded and field has content.");
				return typeof (T).IsArray ? default(T) : (T) defaultValue;
			}

			return (T) ParseRawValue(field, typeof (T), defaultValue, arraySeparator, trueValues);
		}


		/// <summary>
		/// Remove all records.
		/// </summary>
		public void ClearRecord()
		{
			_records = null;
			RowCount = 0;
			ColumnCount = 0;
			Separator = ',';
			_keyRow = -1;
			_descRow = -1;
			_startRow = -1;
		}

		/// <summary>
		/// Chec if given content can be parsed as csv
		/// </summary>
		/// <param name="content"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static bool CheckLegal(string content, char separator = ',')
		{
			if (string.IsNullOrEmpty(content)) return false;
			int rowChar = Regex.Matches(content, "\r").Count;
			int colChar = Regex.Matches(content, separator.ToString()).Count;

			return rowChar > 0 && colChar > 0 && colChar % rowChar == 0;
		}

		#region Overrides of Object

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			if (CanRead)
			{
				string content = "";
				foreach (List<string> record in _records)
				{
					content = record.Aggregate(content, (current, col) => current + (col + ","));
					content = content.TrimEnd(',');
					content += "\r\n";
				}

				return content;
			}

			return string.Empty;
		}

		#endregion
	}
}