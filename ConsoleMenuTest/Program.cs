using ConsoleUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMenuTest
{
	class Program
	{
		static void Main(string[] args)
		{
			//var items = Enumerable.Range(1, 20).Select(i => new ConsoleMenuItem<string>($"Item{i}", itemCallback, $"Item{ i }"));
			//var menu = new ConsoleMenu<string>("This is a test menu", items);
			//menu.RunConsoleMenu();

			OpenDirectoryBrowserConsole(@"C:\");

		}

		private static void OpenDirectoryBrowserConsole(string rootDirPath)
		{
			var dirs = Directory.GetDirectories(rootDirPath).Select(dPath =>
			{
				var dirName = Path.GetFileName(dPath);
				var dirInfo = new DirectoryInfo(dPath);
				return new ConsoleMenuItem<DirectoryInfo>(dirName, directoryCallback, dirInfo);
			});
			var menu = new ConsoleMenu<DirectoryInfo>($"DIR: {Path.GetFileName(rootDirPath)}....", dirs);
			menu.RunConsoleMenu();
		}

		private static void directoryCallback(DirectoryInfo selectedDirInfo)
		{
			Console.Clear();
			OpenDirectoryBrowserConsole(selectedDirInfo.FullName);
		}

		private static void itemCallback(string itemClicked)
		{
			Console.Clear();
			Console.WriteLine(itemClicked);
		}
	}
}
