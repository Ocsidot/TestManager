using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestManager.Lib.Services
{
    public interface IViewService
    {
        void RegisterViewFor<T>(Type viewType, string key = "") where T : ViewModelBase;
        FrameworkElement GetViewFor(ViewModelBase vm);
        FrameworkElement GetViewbyKey(string key, ViewModelBase vm = null);
    }
}
