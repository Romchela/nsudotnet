using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;



namespace LinesCounter
{
	public class Counter
	{
		private Regex _rowRegExp;

		/// <summary>
		/// Calculate amount of not empty lines in all files in the specified folder
		/// </summary>
		/// <param name="dirPath"></param>
		/// <param name="fileFilter"> e.g. *.cs </param>
		/// <param name="rowFilter">Regular expression to exclude line</param>
		/// <returns></returns>
		public int GetProjectLines(string folder, string fileFilter, string rowFilter)
		{
			DirectoryInfo dir = new DirectoryInfo(folder);

			if (!String.IsNullOrEmpty(rowFilter))
			{
				_rowRegExp = new Regex(rowFilter);
			}

			return dir.EnumerateFiles(fileFilter, SearchOption.AllDirectories).Sum(f => GetFileLines(f.FullName));
		}

		protected int GetFileLines(string filePath)
		{
			return File.ReadLines(filePath).Count(line => IsLineInvolved(line));
		}

		protected Boolean IsLineInvolved(string line)
		{
			if (String.IsNullOrWhiteSpace(line))
			{
				return false;
			}

			if (_rowRegExp != null)
			{
				return !_rowRegExp.IsMatch(line);
			}

			return true;
		}
	}
}
