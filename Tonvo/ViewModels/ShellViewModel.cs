using ReactiveUI.Fody.Helpers;
using System.Diagnostics;

namespace Tonvo.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        [Reactive]
        public string Title { get; set; }

        public ShellViewModel()
        {
            Title = "Hello World!!!";
            Debug.WriteLine("QQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQ");
        }
    }
}
