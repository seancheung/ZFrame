using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZFrame.IO.CSV
{
    public class CSVRecord : IEnumerable<List<string>>
    {
        public int Count
        {
            get { return recordList.Count; }
        }

        protected List<List<string>> recordList = new List<List<string>>();

        public IList<string> this[int index]
        {
            get { return recordList[index]; }
        }

        public void Add(IEnumerable<string> row)
        {
            recordList.Add(row.ToList());
        }

        public void Insert(int index, IEnumerable<string> row)
        {
            recordList.Insert(index, row.ToList());
        }

        public void Remove(int index)
        {
            recordList.RemoveAt(index);
        }

        public int IndexOf(List<string> row)
        {
            return recordList.IndexOf(row);
        }

        public bool IsMatrix
        {
            get { return Count > 0 && recordList.TrueForAll(c => c.Count == recordList[0].Count); }
        }

        public IEnumerator<List<string>> GetEnumerator()
        {
            return recordList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}