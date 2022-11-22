using Caliburn.Micro;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TRMDesktopUI.Helpers;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.ViewModels;
using TRMDesktopUI.Models;

namespace TRMDesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();
        public Bootstrapper() 
        {
            Initialize();
            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
        }
        
        private IMapper ConfigureAutomapper() 
        {
            var config = new MapperConfiguration(cfg =>
            {
                // map productmodel over to productdisplaymodel
                cfg.CreateMap<ProductModel, ProductDisplayModel>();
                cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
            });
            var output = config.CreateMapper();

            return output;
        }
        protected override void Configure()
        {
            
            _container.Instance(ConfigureAutomapper());
            // return _container
            _container.Instance(_container)
                .PerRequest<IProductEndPoint, ProductEndPoint>()
                .PerRequest<IUserEndPoint, UserEndPoint>()
                .PerRequest<ISaleEndPoint, SaleEndPoint>();
            // singleton is create on instance of the class for the life of the application
            // singleton is great on memory usage and not great on the overall using
            // WindowManager is bringing window out
            // Pass event throught
            // singleton close with static class
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IConfigHelper, ConfigHelper>()
                .Singleton<IAPIHelper, APIHelper>();
            // Register my caculation interface as what will be asked for
            _container
                .PerRequest<ICaculations, Caculations>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<ShellViewModel>();
        }
        protected override object GetInstance(Type service, string key)
        {
            // Pass type and key to get that instance
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
