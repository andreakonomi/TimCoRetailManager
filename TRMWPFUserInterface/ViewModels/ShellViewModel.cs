using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;

namespace TRMDesktopUI.ViewModels
{
    // this says I can handle this
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        // this says Im registered on this web of events
        private readonly IEventAggregator _events;
        private SalesViewModel _salesVM;
        private SimpleContainer _container;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM,
            SimpleContainer container)
        {
            _events = events;
            _salesVM = salesVM;
            _container = container; // to request new instances when we want

            _events.Subscribe(this);

            ActivateItem(_container.GetInstance<LoginViewModel>());
        }

        // subscriber to the LogOnEvent event
        public void Handle(LogOnEvent message)
        {
            // you can only have one item open at a time when iheriting from conductor, conducts from one to another
            // so the Login Page deactivates it automatically
            ActivateItem(_salesVM);
        }
    }
}
