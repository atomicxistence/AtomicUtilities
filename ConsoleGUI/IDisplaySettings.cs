namespace ConsoleGUI
{
	public interface IDisplaySettings
	{
		int PromptOffset { get; }
		int MinMenuWidth { get; }
		int MinMenuHeight { get; }
		int MainMenuLeftOffset { get; }
		int SubMenuTopOffset { get; }
		int SubMenuLeftOffset { get; }
		int SubMenuPromptOffset { get; }

		string[] Title { get; }
		string PromptMessage { get; }
		string SelectionIndicator { get; }
	}
}
