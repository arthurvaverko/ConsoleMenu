using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
	public class ConsoleMenu<T>
	{
		public ConsoleMenuItem<T>[] MenuItems { get; set; }
		private string Description;
		private int selectedItemIndex = 0;
		private bool loopComplete = false;

		public ConsoleMenu(string description, IEnumerable<ConsoleMenuItem<T>> menuItems)
		{
			MenuItems = menuItems.ToArray();
			Description = description;
		}

		public void RunConsoleMenu()
		{
			//this will resise the console if the amount of elements in the list are too big
			if ((MenuItems.Count()) > Console.WindowHeight)
			{
				//TODO: Deal with console pagging...
			}

			if (!string.IsNullOrEmpty(Description))
			{
				Console.WriteLine($"{Description}: {Environment.NewLine}");
			}

			int topOffset = Console.CursorTop;
			int bottomOffset = 0;
			ConsoleKeyInfo kb;
			Console.CursorVisible = false;


			while (!loopComplete)
			{
				for (int i = 0; i < MenuItems.Length; i++)
				{
					WriteConsoleItem(i, selectedItemIndex);
				}

				//bottomOffset = Console.CursorTop;
				kb = Console.ReadKey(true);
				HandleKeyPress(kb.Key);

				//Reset the cursor to the top of the screen
				Console.SetCursorPosition(0, topOffset);
			}

			//set the cursor just after the menu so that the program can continue after the menu
			Console.SetCursorPosition(0, bottomOffset);
			Console.CursorVisible = true;
			MenuItems[selectedItemIndex].CallBack.Invoke(MenuItems[selectedItemIndex].UnderlyingObject);
		}

		private void HandleKeyPress(ConsoleKey pressedKey)
		{
			switch (pressedKey)
			{
				case ConsoleKey.UpArrow:
					selectedItemIndex = (selectedItemIndex == 0) ? MenuItems.Length - 1 : selectedItemIndex - 1;
					break;

				case ConsoleKey.DownArrow:
					selectedItemIndex = (selectedItemIndex == MenuItems.Length - 1) ? 0 : selectedItemIndex + 1;
					break;

				case ConsoleKey.Enter:
					loopComplete = true;
					break;
			}
		}

		private void WriteConsoleItem(int itemIndex, int selectedItemIndex)
		{
			if (itemIndex == selectedItemIndex)
			{
				Console.BackgroundColor = ConsoleColor.Gray;
				Console.ForegroundColor = ConsoleColor.Black;
			}

			Console.WriteLine(" {0,-20}", this.MenuItems[itemIndex].Name);
			Console.ResetColor();
		}
	}


	public class ConsoleMenuItem<T>
	{
		public string Name { get; set; }
		public Action<T> CallBack { get; set; }
		public T UnderlyingObject { get; set; }

		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ UnderlyingObject.GetHashCode();
		}
		public override bool Equals(object obj)
		{
			// Check for null values and compare run - time types.
			if (obj == null || GetType() != obj.GetType())
				return false;

			var item = (ConsoleMenuItem<T>)obj;
			return item.GetHashCode() == this.GetHashCode();
		}

		public override string ToString()
		{
			return $"{Name} (data: {UnderlyingObject.ToString()})";
		}

		public ConsoleMenuItem(string label, Action<T> callback, T underlyingObject)
		{
			Name = label;
			CallBack = callback;
			UnderlyingObject = underlyingObject;
		}
	}

}
