using System;
using ConsoleGUI;

namespace TestApp
{
	class TAManager
	{
		bool forceRefresh = false;

		public void Run()
		{
			var displaySettings = new DisplaySettings();
			var display = new Display(displaySettings);
			var menu = new Menu();
			var input = new Input();

			menu.MenuItems.Add(new MenuItem("First Item"));
			menu.MenuItems.Add(new MenuItem("Second Item"));
			menu.MenuItems.Add(new MenuItem("Third Item"));

			Console.OutputEncoding = System.Text.Encoding.Unicode;
			display.Initialize(menu);

			while (true)
			{
				display.Refresh(menu, Selection.firstItem, forceRefresh);
				forceRefresh = false;

				InputType action = InputType.Invalid;
				do
				{
					action = input.Selection(SelectionType.MainMenuSelection);
				} while (action == InputType.Invalid);

				ActionUserTaskListInput(action);
			}
		}

		private void ActionUserTaskListInput(InputType action)
		{
			throw new NotImplementedException();
		}
	}
}
