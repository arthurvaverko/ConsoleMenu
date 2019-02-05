using ConsoleUI;
using System;
using System.IO;
using System.Linq;

namespace ConsoleMenuTest
{
	class Program
	{
		static void Main(string[] args)
		{
			//var items = Enumerable.Range(1, 20).Select(i => new ConsoleMenuItem<string>($"Item{i}", ItemCallback, $"Item{ i }"));
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
				return new ConsoleMenuItem<DirectoryInfo>(dirName, DirectoryCallback, dirInfo);
			});

			var menu = new ConsoleMenu<DirectoryInfo>($"DIR: {Path.GetFileName(rootDirPath)}....", dirs, true);
			var userPressedEscape = !menu.RunConsoleMenu();
			if (userPressedEscape && !IsPathRootDirectory(rootDirPath))
			{
				Console.Clear();
				OpenDirectoryBrowserConsole(Directory.GetParent(rootDirPath).FullName);
			}
		}

		private static bool IsPathRootDirectory(string rootDirPath)
		{
			return Path.GetPathRoot(rootDirPath).Equals(rootDirPath, StringComparison.CurrentCultureIgnoreCase);
		}

		private static void DirectoryCallback(DirectoryInfo selectedDirInfo)
		{
			Console.Clear();
			OpenDirectoryBrowserConsole(selectedDirInfo.FullName);
		}

		private static void ItemCallback(string itemClicked)
		{
			Console.Clear();
			Console.WriteLine(itemClicked);
		}
	}
}
