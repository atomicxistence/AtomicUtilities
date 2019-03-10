using System;

namespace ConsoleGUI
{
	public class Display
	{
		#region DisplayVariables

		private IDisplaySettings ds;
		private IColorTheme ct;

		private int currentWindowWidth;
		private int currentWindowHeight;
		private int centeredWindowTopEdge;
		private int centeredWindowLeftEdge;
		private int mainMenuTopEdge;
		private int minHeight;

		private string subMenuPrompt;

		private bool needSubMenuRefresh;
		private bool mainMenuHasChanged;

		private IMenu currentMainMenu;
		private Selection previousMainMenuSelection;
		private Selection nextMainMenuSelection;
		private Selection currentSubMenuSelection;
		private Selection nextSubMenuSelection;

		#endregion

		public Display(IDisplaySettings displaySettings)
		{
			ds = displaySettings;
			ct = new DefaultColorTheme();
		}

		public Display(IDisplaySettings displaySettings, IColorTheme colorTheme)
		{
			ds = displaySettings;
			ct = colorTheme;
		}

		public void Initialize(IMenu currentMainMenu)
		{
			this.currentMainMenu = currentMainMenu;

			minHeight = ds.MinMenuHeight + ds.PromptOffset + ds.Title.Length;
			SetInitialWindowSize();
			mainMenuTopEdge = centeredWindowTopEdge + ds.Title.Length + ds.PromptOffset;

			Console.CursorVisible = false;

			previousMainMenuSelection = Selection.firstItem;
			nextMainMenuSelection = Selection.firstItem;
			currentSubMenuSelection = Selection.firstItem;
			nextSubMenuSelection = Selection.firstItem;

			CompleteRefresh();
		}

		public void Refresh(IMenu currentMainMenu, Selection nextMainMenuSelection, bool forceRefresh)
		{
			mainMenuHasChanged = this.currentMainMenu != currentMainMenu;
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
				PrintMainMenuSelections();
				previousMainMenuSelection = new Selection(nextMainMenuSelection.ItemIndex, nextMainMenuSelection.MenuIndex);
			}

			if (previousMainMenuSelection.ItemIndex != nextMainMenuSelection.ItemIndex)
			{
				PrintMainMenuSelections();
				previousMainMenuSelection = new Selection(nextMainMenuSelection.ItemIndex, nextMainMenuSelection.MenuIndex);
			}

			if (forceRefresh)
			{
				PrintMainMenuItems();
				PrintMainMenuSelections();
			}
		}

		public void SubMenuCompleteRefresh(IMenu subMenu, Selection nextSubMenuSelection, string subMenuPrompt)
		{
			this.nextSubMenuSelection = nextSubMenuSelection;
			currentSubMenuSelection = nextSubMenuSelection;

			this.subMenuPrompt = subMenuPrompt;

			var subMenuHorizontalSize = ds.MinMenuHeight / 2;
			var subMenuLeftStart = (Console.WindowWidth / 2)  - (subMenuHorizontalSize / 2);
			var subMenuTopStart = (Console.WindowHeight / 2) - (subMenu.MenuItems.Count / 2) + (ds.SubMenuTopOffset * 2);

			PrintSubMenuField(subMenu, subMenuLeftStart, subMenuTopStart);
			PrintSubMenuOptions(subMenu, subMenuLeftStart, subMenuTopStart);
			PrintSubMenuSelections(subMenu);
		}

		public void SubMenuRefresh(IMenu subMenu, Selection nextSubMenuSelection)
		{
			this.nextSubMenuSelection = nextSubMenuSelection;

			if (needSubMenuRefresh)
			{
				SubMenuCompleteRefresh(subMenu, nextSubMenuSelection, subMenuPrompt);
			}

			if (currentSubMenuSelection.ItemIndex != nextSubMenuSelection.ItemIndex)
			{
				PrintSubMenuSelections(subMenu);
				currentSubMenuSelection = new Selection(nextSubMenuSelection.ItemIndex, 0);
			}
		}

		public void UserInputPrompt(string promptMessage)
		{
			var userInputPromptWidth = ds.MinMenuWidth / 2 + ds.MinMenuWidth / 4;
			var userInputPromptHeight = 3;
			var userInputPromptLeftEdge = (Console.WindowWidth / 2) - (userInputPromptWidth / 2);
			var userInputPromptTopEdge = (Console.WindowHeight / 2) - (userInputPromptHeight / 2);

			//Print user prompt background
			Console.BackgroundColor = ct.ColorSubMenuBG;
			Console.ForegroundColor = ct.ColorSubMenuFG;
			for (int i = 0; i < userInputPromptHeight; i++)
			{
				Console.SetCursorPosition(userInputPromptLeftEdge, userInputPromptTopEdge + i);
				PrintBackgroundFill(userInputPromptWidth);
			}
			//Print user input prompt message
			Console.SetCursorPosition(userInputPromptLeftEdge + ds.SubMenuLeftOffset, 
									  userInputPromptTopEdge + (userInputPromptHeight / 2));
			Console.Write(promptMessage);

			// Print user input field background
			Console.ForegroundColor = ct.ColorTextEntryFG;
			Console.BackgroundColor = ct.ColorTextEntryBG;
			PrintBackgroundFill(userInputPromptWidth - promptMessage.Length - (ds.SubMenuLeftOffset * 2));
			Console.SetCursorPosition(userInputPromptLeftEdge + ds.SubMenuLeftOffset + promptMessage.Length + 1,
									  userInputPromptTopEdge + (userInputPromptHeight / 2));
		}

		public void MessageBox(string message)
		{
			var messageBoxWidth = ds.MinMenuWidth / 2 + ds.MinMenuWidth / 4;
			var messageBoxHeight = 3;
			var messageBoxLeftEdge = (Console.WindowWidth / 2) - (messageBoxWidth / 2);
			var messageBoxTopEdge = (Console.WindowHeight / 2) - (messageBoxHeight / 2);

			//Print message box background
			Console.BackgroundColor = ct.ColorSubMenuBG;
			Console.ForegroundColor = ct.ColorSubMenuFG;
			for (int i = 0; i < messageBoxHeight; i++)
			{
				Console.SetCursorPosition(messageBoxLeftEdge, messageBoxTopEdge + i);
				PrintBackgroundFill(messageBoxWidth);
			}
			//Print message box message
			Console.SetCursorPosition(messageBoxLeftEdge + ds.SubMenuLeftOffset,
									  messageBoxTopEdge + (messageBoxHeight / 2));
			Console.Write(message);
		}

		//-----------------------------------------------------------------------------------------

		private void PrintSubMenuField(IMenu subMenu, int subMenuLeftStart, int subMenuTopStart)
		{
			Console.ForegroundColor = ct.ColorSubMenuFG;
			Console.BackgroundColor = ct.ColorSubMenuBG;

			for (int i = 0; i < subMenu.MenuItems.Count + (ds.SubMenuTopOffset * 2) + ds.SubMenuPromptOffset; i++)
			{
				Console.SetCursorPosition(subMenuLeftStart, subMenuTopStart + i);
				PrintBackgroundFill(ds.MinMenuWidth / 2);
			}
		}

		private void PrintSubMenuOptions(IMenu subMenu, int subMenuLeftStart, int subMenuTopStart)
		{
			Console.ForegroundColor = ct.ColorSubMenuFG;
			Console.BackgroundColor = ct.ColorSubMenuBG;

			Console.SetCursorPosition(subMenuLeftStart + ds.SubMenuLeftOffset,
										  subMenuTopStart + ds.SubMenuTopOffset);
			Console.Write(subMenuPrompt);

			for (int i = 0; i < subMenu.MenuItems.Count; i++)
			{
				Console.SetCursorPosition(subMenuLeftStart + ds.SubMenuLeftOffset,
										  subMenuTopStart + ds.SubMenuTopOffset + ds.SubMenuPromptOffset + i);
				Console.Write(subMenu.MenuItems[i].Title);
			}
		}

		private void PrintSubMenuSelections(IMenu subMenu)
		{
			var subMenuHorizontalSize = ds.MinMenuWidth / 2;
			var subMenuLeftStart = (Console.WindowWidth / 2) - (subMenuHorizontalSize / 2);
			var subMenuTopStart = (Console.WindowHeight / 2) - (subMenu.MenuItems.Count / 2) + (ds.SubMenuTopOffset * 2);

			PrintSubMenuPreviousSelection(subMenu, subMenuLeftStart, subMenuTopStart);
			PrintSubMenuNextSelection(subMenu, subMenuLeftStart, subMenuTopStart);
		}

		private void PrintSubMenuPreviousSelection(IMenu subMenu, int subMenuLeftStart, int subMenuTopStart)
		{
			Console.ForegroundColor = ct.ColorSubMenuFG;
			Console.BackgroundColor = ct.ColorSubMenuBG;

			Console.SetCursorPosition(subMenuLeftStart + ds.SubMenuLeftOffset,
												  subMenuTopStart + ds.SubMenuTopOffset + currentSubMenuSelection.ItemIndex + ds.SubMenuPromptOffset);
			PrintBackgroundFill((ds.MinMenuWidth / 2) - (ds.SubMenuLeftOffset * 2) + 1);

			Console.SetCursorPosition(subMenuLeftStart + ds.SubMenuLeftOffset,
									  subMenuTopStart + ds.SubMenuTopOffset + currentSubMenuSelection.ItemIndex + ds.SubMenuPromptOffset);
			Console.Write(subMenu.MenuItems[currentSubMenuSelection.ItemIndex].Title);
		}

		private void PrintSubMenuNextSelection(IMenu subMenu, int subMenuLeftStart, int subMenuTopStart)
		{
			Console.ForegroundColor = ct.ColorItemSelectedFG;
			Console.BackgroundColor = ct.ColorItemSelectedBG;

			Console.SetCursorPosition(subMenuLeftStart + ds.SubMenuLeftOffset,
									  subMenuTopStart + ds.SubMenuTopOffset + nextSubMenuSelection.ItemIndex + ds.SubMenuPromptOffset);
			PrintBackgroundFill((ds.MinMenuWidth / 2) - (ds.SubMenuLeftOffset * 2));

			Console.SetCursorPosition(subMenuLeftStart + ds.SubMenuLeftOffset,
									  subMenuTopStart + ds.SubMenuTopOffset + nextSubMenuSelection.ItemIndex + ds.SubMenuPromptOffset);
			Console.Write(ds.SelectionIndicator);
			Console.Write(subMenu.MenuItems[nextSubMenuSelection.ItemIndex].Title);
			PrintBackgroundFill((ds.MinMenuWidth / 2) - (ds.SubMenuLeftOffset * 2) -
								(subMenu.MenuItems[nextSubMenuSelection.ItemIndex].Title.Length +
								 ds.SelectionIndicator.Length));
		}

		private void SetInitialWindowSize()
		{
			Console.SetWindowSize(ds.MinMenuWidth, minHeight);
			currentWindowWidth = Console.WindowWidth;
			currentWindowHeight = Console.WindowHeight;

			SetCenteredWindowEdges();
		}

		private void SetCenteredWindowEdges()
		{
			centeredWindowLeftEdge = (currentWindowWidth / 2) - (ds.MinMenuWidth / 2);
			centeredWindowTopEdge = (currentWindowHeight / 2) - (minHeight / 2);
		}

		private bool WindowSizeHasChanged()
		{
			bool hasChanged = false;

			while (Console.WindowWidth < ds.MinMenuWidth || Console.WindowHeight < minHeight)
			{
				hasChanged = true;
				Console.Clear();
				Console.WriteLine($"Please adjust your window size to at least {ds.MinMenuWidth} by {minHeight}. Press any key to continue.");
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
			PrintMainMenuSelections();
		}

		private void PrintMainMenuSelections()
		{
			IMenuItem previousItem;
			if (mainMenuHasChanged)
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

		private void PrintPreviousSelection(IMenuItem currentMenuItem)
		{
			Console.ForegroundColor = currentMenuItem.IsActive ? ct.ColorDefaultFG : ct.ColorItemInactiveFG;
			Console.BackgroundColor = ct.ColorDefaultBG;

			Console.SetCursorPosition(centeredWindowLeftEdge,
									  centeredWindowTopEdge + mainMenuTopEdge + previousMainMenuSelection.ItemIndex);
			PrintBackgroundFill(ds.MinMenuWidth);
			
			Console.SetCursorPosition(centeredWindowLeftEdge + ds.MainMenuLeftOffset,
									  centeredWindowTopEdge + mainMenuTopEdge + previousMainMenuSelection.ItemIndex);
			Console.Write(currentMenuItem.Title);
		}

		private void PrintNextSelection(IMenuItem nextMenuItem)
		{
			Console.ForegroundColor = nextMenuItem.IsActive ? ct.ColorItemSelectedFG : ct.ColorItemInactiveFG;
			Console.BackgroundColor = ct.ColorItemSelectedBG;
			Console.SetCursorPosition(centeredWindowLeftEdge,
									  centeredWindowTopEdge + mainMenuTopEdge + nextMainMenuSelection.ItemIndex);
			Console.Write(ds.SelectionIndicator);
			Console.Write(nextMenuItem.Title);
			PrintBackgroundFill(ds.MinMenuWidth - nextMenuItem.Title.Length - ds.SelectionIndicator.Length);
		}

		private void PrintTitle()
		{
			Console.SetCursorPosition(centeredWindowLeftEdge, centeredWindowTopEdge);
			Console.ForegroundColor = ct.ColorTitleFG;
			Console.BackgroundColor = ct.ColorTitleBG;

			// Write Background Color
			for (int i = 0; i <= ds.Title.Length; i++)
			{
				PrintBackgroundFill(ds.MinMenuWidth);
				Console.SetCursorPosition(centeredWindowLeftEdge, centeredWindowTopEdge + i);
			}
			// Write Centered Title
			for (int i = 0; i < ds.Title.Length; i++)
			{
				Console.SetCursorPosition(centeredWindowLeftEdge + ((ds.MinMenuWidth / 2) - (ds.Title[i].Length / 2)), centeredWindowTopEdge + i);
				Console.Write(ds.Title[i]);
			}
		}

		private void PrintMainMenuPrompt()
		{
			Console.ForegroundColor = ct.ColorPromptFG;
			Console.BackgroundColor = ct.ColorPromptBG;

			// Print main menu prompt background
			for (int i = 0; i < ds.PromptOffset - 1; i++)
			{
				Console.SetCursorPosition(centeredWindowLeftEdge, centeredWindowTopEdge + ds.Title.Length + i);
				PrintBackgroundFill(ds.MinMenuWidth);
			}

			// Print main menu prompt text
			Console.SetCursorPosition(centeredWindowLeftEdge + ((ds.MinMenuWidth / 2) - (ds.PromptMessage.Length / 2)), 
									  centeredWindowTopEdge + ds.Title.Length);
			Console.Write(ds.PromptMessage);

			// Print top margin before main menu contents
			Console.ForegroundColor = ct.ColorDefaultFG;
			Console.BackgroundColor = ct.ColorDefaultBG;
			Console.SetCursorPosition(centeredWindowLeftEdge, centeredWindowTopEdge + ds.Title.Length + ds.PromptOffset - 1);
			PrintBackgroundFill(ds.MinMenuWidth);
		}

		private void PrintMainMenuItems()
		{
			Console.ForegroundColor = ct.ColorDefaultFG;
			Console.BackgroundColor = ct.ColorDefaultBG;
			// Print main menu background
			for (int i = 0; i < ds.MinMenuHeight; i++)
			{
				Console.SetCursorPosition(centeredWindowLeftEdge, centeredWindowTopEdge + mainMenuTopEdge + i);
				PrintBackgroundFill(ds.MinMenuWidth);
			}
			// Print main menu contents
			for (int i = 0; i < currentMainMenu.MenuItems.Count; i++)
			{
				Console.ForegroundColor = currentMainMenu.MenuItems[i].IsActive ? ct.ColorDefaultFG : ct.ColorItemInactiveFG;
				Console.SetCursorPosition(centeredWindowLeftEdge + ds.MainMenuLeftOffset, 
										  centeredWindowTopEdge + mainMenuTopEdge + i);
				
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
