using Google.Protobuf;
using ReactiveUI;
using System.Diagnostics;
using System.Reactive.Linq;
using Tonvo.Core;

namespace Tonvo.ViewModels
{
    public class ApplicantControlPanelViewModel : ViewModelBase
    {
        private readonly IMessageBus _messageBus;
        [Reactive] public new Vacancy SelectedVacancy { get; set; } = new();
        public ApplicantControlPanelViewModel(IMessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Listen<Messages>()
                       .DistinctUntilChanged()
                       .Where(message => message != null)
                       .Subscribe(message =>
                       {
                           SelectedVacancy = message.SelectedVacancy;
                       });
        }
    }
}
