using LinesCounter;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LineCounter
{
	class Program
	{
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
					var counter = new Counter();
					int lines = counter.GetProjectLines(options.Directory, options.FileFilter, options.RowFilter);
					Console.WriteLine(lines);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
