using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestManager.Lib.Services
{
    public interface ITestDialogService
    {
        Task<bool?> ShowDialogFor(ViewModelBase vm, string title = "");
        Task<bool?> ShowDialogMessage(string message, string title = "");
    }
}
