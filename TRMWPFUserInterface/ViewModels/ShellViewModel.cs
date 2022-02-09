using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Api;
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
        private IAPIHelper _apiHelper;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel user, IAPIHelper apiHelper)
        {
            _events = events;
            _salesVM = salesVM;
            _user = user;
            _apiHelper = apiHelper;

            _events.SubscribeOnPublishedThread(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
        }

        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(_user.Token);

        public void ExitApplication()
        {
            TryCloseAsync();
        }

        public async Task UserManagment()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken()); 
        }

        public async Task LogOut()
        {
            _user.ResetUserModel();     // Clear method inside the model itself, special case when you put a method in a model.
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        // subscriber to the LogOnEvent event
        //public void Handle(LogOnEvent message)
        //{
        //    // you can only have one item open at a time when iheriting from conductor, conducts from one to another
        //    // so the Login Page deactivates it automatically
        //    ActivateItem(_salesVM);
        //    //notify the logout button to reset
        //    NotifyOfPropertyChange(() => IsLoggedIn);
        //}

        // subscriber to the LogOnEvent
        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            // you can only have one item open at a time when iheriting from conductor, conducts from one to another
            // so the Login Page deactivates it automatically
            await ActivateItemAsync(_salesVM, cancellationToken);
            //notify the logout button to reset
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
