using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Windows.ApplicationModel.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Tinyflix
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public bool CompactAvailable { get; private set; }

        private bool _isCompact = false;

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            CompactAvailable = ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var mode = _isCompact ? ApplicationViewMode.Default : ApplicationViewMode.CompactOverlay;
            var viewOptions = _isCompact
                ? ViewModePreferences.CreateDefault(ApplicationViewMode.Default)
                : ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);

            if(!_isCompact)
            {
                viewOptions.CustomSize = new Windows.Foundation.Size(640, 360); // Quarter 720p
            }

            var modeSwitched = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(mode, viewOptions);
            if(modeSwitched)
            {
                _isCompact = !_isCompact;
            }
        }
    }
}
