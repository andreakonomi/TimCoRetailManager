using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    // this says I can handle this
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        // this says Im registered on this web of events
        private readonly IEventAggregator _events;
        private SalesViewModel _salesVM;
        private ILoggedInUserModel _user;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel user)
        {
            _events = events;
            _salesVM = salesVM;
            _user = user;

            _events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(_user.Token);

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        // subscriber to the LogOnEvent event
        public void Handle(LogOnEvent message)
        {
            // you can only have one item open at a time when iheriting from conductor, conducts from one to another
            // so the Login Page deactivates it automatically
            ActivateItem(_salesVM);
            //notify the logout button to reset
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
