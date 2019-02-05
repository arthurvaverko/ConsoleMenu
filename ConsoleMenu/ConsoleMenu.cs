using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
    public class ConsoleMenu<T>
    {
        public ConsoleMenuItem<T>[] MenuItems { get; set; }

        private readonly bool enableQuitUsingEscape;
        private string Description;
        private int selectedItemIndex = 0;
        private bool loopComplete = false;


        public ConsoleMenu(string description, IEnumerable<ConsoleMenuItem<T>> menuItems)
            : this(description, menuItems, false)
        {
        }

        public ConsoleMenu(string description, IEnumerable<ConsoleMenuItem<T>> menuItems, bool enableQuitUsingEscape)
        {
            MenuItems = menuItems.ToArray();
            Description = description;
            this.enableQuitUsingEscape = enableQuitUsingEscape;
        }

        /// <summary>
        /// Runs the console menu, waiting for user to chose the desired item
        /// </summary>
        /// <returns>True - if user selected an item from the menu, false - if user used Esc key to cancel the menu</returns>
        public bool RunConsoleMenu()
        {
            //this will resise the console if the amount of elements in the list are too big
            if ((MenuItems.Count()) > Console.WindowHeight)
            {
                //TODO: Deal with console paging...
            }

            if (!string.IsNullOrEmpty(Description))
            {
                Console.WriteLine($"{Description}: {Environment.NewLine}");
            }

            int topOffset = Console.CursorTop;
            int bottomOffset = 0;
            ConsoleKeyInfo kb;
            Console.CursorVisible = false;

            bool menuBailedOut = false;
            while (!loopComplete)
            {
                for (int i = 0; i < MenuItems.Length; i++)
                {
                    WriteConsoleItem(i);
                }

                bottomOffset = Console.CursorTop;
                kb = Console.ReadKey(true);
                menuBailedOut = HandleKeyPress(kb.Key);

                //Reset the cursor to the top of the screen
                Console.SetCursorPosition(0, topOffset);
            }

            //set the cursor just after the menu so that the program can continue after the menu
            Console.SetCursorPosition(0, bottomOffset);
            Console.CursorVisible = true;
            loopComplete = false;

            if (!menuBailedOut)
            {
                MenuItems[selectedItemIndex].CallBack.Invoke(MenuItems[selectedItemIndex].UnderlyingObject);
            }

            return !menuBailedOut;
        }

        private bool HandleKeyPress(ConsoleKey pressedKey)
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

                case ConsoleKey.Escape:
                    if (!enableQuitUsingEscape)
                    {
                        break;
                    }

                    loopComplete = true;
                    return true;
            }

            return false;
        }

        private void WriteConsoleItem(int itemIndex)
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
