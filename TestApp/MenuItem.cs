using ConsoleGUI;

namespace TestApp
{
	class MenuItem : IMenuItem
	{
		public string Title { get; private set; }

		public MenuItem(string title)
		{
			Title = title;
		}

		public void Interact()
		{
			
		}
	}
}
