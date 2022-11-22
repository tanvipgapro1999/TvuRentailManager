using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Helpers;
using TRMDesktopUI;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.EventModels;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName = "tanvipgapro1999@gmail.com";
        private string _password = "Tv08041999@@@";
        private IAPIHelper _apiHelper;
        private IEventAggregator _events;
       
        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events) 
        {
            _apiHelper = apiHelper;
            _events = events;
        }
        public string userName
        {
            get { return _userName; }
            set 
            { 
                _userName = value;
                // Notify when we change value
                NotifyOfPropertyChange(() => userName);
                NotifyOfPropertyChange(() => CanLogIn);
            }

        }

        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public bool IsErrorVisible
        {
            get 
            {
                bool output = false;

                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }
                return output; 
            }
            
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }


        public bool CanLogIn
        {
            get
            {
                bool output = false;
                if (userName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }
                return output;
            }
        }
        
        public async void LogIn() 
        {
            try
            {
                ErrorMessage = "";
                var result = await _apiHelper.Authenticate(userName, Password);
                // Capture more information about the user
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);
                await _events.PublishOnUIThreadAsync(new LogOnEventModel());
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }        
        }

    }
}
