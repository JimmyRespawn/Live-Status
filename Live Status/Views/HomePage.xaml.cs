using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Live_Status.ViewModels;

namespace Live_Status.Views
{
    public sealed partial class HomePage : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public HomePageViewModel ViewModel { get; set; }

        public HomePage()
        {
            this.InitializeComponent();
            this.ViewModel = new HomePageViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
            }

            if (localSettings.Values.ContainsKey("light"))
                this.RequestedTheme = ElementTheme.Light;
            else
                this.RequestedTheme = ElementTheme.Dark;
        }

        private void Title_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (localSettings.Values.ContainsKey("light"))
            {
                localSettings.Values.Remove("light");
                this.RequestedTheme = ElementTheme.Dark;
            }
            else
            {
                localSettings.Values["light"] = "1";
                this.RequestedTheme = ElementTheme.Light;
            }
        }

        private void ShowWebview_Click(object sender, RoutedEventArgs e)
        {
            if (Webgird.Visibility == Visibility.Visible)
            {
                Webgird.Visibility = Visibility.Collapsed;
            }
            else if (Webgird.Visibility == Visibility.Collapsed)
            {
                Webgird.Visibility = Visibility.Visible;
            }
        }

        private void NoInternet_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GetStatus("");
        }
    }
}
