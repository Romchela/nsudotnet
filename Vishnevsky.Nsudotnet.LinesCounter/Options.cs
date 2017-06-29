using CommandLineParser.Arguments;

namespace LineCounter
{
	public class Options
	{

		[SwitchArgument('h', "help", false, Description = "Show help.")]
		public bool Help;

		[ValueArgument(typeof(string), 'd', "dir", DefaultValue = null, Description = "Directory.")]
		public string Directory { set; get; }

		[ValueArgument(typeof(string), 'm', "mask", DefaultValue = "*.*", Description = "File mask e.g. '*.cs'")]
		public string FileFilter { set; get; }

		[ValueArgument(typeof(string), 'r', "rexp", DefaultValue = null, Description = @"Line exception regular expression e.g. '^\s{0,}[/][/]' to exclude comments.")]
		public string RowFilter { set; get; }
	}
}