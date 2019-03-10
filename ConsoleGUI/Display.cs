using System;

namespace ConsoleGUI
{
	public class Display
	{
		#region DisplayVariables
		private int currentWindowWidth;
		private int currentWindowHeight;
		private int centeredWindowTopEdge;
		private int centeredWindowLeftEdge;
		private int promptOffset = 3;
		private int minMenuWidth = 80;
		private int minMenuHeight = 26;
		private int pageTopOffset;
		private int pageLeftOffset = 2;
		private int subMenuVerticalOffset = 1;
		private int subMenuLeftOffset = 3;
		private int subMenuPromptOffset = 2;

		private string mainMenuPrompt = "▲ ▼ Tasks | ◄ ► Pages | N = New Task  | Esc = Quit";
		private string subMenuPrompt;

		private Selection previousMainMenuSelection;
		private Selection nextMainMenuSelection;
		private IMenu currentMainMenu;
		private Selection currentSubMenuSelection;
		private Selection nextSubMenuSelection;

		private bool needSubMenuRefresh;
		private bool menuHasChanged;

		#region Colors
		private ConsoleColor colorSubMenuBG = ConsoleColor.DarkGray;
		private ConsoleColor colorSubMenuFG = ConsoleColor.White;
		private ConsoleColor colorTitleBG = ConsoleColor.DarkGray;
		private ConsoleColor colorTitleFG = ConsoleColor.Cyan;
		private ConsoleColor colorPromptFG = ConsoleColor.Cyan;
		private ConsoleColor colorPromptBG = ConsoleColor.DarkGray;
		private ConsoleColor colorTaskActioned = ConsoleColor.DarkGray;
		private ConsoleColor colorTaskSelectedBG = ConsoleColor.White;
		private ConsoleColor colorTaskSelectedFG = ConsoleColor.DarkYellow;
		private ConsoleColor colorTextEntryBG = ConsoleColor.White;
		private ConsoleColor colorTextEntryFG = ConsoleColor.Black;
		private ConsoleColor colorDefaultFG = ConsoleColor.Black;
		private ConsoleColor colorDefaultBG = ConsoleColor.Gray;
		#endregion

		private string selectionIndicator = " ► ";
		private string[] title = new string[]
			{"                                         ",
			 "████████╗ █████╗ ███████╗██╗  ██╗██████╗ ", 
			 "╚══██╔══╝██╔══██╗██╔════╝██║ ██╔╝██╔══██╗",
			 "   ██║   ███████║███████╗█████╔╝ ██████╔╝",
			 "   ██║   ██╔══██║╚════██║██╔═██╗ ██╔══██╗",
			 "   ██║   ██║  ██║███████║██║  ██╗██║  ██║",
			 "   ╚═╝   ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝",
			 "                                         "};
		#endregion

		public void Initialize(IMenu currentMainMenu)
		{
			this.currentMainMenu = currentMainMenu;
			minMenuHeight += title.Length + promptOffset;
			SetInitialWindowSize();
			pageTopOffset = centeredWindowTopEdge + title.Length + promptOffset;
			Console.CursorVisible = false;
			previousMainMenuSelection = Selection.firstItem;
			nextMainMenuSelection = Selection.firstItem;
			currentSubMenuSelection = Selection.firstItem;
			nextSubMenuSelection = Selection.firstItem;
			CompleteRefresh();
		}

		public void Refresh(IMenu currentMainMenu, Selection nextMainMenuSelection, bool forceRefresh)
		{
			menuHasChanged = this.currentMainMenu != currentMainMenu;
			this.currentMainMenu = currentMainMenu;
			this.nextMainMenuSelection = nextMainMenuSelection;

			Console.CursorVisible = false;

			if (WindowSizeHasChanged())
			{
				CompleteRefresh();
			}

			if (previousMainMenuSelection.MenuIndex != nextMainMenuSelection.MenuIndex)
			{
				PrintMainMenuItems();
				PrintSelections();
				previousMainMenuSelection = new Selection(nextMainMenuSelection.ItemIndex, nextMainMenuSelection.MenuIndex);
			}

			if (previousMainMenuSelection.ItemIndex != nextMainMenuSelection.ItemIndex)
			{
				PrintSelections();
				previousMainMenuSelection = new Selection(nextMainMenuSelection.ItemIndex, nextMainMenuSelection.MenuIndex);
			}

			if (forceRefresh)
			{
				PrintMainMenuItems();
				PrintSelections();
			}
		}

		public void SubMenuCompleteRefresh(IMenu subMenu, Selection nextSubMenuSelection, string subMenuPrompt)
		{
			this.nextSubMenuSelection = nextSubMenuSelection;
			currentSubMenuSelection = nextSubMenuSelection;

			this.subMenuPrompt = subMenuPrompt;

			var subMenuHorizontalSize = minMenuWidth / 2;
			var subMenuLeftStart = (Console.WindowWidth / 2)  - (subMenuHorizontalSize / 2);
			var subMenuTopStart = (Console.WindowHeight / 2) - (subMenu.MenuItems.Count / 2) + (subMenuVerticalOffset * 2);

			PrintSubMenuField(subMenu, subMenuLeftStart, subMenuTopStart);
			PrintSubMenuOptions(subMenu, subMenuLeftStart, subMenuTopStart);
			PrintSubMenuSelections(subMenu);
		}

		public void SubMenuRefresh(IMenu subMenu, Selection nextSubSelection)
		{
			this.nextSubMenuSelection = nextSubSelection;

			if (needSubMenuRefresh)
			{
				SubMenuCompleteRefresh(subMenu, nextSubSelection, subMenuPrompt);
			}

			if (currentSubMenuSelection.ItemIndex != nextSubSelection.ItemIndex)
			{
				PrintSubMenuSelections(subMenu);
				currentSubMenuSelection = new Selection(nextSubSelection.ItemIndex, 0);
			}
		}

		public void UserInputPrompt(string promptMessage)
		{
			var userInputPromptWidth = minMenuWidth / 2 + minMenuWidth / 4;
			var userInputPromptHeight = 3;
			var userInputPromptLeftEdge = (Console.WindowWidth / 2) - (userInputPromptWidth / 2);
			var userInputPromptTopEdge = (Console.WindowHeight / 2) - (userInputPromptHeight / 2);

			//Print user prompt background
			Console.BackgroundColor = colorSubMenuBG;
			Console.ForegroundColor = colorSubMenuFG;
			for (int i = 0; i < userInputPromptHeight; i++)
			{
				Console.SetCursorPosition(userInputPromptLeftEdge, userInputPromptTopEdge + i);
				PrintBackgroundFill(userInputPromptWidth);
			}
			//Print user input prompt message
			Console.SetCursorPosition(userInputPromptLeftEdge + subMenuLeftOffset, 
									  userInputPromptTopEdge + (userInputPromptHeight / 2));
			Console.Write(promptMessage);

			// Print user input field background
			Console.ForegroundColor = colorTextEntryFG;
			Console.BackgroundColor = colorTextEntryBG;
			PrintBackgroundFill(userInputPromptWidth - promptMessage.Length - (subMenuLeftOffset * 2));
			Console.SetCursorPosition(userInputPromptLeftEdge + subMenuLeftOffset + promptMessage.Length + 1,
									  userInputPromptTopEdge + (userInputPromptHeight / 2));
		}

		//-----------------------------------------------------------------------------------------

		private void PrintSubMenuField(IMenu subMenu, int subMenuLeftStart, int subMenuTopStart)
		{
			Console.ForegroundColor = colorSubMenuFG;
			Console.BackgroundColor = colorSubMenuBG;

			for (int i = 0; i < subMenu.MenuItems.Count + (subMenuVerticalOffset * 2) + subMenuPromptOffset; i++)
			{
				Console.SetCursorPosition(subMenuLeftStart, subMenuTopStart + i);
				PrintBackgroundFill(minMenuWidth / 2);
			}
		}

		private void PrintSubMenuOptions(IMenu subMenu, int subMenuLeftStart, int subMenuTopStart)
		{
			Console.ForegroundColor = colorSubMenuFG;
			Console.BackgroundColor = colorSubMenuBG;

			Console.SetCursorPosition(subMenuLeftStart + subMenuLeftOffset,
										  subMenuTopStart + subMenuVerticalOffset);
			Console.Write(subMenuPrompt);

			for (int i = 0; i < subMenu.MenuItems.Count; i++)
			{
				Console.SetCursorPosition(subMenuLeftStart + subMenuLeftOffset,
										  subMenuTopStart + subMenuVerticalOffset + subMenuPromptOffset + i);
				Console.Write(subMenu.MenuItems[i].Title);
			}
		}

		private void PrintSubMenuSelections(IMenu subMenu)
		{
			var subMenuHorizontalSize = minMenuWidth / 2;
			var subMenuLeftStart = (Console.WindowWidth / 2) - (subMenuHorizontalSize / 2);
			var subMenuTopStart = (Console.WindowHeight / 2) - (subMenu.MenuItems.Count / 2) + (subMenuVerticalOffset * 2);

			PrintSubMenuPreviousSelection(subMenu, subMenuLeftStart, subMenuTopStart);
			PrintSubMenuNextSelection(subMenu, subMenuLeftStart, subMenuTopStart);
		}

		private void PrintSubMenuPreviousSelection(IMenu subMenu, int subMenuLeftStart, int subMenuTopStart)
		{
			Console.ForegroundColor = colorSubMenuFG;
			Console.BackgroundColor = colorSubMenuBG;

			Console.SetCursorPosition(subMenuLeftStart + subMenuLeftOffset,
												  subMenuTopStart + subMenuVerticalOffset + currentSubMenuSelection.ItemIndex + subMenuPromptOffset);
			PrintBackgroundFill((minMenuWidth / 2) - (subMenuLeftOffset * 2) + 1);

			Console.SetCursorPosition(subMenuLeftStart + subMenuLeftOffset,
									  subMenuTopStart + subMenuVerticalOffset + currentSubMenuSelection.ItemIndex + subMenuPromptOffset);
			Console.Write(subMenu.MenuItems[currentSubMenuSelection.ItemIndex].Title);
		}

		private void PrintSubMenuNextSelection(IMenu subMenu, int subMenuLeftStart, int subMenuTopStart)
		{
			Console.ForegroundColor = colorTaskSelectedFG;
			Console.BackgroundColor = colorTaskSelectedBG;

			Console.SetCursorPosition(subMenuLeftStart + subMenuLeftOffset,
									  subMenuTopStart + subMenuVerticalOffset + nextSubMenuSelection.ItemIndex + subMenuPromptOffset);
			PrintBackgroundFill((minMenuWidth / 2) - (subMenuLeftOffset * 2));

			Console.SetCursorPosition(subMenuLeftStart + subMenuLeftOffset,
									  subMenuTopStart + subMenuVerticalOffset + nextSubMenuSelection.ItemIndex + subMenuPromptOffset);
			Console.Write(selectionIndicator);
			Console.Write(subMenu.MenuItems[nextSubMenuSelection.ItemIndex].Title);
			PrintBackgroundFill((minMenuWidth / 2) - (subMenuLeftOffset * 2) -
								(subMenu.MenuItems[nextSubMenuSelection.ItemIndex].Title.Length +
								 selectionIndicator.Length));
		}

		private void SetInitialWindowSize()
		{
			Console.SetWindowSize(minMenuWidth, minMenuHeight);
			currentWindowWidth = Console.WindowWidth;
			currentWindowHeight = Console.WindowHeight;

			SetCenteredWindowEdges();
		}

		private void SetCenteredWindowEdges()
		{
			centeredWindowLeftEdge = (currentWindowWidth / 2) - (minMenuWidth / 2);
			centeredWindowTopEdge = (currentWindowHeight / 2) - (minMenuHeight / 2);
		}

		private bool WindowSizeHasChanged()
		{
			bool hasChanged = false;

			while (Console.WindowWidth < minMenuWidth || Console.WindowHeight < minMenuHeight)
			{
				hasChanged = true;
				Console.Clear();
				Console.WriteLine($"Please adjust your window size to at least {minMenuWidth} by {minMenuHeight}. Press any key to continue.");
				Console.ReadKey();
			}

			if (Console.WindowWidth != currentWindowWidth || Console.WindowHeight != currentWindowHeight)
			{
				hasChanged = true;
				currentWindowWidth = Console.WindowWidth;
				currentWindowHeight = Console.WindowHeight;
			}

			needSubMenuRefresh = hasChanged;

			return hasChanged;
		}

		private void CompleteRefresh()
		{
			Console.ResetColor();
			Console.Clear();
			Console.CursorVisible = false;
			SetCenteredWindowEdges();
			PrintTitle();
			PrintMainMenuPrompt();
			PrintMainMenuItems();
			PrintSelections();
		}

		private void PrintSelections()
		{
			IMenuItem previousItem;
			if (menuHasChanged)
			{
				previousItem = currentMainMenu.MenuItems[0];
			}
			else
			{
				previousItem = currentMainMenu.MenuItems[previousMainMenuSelection.ItemIndex];
				PrintPreviousSelection(previousItem);
			}

			var nextItem = currentMainMenu.MenuItems[nextMainMenuSelection.ItemIndex];
			PrintNextSelection(nextItem);
		}

		private void PrintPreviousSelection(IMenuItem currentItem)
		{
			Console.ForegroundColor = colorDefaultFG;
			Console.BackgroundColor = colorDefaultBG;

			Console.SetCursorPosition(centeredWindowLeftEdge,
									  centeredWindowTopEdge + pageTopOffset + previousMainMenuSelection.ItemIndex);
			PrintBackgroundFill(minMenuWidth);
			
			Console.SetCursorPosition(centeredWindowLeftEdge + pageLeftOffset,
									  centeredWindowTopEdge + pageTopOffset + previousMainMenuSelection.ItemIndex);
			Console.Write(currentItem.Title);
		}

		private void PrintNextSelection(IMenuItem nextMenuItem)
		{
			Console.ForegroundColor = colorTaskSelectedFG;
			Console.BackgroundColor = colorTaskSelectedBG;
			Console.SetCursorPosition(centeredWindowLeftEdge,
									  centeredWindowTopEdge + pageTopOffset + nextMainMenuSelection.ItemIndex);
			Console.Write(selectionIndicator);
			Console.Write(nextMenuItem.Title);
			PrintBackgroundFill(minMenuWidth - nextMenuItem.Title.Length - selectionIndicator.Length);
		}

		private void PrintTitle()
		{
			Console.SetCursorPosition(centeredWindowLeftEdge, centeredWindowTopEdge);
			Console.ForegroundColor = colorTitleFG;
			Console.BackgroundColor = colorTitleBG;

			// Write Background Color
			for (int i = 0; i <= title.Length; i++)
			{
				PrintBackgroundFill(minMenuWidth);
				Console.SetCursorPosition(centeredWindowLeftEdge, centeredWindowTopEdge + i);
			}
			// Write Centered Title
			for (int i = 0; i < title.Length; i++)
			{
				Console.SetCursorPosition(centeredWindowLeftEdge + ((minMenuWidth / 2) - (title[i].Length / 2)), centeredWindowTopEdge + i);
				Console.Write(title[i]);
			}
		}

		private void PrintMainMenuPrompt()
		{
			Console.ForegroundColor = colorPromptFG;
			Console.BackgroundColor = colorPromptBG;

			// Print main menu prompt background
			for (int i = 0; i < promptOffset - 1; i++)
			{
				Console.SetCursorPosition(centeredWindowLeftEdge, centeredWindowTopEdge + title.Length + i);
				PrintBackgroundFill(minMenuWidth);
			}

			// Print main menu prompt text
			Console.SetCursorPosition(centeredWindowLeftEdge + ((minMenuWidth / 2) - (mainMenuPrompt.Length / 2)), 
									  centeredWindowTopEdge + title.Length);
			Console.Write(mainMenuPrompt);

			// Print top margin before main menu contents
			Console.ForegroundColor = colorDefaultFG;
			Console.BackgroundColor = colorDefaultBG;
			Console.SetCursorPosition(centeredWindowLeftEdge, centeredWindowTopEdge + title.Length + promptOffset - 1);
			PrintBackgroundFill(minMenuWidth);
		}

		private void PrintMainMenuItems()
		{
			Console.ForegroundColor = colorDefaultFG;
			Console.BackgroundColor = colorDefaultBG;
			// Print main menu background
			for (int i = 0; i < minMenuHeight; i++)
			{
				Console.SetCursorPosition(centeredWindowLeftEdge, centeredWindowTopEdge + pageTopOffset + i);
				PrintBackgroundFill(minMenuWidth);
			}
			// Print main menu contents
			for (int i = 0; i < currentMainMenu.MenuItems.Count; i++)
			{
				Console.ForegroundColor = colorDefaultFG;
				Console.SetCursorPosition(centeredWindowLeftEdge + pageLeftOffset, 
										  centeredWindowTopEdge + pageTopOffset + i);
				Console.ForegroundColor = colorDefaultFG;
				Console.Write(currentMainMenu.MenuItems[i].Title);
			}
		}

		private void PrintBackgroundFill(int fillSize)
		{
			for (int i = 0; i < fillSize; i++)
			{
				Console.Write(" ");
			}
		}
	}
}
