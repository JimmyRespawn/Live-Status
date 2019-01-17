using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Live_Status.Views;
using Live_Status.Helpers;

namespace Live_Status
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            try
            {
                if (DeviceTypeHelper.GetDeviceFormFactorType().ToString() == "Xbox")
                {
                    this.RequiresPointerMode = ApplicationRequiresPointerMode.WhenRequested;
                    //Test if xbox support reveal effect
                    if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.FocusVisualKind"))
                    {
                        try
                        {
                            this.FocusVisualKind = FocusVisualKind.Reveal;
                        }
                        catch
                        {
                            //Earlier version on Xbox than RS4 which does not support reveal effect
                        }
                    }
                }
            }
            catch
            {

            }
            this.Suspending += OnSuspending;
        }
        
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            
            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {

                }
                
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(HomePage), e.Arguments);
                }
                Window.Current.Activate();
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            deferral.Complete();
        }
    }
}
