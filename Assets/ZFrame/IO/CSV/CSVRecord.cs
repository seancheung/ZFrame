using System.Collections.Generic;
using System.Linq;

public class CSVRecord
{
	public int Count
	{
		get { return recordList.Count; }
	}

	protected List<List<string>> recordList;

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

	public bool IsMatrix
	{
		get { return Count > 0 && recordList.TrueForAll(c => c.Count == recordList[0].Count); }
	}
}