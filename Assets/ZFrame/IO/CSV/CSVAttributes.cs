using System;

namespace ZFrame.IO.CSV
{
    /// <summary>
    /// CSV Index mapping
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CSVColumnAttribute : Attribute
    {
        /// <summary>
        /// Name of this property/field in csv file(default is property name)
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Index of this property/field in csv file(if Index is assigned, key will be ignored)
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Default value(if reading NULL or failed;deault: -1 for number value, null for class, false for bool)
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Separator for parsing if it's an array('#' by default)
        /// </summary>
        public char ArraySeparator { get; set; }

        /// <summary>
        /// True values for bool type parsing. Default is '1' and 'true'
        /// </summary>
        public string[] TrueValues { get; set; }

        public CSVColumnAttribute(string key)
        {
            Key = key;
            Index = -1;
            ArraySeparator = '#';
            TrueValues = new[] {"1", "true"};
        }

        public CSVColumnAttribute(int index)
        {
            Index = index;
            ArraySeparator = '#';
            TrueValues = new[] {"1", "true"};
        }
    }

    /// <summary>
    /// CSV Mapping class or struct(Try avoid struct as possible. Struct is boxed then unboxed in reflection)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class CSVDocumentAttribute : Attribute
    {
        /// <summary>
        /// Path of the CSV file(without file extension). Base directory is Assets/Resources/
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Rows to skipped
        /// </summary>
        public int[] SkippedRows { get; set; }

        /// <summary>
        /// Mapping key row(0 by default)
        /// </summary>
        public int MappingRow { get; set; }

        public CSVDocumentAttribute(string path)
        {
            Path = path;
            MappingRow = 0;
        }
    }
}