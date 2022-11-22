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
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEventModel>
    {
        private IEventAggregator _events;
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;
        public ShellViewModel(IEventAggregator events, 
                              ILoggedInUserModel user, 
                              IAPIHelper IAPIHelper)
        {
            _events = events;
            _user = user;
            _apiHelper = IAPIHelper;
            _events.SubscribeOnUIThread(this);
            // Activate login in sell model
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public void ExitApplication() 
        {
            TryCloseAsync();
        }

        public void UserManagement() 
        {
            ActivateItemAsync(IoC.Get<UserDisplayViewModel>());
        }
        public bool IsLoggedIn
        {
            get
            {
                bool output = false;

                if (string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }
                return output;
            }
        }
        public void LogOut() 
        {
            _user.ResetUserModel();
            _apiHelper.LogOffUser();
            NotifyOfPropertyChange(() => IsLoggedIn);
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }
        public Task HandleAsync(LogOnEventModel message, CancellationToken cancellationToken)
        {
            ActivateItemAsync(IoC.Get<SalesViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
            return Task.CompletedTask;
        }
    }
}
