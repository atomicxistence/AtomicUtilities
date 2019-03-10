using System;

namespace ConsoleGUI
{
	/// <summary>
	/// Defines the set of color values used in ConsoleGUI.Display
	/// </summary>
	public interface IColorTheme
	{
		/// <summary>
		/// The color used for the sub menu background
		/// </summary>
		ConsoleColor ColorSubMenuBG { get; }
		/// <summary>
		/// The color used for the sub menu text
		/// </summary>
		ConsoleColor ColorSubMenuFG { get; }
		/// <summary>
		/// The color used for the title background
		/// </summary>
		ConsoleColor ColorTitleBG { get; }
		/// <summary>
		/// The color used for the title text
		/// </summary>
		ConsoleColor ColorTitleFG { get; }
		/// <summary>
		/// The color used for the prompt message text
		/// </summary>
		ConsoleColor ColorPromptFG { get; }
		/// <summary>
		/// The color used for the prompt message background
		/// </summary>
		ConsoleColor ColorPromptBG { get; }
		/// <summary>
		/// The color used for the currently selected menu item's background
		/// </summary>
		ConsoleColor ColorItemSelectedBG { get; }
		/// <summary>
		/// The color used for the currently selected menu item's text
		/// </summary>
		ConsoleColor ColorItemSelectedFG { get; }
		/// <summary>
		/// The color used for the user text entry background
		/// </summary>
		ConsoleColor ColorTextEntryBG { get; }
		/// <summary>
		/// The color used for the user text entry text
		/// </summary>
		ConsoleColor ColorTextEntryFG { get; }
		/// <summary>
		/// The color used for the default text
		/// </summary>
		ConsoleColor ColorDefaultFG { get; }
		/// <summary>
		/// The color used for the default background
		/// </summary>
		ConsoleColor ColorDefaultBG { get; }
	}
}
