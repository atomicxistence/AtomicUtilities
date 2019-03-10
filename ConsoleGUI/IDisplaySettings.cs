namespace Atomic.ConsoleGUI
{
	/// <summary>
	/// Defines the settings needed to initialize a main menu
	/// </summary>
	public interface IDisplaySettings
	{
		/// <summary>
		/// The offset value of the prompt message from the title
		/// </summary>
		int PromptOffset { get; }
		/// <summary>
		/// The width of the menu and defines the minimum width of the console window
		/// </summary>
		int MinMenuWidth { get; }
		/// <summary>
		/// The height of the menu and defines the minimum height of the console window
		/// </summary>
		int MinMenuHeight { get; }
		/// <summary>
		/// The left margin of menu items
		/// </summary>
		int MainMenuLeftOffset { get; }
		/// <summary>
		/// The top margin of sub menu items
		/// </summary>
		int SubMenuTopOffset { get; }
		/// <summary>
		/// The left margin of sub menu items
		/// </summary>
		int SubMenuLeftOffset { get; }
		/// <summary>
		/// The top margin of the sub menu prompt
		/// </summary>
		int SubMenuPromptOffset { get; }

		/// <summary>
		/// The title of the main menu
		/// </summary>
		string[] Title { get; }
		/// <summary>
		/// The prompt message displayed below the main menu title
		/// </summary>
		string PromptMessage { get; }
		/// <summary>
		/// The indicator that is displayed to the left of the current selection
		/// </summary>
		string SelectionIndicator { get; }
	}
}
