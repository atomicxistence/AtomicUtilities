using System.Collections.Generic;

namespace ConsoleGUI
{
	public interface IMenu
	{
		List<IMenuItem> MenuItems { get; set; }
	}
}
