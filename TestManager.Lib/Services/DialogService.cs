using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CommonServiceLocator;
using GalaSoft.MvvmLight;

namespace TestManager.Lib.Services
{
    internal class DialogService : ITestDialogService
    {
        private IViewService _viewService;

        public DialogService()
        {
            _viewService = ServiceLocator.Current.GetInstance<IViewService>();
        }

        public async Task<bool?> ShowDialogFor(ViewModelBase vm, string title = "")
        {
            var view = _viewService.GetViewFor(vm);

            return await GetCurrentDispatcher().InvokeAsync(() => {
                var window = new Window
                {
                    Title = title,
                    Content = view,
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                return window.ShowDialog();
            });
        }

        public async Task<bool?> ShowDialogMessage(string message, string title = "")
        {
            return await GetCurrentDispatcher().InvokeAsync(() => {
                var result = MessageBox.Show(message, title, MessageBoxButton.OK);
                return result == MessageBoxResult.OK;
            });
        }

        private static Dispatcher GetCurrentDispatcher()
        {
            var currentApplication = System.Windows.Application.Current;
            if (currentApplication != null)
            {
                return currentApplication.Dispatcher;
            }

            return Dispatcher.CurrentDispatcher;
        }
    }
}
