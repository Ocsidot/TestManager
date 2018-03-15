using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestManager.Lib.ViewModels
{
    public partial class Event
    {
        private static readonly List<string> TEST_SELECTOR = new List<string> { "it(", "test[" };
        private int _numberOfTests;
        private ObservableCollection<string> _availableTests;

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

        public ObservableCollection<string> AvailableTests
        {
            get { return _availableTests; }
            set
            {
                if (value == _availableTests)
                {
                    return;
                }
                _availableTests = value;
                RaisePropertyChanged("AvailableTests");
            }
        }

        public string Code
        {
            get { return string.Join("\n", this.Script.ExecLines); }
        }

        public Event()
        {
            _availableTests = new ObservableCollection<string>();
            _numberOfTests = -1;
        }

        internal void RefreshInformation()
        {
            var testLines = this.Script.ExecLines.Where(line => TEST_SELECTOR.Any(selector => line.Contains(selector)));
            this.NumberOfTests = testLines.Count();

            testLines.Select(test => GetTestDescription(test))
                     .Where(desc => !string.IsNullOrWhiteSpace(desc)).ToList()
                     .ForEach(line => this.AvailableTests.Add(line));
        }

        private string GetTestDescription(string testLine)
        {
            Regex r = new Regex("^(?:it\\(|test\\[)(?:\"|')(.*)(?:\"|')(?:,|\\]).*$", RegexOptions.IgnoreCase);
            Match m = r.Match(testLine);
            return m.Success ? m.Groups[1].Value : null;
        }
    }
}
