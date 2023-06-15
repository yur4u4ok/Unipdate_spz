namespace ProgramTools.ViewModel
{
    public interface INavigableView<out T>
    {
        T ViewModel { get; }
    }
}