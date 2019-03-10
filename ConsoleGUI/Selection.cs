namespace ConsoleGUI
{
	public struct Selection
	{
		public static Selection firstItem = new Selection(0, 0);

		public int ItemIndex { get; private set; }
		public int MenuIndex { get; private set; }

		/// <summary>
		/// Struct to hold an Item index value & a Page index value
		/// </summary>
		public Selection(int itemIndex, int menuIndex)
		{
			ItemIndex = itemIndex;
			MenuIndex = menuIndex;
		}
	}
}
