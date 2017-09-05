# Console Menu

A console base UI menu (old school) navigation using arrow keys

# How it looks

![Gif animation](https://gfycat.com/gifs/detail/FlawedForkedHochstettersfrog)
![](https://gfycat.com/gifs/detail/FlawedForkedHochstettersfrog)

# Usage example
```C#
using ConsoleUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMenuTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var items = Enumerable.Range(1,20).Select(i => new ConsoleMenuItem($"Item{i}", itemCallback));
			var menu = new ConsoleMenu("This is a test menu", items);
			menu.RunConsoleMenu();
		}

		private static void itemCallback(string itemClicked)
		{
			Console.Clear();
			Console.WriteLine(itemClicked);
		}
	}
}
```


