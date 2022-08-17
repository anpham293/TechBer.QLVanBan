using Xamarin.Forms.Internals;

namespace TechBer.ChuyenDoiSo.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}