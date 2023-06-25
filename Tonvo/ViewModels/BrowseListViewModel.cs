using Google.Protobuf;
using ReactiveUI;
using System.Reactive;

namespace Tonvo.ViewModels
{
    public class BrowseListViewModel : ReactiveObject
    {
        private readonly IMessageBus _messageBus;

        [Reactive] public int SelectedList { get; set; }

        public BrowseListViewModel(IMessageBus messageBus)
        {
            _messageBus = messageBus;

            this.WhenAnyValue(x => x.SelectedList)
                .Subscribe(selectedList =>
                {
                    var message = new Messages { SelectedList = selectedList };
                    _messageBus.SendMessage(message);
                });
        }
    }
}
