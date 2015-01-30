using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class CSVReader
{
	private const string ColMatch = @"""[^""\r\n]*""|'[^'\r\n]*'|[^,\r\n]*";
	private const string LineMatch = "^.*$";

	private IEnumerable<string> GetLines(string content)
	{
		return from Match match in Regex.Matches(content, LineMatch, RegexOptions.Multiline)
			where !string.IsNullOrEmpty(match.Value)
			select match.Value;
	}

	private IEnumerable<string> GetCols(string line)
	{
		Match match = Regex.Match(line, ColMatch);
		Match prevMatch = match;
		while (match.Success)
		{
			//check empty caused by regex anchor match
			if (match.Length != 0 || match.Index != line.Length && match.Index != prevMatch.Index + prevMatch.Length)
				yield return match.Value.TrimStart('"').TrimEnd('"');

			prevMatch = match;
			match = match.NextMatch();
		}
	}

	public CSVRecord Read(string content)
	{
		CSVRecord csv = new CSVRecord();
		foreach (string line in GetLines(content))
		{
			csv.Add(GetCols(line));
		}
		return csv;
	}
}