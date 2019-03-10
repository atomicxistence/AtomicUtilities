using System.Collections.Generic;
using ConsoleGUI;

namespace TestApp
{
	class Menu : IMenu
	{
		public List<IMenuItem> MenuItems { get; set; }

		public Menu()
		{
			MenuItems = new List<IMenuItem>();
		}
	}
}
