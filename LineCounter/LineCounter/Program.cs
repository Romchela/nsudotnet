﻿using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LineCounter
{
	class Program
	{
		static Boolean IsLineInvolved(string line, string rowRegExp)
		{
			line = line.Trim();
			if (String.IsNullOrEmpty(line))
			{
				return false;
			}

			if (!String.IsNullOrEmpty(rowRegExp))
			{
				Regex regex = new Regex(rowRegExp);
				return !regex.IsMatch(line);
			}

			return true;
		}

		static public int GetFileLines(string filePath, string rowRegExp)
		{
			return File.ReadLines(filePath).Where(line => IsLineInvolved(line, rowRegExp)).Count();
		}

		static public int GetPropjectLines(string dirPath, string fileFilter, string rowFilter)
		{
			DirectoryInfo dir = new DirectoryInfo(dirPath);

			return dir.GetFiles(fileFilter, SearchOption.AllDirectories)
				.Sum(f => GetFileLines(f.FullName, rowFilter));
		}


		static void Main(string[] args)
		{
			try
			{
				var parser = new CommandLineParser.CommandLineParser();
				var options = new Options();
				parser.ExtractArgumentAttributes(options);
				parser.ParseCommandLine(args);
				if (options.Help || String.IsNullOrEmpty(options.Directory))
				{
					parser.ShowUsage();
				}
				else
				{
					int lines = GetPropjectLines(options.Directory, options.FileFilter, options.RowFilter);
					Console.WriteLine(lines);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}
