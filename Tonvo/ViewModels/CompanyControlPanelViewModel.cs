using ReactiveUI;
using Tonvo.Core;

namespace Tonvo.ViewModels
{
    public class CompanyControlPanelViewModel : ViewModelBase
    {
        private readonly IMessageBus _messageBus;
        [Reactive] public new Applicant SelectedApplicant { get; set; } = new();
        public CompanyControlPanelViewModel(IMessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Listen<Messages>()
                       .DistinctUntilChanged()
                       .Where(message => message != null)
                       .Subscribe(message =>
                       {
                           SelectedApplicant = message.SelectedApplicant;
                       });
        }
    }
}
