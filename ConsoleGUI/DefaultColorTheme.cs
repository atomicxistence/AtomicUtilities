using System;

namespace ConsoleGUI
{
	class DefaultColorTheme : IColorTheme
	{
		public ConsoleColor ColorSubMenuBG => ConsoleColor.DarkGray;
		public ConsoleColor ColorSubMenuFG => ConsoleColor.White;
		public ConsoleColor ColorTitleBG => ConsoleColor.DarkGray;
		public ConsoleColor ColorTitleFG => ConsoleColor.Cyan;
		public ConsoleColor ColorPromptFG => ConsoleColor.Cyan;
		public ConsoleColor ColorPromptBG => ConsoleColor.DarkGray;
		public ConsoleColor ColorTaskActioned => ConsoleColor.DarkGray;
		public ConsoleColor ColorTaskSelectedBG => ConsoleColor.White;
		public ConsoleColor ColorTaskSelectedFG => ConsoleColor.DarkYellow;
		public ConsoleColor ColorTextEntryBG => ConsoleColor.White;
		public ConsoleColor ColorTextEntryFG => ConsoleColor.Black;
		public ConsoleColor ColorDefaultFG => ConsoleColor.Black;
		public ConsoleColor ColorDefaultBG => ConsoleColor.Gray;
	}
}
