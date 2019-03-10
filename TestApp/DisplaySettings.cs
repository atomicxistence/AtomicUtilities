using ConsoleGUI;

namespace TestApp
{
	class DisplaySettings : IDisplaySettings
	{
		public int PromptOffset => 3;
		public int MinMenuWidth => 80;
		public int MinMenuHeight => 26;
		public int MainMenuLeftOffset => 2;
		public int SubMenuTopOffset => 1;
		public int SubMenuLeftOffset => 3;
		public int SubMenuPromptOffset => 2;

		public string PromptMessage => "▲ ▼ Tasks | ◄ ► Pages | N = New Task  | Esc = Quit";
		public string SelectionIndicator =>  " ► ";
		public string[] Title => new string[]
			{"                                         ",
			 "████████╗ █████╗ ███████╗██╗  ██╗██████╗ ",
			 "╚══██╔══╝██╔══██╗██╔════╝██║ ██╔╝██╔══██╗",
			 "   ██║   ███████║███████╗█████╔╝ ██████╔╝",
			 "   ██║   ██╔══██║╚════██║██╔═██╗ ██╔══██╗",
			 "   ██║   ██║  ██║███████║██║  ██╗██║  ██║",
			 "   ╚═╝   ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝",
			 "                                         "};
	}
}
