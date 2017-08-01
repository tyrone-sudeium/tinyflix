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
using System.Threading;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Tinyflix
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public bool CompactAvailable { get; private set; }
        private bool ForceCompactHidden { get; set; }

        private bool _isCompact = false;
        private bool _supportsCompact = false;
        private bool _forceCompactHidden = false;
        private Timer timer = null;

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

            _supportsCompact = ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay);
            updateVisbilityBool();
        }

        private void updateVisbilityBool()
        {
            if (!_supportsCompact)
            {
                this.CompactAvailable = false;
            }
            else if (_isCompact)
            {
                this.CompactAvailable = false;
            }
            else if (_forceCompactHidden)
            {
                this.CompactAvailable = false;
            }
            else
            {
                this.CompactAvailable = true;
            }
            System.Diagnostics.Debug.WriteLine("CompactAvailable = " + this.CompactAvailable);
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

        private void Page_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _forceCompactHidden = true;
            updateVisbilityBool();
            startControlHideTimer();
        }

        private void Page_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            _forceCompactHidden = true;
            updateVisbilityBool();
            startControlHideTimer();
        }

        private void Page_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            startControlHideTimer();
        }

        private void startControlHideTimer()
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
            }
            this.timer = new Timer((obj) =>
            {
                _forceCompactHidden = false;
                updateVisbilityBool();
            }, null, 3000, Timeout.Infinite);
        }

    }
}
