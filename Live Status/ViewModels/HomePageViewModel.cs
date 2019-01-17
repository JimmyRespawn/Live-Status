using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Net.Http;
using System.Globalization;
using Windows.Foundation.Metadata;
using Windows.ApplicationModel.Background;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml;
using HtmlAgilityPack;
using Windows.Globalization;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Live_Status.Helpers;
using Live_Status.Models;

namespace Live_Status.ViewModels
{
    public class HomePageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public RelayCommand LoadHomePageCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<status> _statusCollection = new ObservableCollection<status>();
        public ObservableCollection<status> StatusCollection
        {
            get { return _statusCollection; }
            set
            {
                _statusCollection = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StatusCollection"));
                }
            }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsLoading"));
            }
        }

        private bool _isError = false;
        public bool IsError
        {
            get
            {
                return _isError;
            }
            set
            {
                _isError = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsError"));
            }
        }

        public async Task GetStatus(string tobeParsedString)
        {
            try
            {
                IsLoading = true;
                IsError = false;

                var ResultList = await StatusParser.Parse(tobeParsedString);
                foreach (var s in ResultList)
                {
                    StatusCollection.Add(s);
                }
                if (StatusCollection.Count == 0)
                    IsError = true;
            }
            catch
            {
                IsError = true;
            }
            IsLoading = false;
        }

        //different here
        public HomePageViewModel()
        {
            LoadHomePageCommand = new RelayCommand(async () => await GetStatus(""));
        }
    }
}
