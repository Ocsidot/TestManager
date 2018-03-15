using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Lib.ViewModels
{
    public partial class RequestItem
    {
        private int _numberOfTests;

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

        internal override void RefreshInformation()
        {
            var tests = this.Events.Where(e => e.ListenType == ListenType.Test).ToList();
            tests.ForEach(test => test.RefreshInformation());

            this.NumberOfTests = tests.Sum(test => test.NumberOfTests);
        }
    }
}
