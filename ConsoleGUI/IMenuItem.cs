namespace ConsoleGUI
{
    public interface IMenuItem
    {
        string Title { get; }
        void Interact();
    }
}
