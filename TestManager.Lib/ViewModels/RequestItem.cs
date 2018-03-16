using CommonServiceLocator;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestManager.Lib.Services;

namespace TestManager.Lib.ViewModels
{
    public partial class RequestItem
    {
        private int _numberOfTests;

        #region Properties
        public int NumberOfTests
        {
            get { return _numberOfTests; }
            set
            {
                if (value == _numberOfTests)
                {
                    return;
                }
                _numberOfTests = value;
                RaisePropertyChanged("NumberOfTests");
            }
        }

        private ITestDialogService DialogService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ITestDialogService>();
            }
        }

        public Event PreRequest
        {
            get { return this.Events.FirstOrDefault(e => e.ListenType == ListenType.Prerequest); }
        }

        public Event Test
        {
            get { return this.Events.FirstOrDefault(e => e.ListenType == ListenType.Test); }
        }

        #endregion

        #region Commands

        private RelayCommand _showInfoCommand;
        public RelayCommand ShowInfoCommand
        {
            get
            {
                return _showInfoCommand ??
                    (_showInfoCommand = new RelayCommand(async () =>
                    {
                        await DialogService.ShowDialogFor(this);
                    }));
            }
        }

        #endregion

        internal override void RefreshInformation()
        {
            var tests = this.Events.Where(e => e.ListenType == ListenType.Test).ToList();
            tests.ForEach(test => test.RefreshInformation());

            this.NumberOfTests = tests.Sum(test => test.NumberOfTests);
        }
    }
}
