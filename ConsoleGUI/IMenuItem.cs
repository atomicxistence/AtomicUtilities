namespace ConsoleGUI
{
    public interface IMenuItem
    {
        string Title { get; }
        bool IsActive { get; set; }
    }
}
