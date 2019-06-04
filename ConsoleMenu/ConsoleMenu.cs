using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
	public class ConsoleMenu<T>
	{
		public ConsoleMenuItem<T>[] _MenuItems { get; set; }
		private string _Description;
		private int _SelectedItemIndex = 0;
		private bool _ItemIsSelcted = false;

		public ConsoleMenu(string description, IEnumerable<ConsoleMenuItem<T>> menuItems)
		{
			_MenuItems = menuItems.ToArray();
			_Description = description;
		}

		public void RunConsoleMenu()
		{
			if (!string.IsNullOrEmpty(_Description))
			{
				Console.WriteLine($"{_Description}: {Environment.NewLine}");
			}

			StartConsoleDrawindLoopUntilInputIsMade();
			
			// reset the selection flag so we can re-draw the same console if required after selection
			_ItemIsSelcted = false;
			_MenuItems[_SelectedItemIndex].CallBack.Invoke(_MenuItems[_SelectedItemIndex].UnderlyingObject);
		}

		private void StartConsoleDrawindLoopUntilInputIsMade()
		{
			int topOffset = Console.CursorTop;
			int bottomOffset = 0;
			ConsoleKeyInfo kb;
			Console.CursorVisible = false;


			while (!_ItemIsSelcted)
			{
				for (int i = 0; i < _MenuItems.Length; i++)
				{
					WriteConsoleItem(i, _SelectedItemIndex);
				}

				bottomOffset = Console.CursorTop;
				kb = Console.ReadKey(true);
				HandleKeyPress(kb.Key);

				//Reset the cursor to the top of the screen
				Console.SetCursorPosition(0, topOffset);
			}

			//set the cursor just after the menu so that the program can continue after the menu
			Console.SetCursorPosition(0, bottomOffset);
			Console.CursorVisible = true;
		}

		private void HandleKeyPress(ConsoleKey pressedKey)
		{
			switch (pressedKey)
			{
				case ConsoleKey.UpArrow:
					_SelectedItemIndex = (_SelectedItemIndex == 0) ? _MenuItems.Length - 1 : _SelectedItemIndex - 1;
					break;

				case ConsoleKey.DownArrow:
					_SelectedItemIndex = (_SelectedItemIndex == _MenuItems.Length - 1) ? 0 : _SelectedItemIndex + 1;
					break;

				case ConsoleKey.Enter:
					_ItemIsSelcted = true;
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

			Console.WriteLine(" {0,-20}", this._MenuItems[itemIndex].Name);
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
