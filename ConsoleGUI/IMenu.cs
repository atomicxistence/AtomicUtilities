using System.Collections.Generic;

namespace Atomic.ConsoleGUI
{
	/// <summary>
	/// Defines a menu list of menu items
	/// </summary>
	public interface IMenu
	{
		/// <summary>
		/// The list of menu items
		/// </summary>
		List<IMenuItem> MenuItems { get; set; }
	}
}
