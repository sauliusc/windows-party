using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Tesonet.WindowsParty.Interfaces;
using Tesonet.WindowsParty.Services;
using Tesonet.WindowsParty.ViewModels;
using Tesonet.WindowsParty.Input;
using Xceed.Wpf.Toolkit;

namespace Tesonet.WindowsParty
{
    public class Bootstrapper : BootstrapperBase
    {
        #region fields
        private SimpleContainer _container;
        #endregion

        #region Constructors
        public Bootstrapper()
        {
            Initialize();
        }
        #endregion

        #region Overrides
        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>()
                .Singleton<IConfigurationService, ConfigurationService>()
                .Singleton<IAuthentificationService, AuthentificationService>()
                .Singleton<IDataService, DataService>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IInvoker, Invoker>();

            _container.PerRequest<ShellViewModel>()
                      .PerRequest<LoginViewModel>()
                      .PerRequest<ServerListViewModel>();

            ConventionManager.AddElementConvention<WatermarkPasswordBox>(
           PasswordBoxHelper.BoundPasswordProperty,
           "Password",
           "PasswordChanged");

            //LogManager.GetLog = type => new DebugLogService(type);
            LogManager.GetLog = type => new Log4netService(type);
            InitKeyGesture();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
            Application.MainWindow.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IoC.Get<IAuthentificationService>().Logout();
        }

        protected override object GetInstance(Type service, string key)
        {
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

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            LogManager.GetLog(typeof(ShellViewModel)).Error(e.Exception);
            Xceed.Wpf.Toolkit.MessageBox.Show(Application.MainWindow, e.Exception.Message, "Ooops, something went wrong...", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region Private methods
        private void InitKeyGesture()
        {
            var defaultCreateTrigger = Parser.CreateTrigger;
            Parser.CreateTrigger = (target, triggerText) =>
            {
                if (triggerText == null)
                {
                    return defaultCreateTrigger(target, null);
                }

                var triggerDetail = triggerText
                    .Replace("[", string.Empty)
                    .Replace("]", string.Empty);

                var splits = triggerDetail.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                switch (splits[0])
                {
                    case "Key":
                        var key = (Key)Enum.Parse(typeof(Key), splits[1], true);
                        return new KeyTrigger { Key = key };

                    case "Gesture":
                        var mkg = (MultiKeyGesture)(new MultiKeyGestureConverter()).ConvertFrom(splits[1]);
                        return new KeyTrigger { Modifiers = mkg.KeySequences[0].Modifiers, Key = mkg.KeySequences[0].Keys[0] };
                }

                return defaultCreateTrigger(target, triggerText);
            };
        }
        #endregion
    }
}
