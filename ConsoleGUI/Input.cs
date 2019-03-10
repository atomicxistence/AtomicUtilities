using System;

namespace ConsoleGUI
{
	public static class Input
	{
		/// <summary>
		/// Used to get the key pressed from the user and return the desired action
		/// </summary>
		/// <param name="selectionType">Give the type of menu selection being used</param>
		/// <returns>A type of input based on the key pressed</returns>
		public static InputType Selection(SelectionType selectionType)
		{
			var input = Console.ReadKey(true);

			switch (selectionType)
			{
				case SelectionType.MainMenuSelection:
					return ItemSelect(input.Key);
				case SelectionType.SubMenuSelection:
					return ActionSelect(input.Key);
				case SelectionType.VerificationSelection:
					return VerificationSelect(input.Key);
				default:
					return InputType.Invalid;
			}
		}

		private static InputType ItemSelect(ConsoleKey input)
		{
			switch (input)
			{
				case ConsoleKey.Enter:
					return InputType.Select;
				case ConsoleKey.UpArrow:
					return InputType.PreviousItem;
				case ConsoleKey.DownArrow:
					return InputType.NextItem;
				case ConsoleKey.RightArrow:
					return InputType.NextMenu;
				case ConsoleKey.LeftArrow:
					return InputType.PreviousMenu;
				case ConsoleKey.Escape:
					return InputType.Quit;
				default:
					return InputType.Invalid;
			}
		}

		private static InputType ActionSelect(ConsoleKey input)
		{
			switch (input)
			{
				case ConsoleKey.Enter:
					return InputType.Select;
				case ConsoleKey.UpArrow:
					return InputType.PreviousItem;
				case ConsoleKey.DownArrow:
					return InputType.NextItem;
				case ConsoleKey.Escape:
					return InputType.Back;
				default:
					return InputType.Invalid;
			}
		}

		private static InputType VerificationSelect(ConsoleKey input)
		{
			switch (input)
			{
				case ConsoleKey.Enter:
					return InputType.Select;
				case ConsoleKey.UpArrow:
					return InputType.PreviousItem;
				case ConsoleKey.DownArrow:
					return InputType.NextItem;
				default:
					return InputType.Invalid;
			}
		}
	}
}
