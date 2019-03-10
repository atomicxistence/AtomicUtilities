namespace ConsoleGUI
{
	public struct Selection
	{
		/// <summary>
		/// Representation of the first item within the first menu list
		/// </summary>
		public static Selection firstItem = new Selection(0, 0);

		public int ItemIndex { get; private set; }
		public int MenuIndex { get; private set; }

		/// <summary>
		/// Representation of the indices of an item within a list and a menu within a list
		/// </summary>
		/// <param name="itemIndex">The index value of the item in a list</param>
		/// <param name="menuIndex">The index value of a menu in a list</param>
		public Selection(int itemIndex, int menuIndex)
		{
			ItemIndex = itemIndex;
			MenuIndex = menuIndex;
		}
	}
}
