using System;

namespace ConsoleGUI
{
	public interface IColorTheme
	{
		ConsoleColor ColorSubMenuBG { get; }
		ConsoleColor ColorSubMenuFG { get; }
		ConsoleColor ColorTitleBG { get; }
		ConsoleColor ColorTitleFG { get; }
		ConsoleColor ColorPromptFG { get; }
		ConsoleColor ColorPromptBG { get; }
		ConsoleColor ColorTaskActioned { get; }
		ConsoleColor ColorTaskSelectedBG { get; }
		ConsoleColor ColorTaskSelectedFG { get; }
		ConsoleColor ColorTextEntryBG { get; }
		ConsoleColor ColorTextEntryFG { get; }
		ConsoleColor ColorDefaultFG { get; }
		ConsoleColor ColorDefaultBG { get; }
	}
}
