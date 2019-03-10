namespace ConsoleGUI
{
	/// <summary>
	/// Defines an item that is held within a menu list
	/// </summary>
	public interface IMenuItem
    {
		/// <summary>
		/// The title of the menu item
		/// </summary>
		string Title { get; }
		/// <summary>
		/// The status of the menu item
		/// </summary>
        bool IsActive { get; set; }
    }
}
