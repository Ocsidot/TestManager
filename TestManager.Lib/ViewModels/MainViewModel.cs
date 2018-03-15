using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TestManager.Lib.Services;

namespace TestManager.Lib.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields 
        private IDataRetriever _dataRetriever;
        private ObservableCollection<PostmanCollection> _collections;
        private PostmanCollection _selectedCollection;

        #endregion

        public ObservableCollection<PostmanCollection> Collections
        {
            get { return _collections; }
            set
            {
                if(_collections != value)
                {
                    _collections = value;
                    RaisePropertyChanged("Collections");
                }
            }
        }

        public PostmanCollection SelectedCollection
        {
            get { return _selectedCollection; }
            set
            {
                if (_selectedCollection != value)
                {
                    _selectedCollection = value;
                    RaisePropertyChanged("SelectedCollection");
                }
            }
        }

        #region Commands

        private RelayCommand _loadCommand;
        public RelayCommand LoadCommand
        {
            get
            {
                return _loadCommand ??
                    (_loadCommand = new RelayCommand(async() =>
                    {
                        await LoadCollections();
                    }));
            }
        }

        #endregion

        public MainViewModel()
        {
            _dataRetriever = new DataRetrieverService();
            _collections = new ObservableCollection<PostmanCollection>();
            _selectedCollection = null;
        }

        private async Task LoadCollections()
        {
            this.Collections.Clear();

            var result = await _dataRetriever.GetCollections("d:\\Userfiles\\atodisco\\Desktop\\Postman\\Collection\\test");
            foreach(var collection in result)
            {
                collection.RefreshInformation();
                this.Collections.Add(collection);
            }
        }
    }
}
