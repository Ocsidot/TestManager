using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Lib.ViewModels
{
    public partial class PostmanCollection
    {
        private int _numberOfRequests;
        private int _numberOfTests;

        public int NumberOfRequests
        {
            get { return _numberOfRequests; }
            set
            {
                if (value == _numberOfRequests)
                {
                    return;
                }
                _numberOfRequests = value;
                RaisePropertyChanged("NumberOfRequests");
            }
        }

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

        internal void RefreshInformation()
        {
            var firstLevelRequests = this.Items.OfType<RequestItem>();
            var subRequests = this.GetSubRequests();

            this.NumberOfRequests = firstLevelRequests.Count() + subRequests.Count();
            this.NumberOfTests = firstLevelRequests.Sum(request => request.NumberOfTests) + subRequests.Sum(request => request.NumberOfTests);
        }

        private IEnumerable<RequestItem> GetSubRequests()
        {
            var folders = this.Items.OfType<FolderItem>();

            return folders.SelectMany(folder => folder.GetRequests());
        }
    }
}
