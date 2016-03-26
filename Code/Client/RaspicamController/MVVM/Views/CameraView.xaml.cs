using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RaspicamController.CameraServiceReference;
using RaspicamController.MVVM.ViewModels;

namespace RaspicamController.MVVM.Views
{
    public partial class CameraView
    {
        public static readonly DependencyProperty BindingIDProperty = DependencyProperty.Register("BindingID", typeof(string), typeof(CameraView), new PropertyMetadata("Camera1", OnBindingIDChanged));

        private static void OnBindingIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CameraView) d).BindingID = e.NewValue as string;
        }

        private CameraServiceClient client;
        
        public string BindingID
        {
            get { return (string)GetValue(BindingIDProperty); }
            set { SetValue(BindingIDProperty, value); }
        }
        
        public CameraView()
        {
            InitializeComponent();
            Loaded += OnFinishedLoading;
        }

        private void OnFinishedLoading(object sender, RoutedEventArgs e)
        {
            client = new CameraServiceClient(BindingID);
            client.Open();
        }
        
        private async void OnSetupCamera(object sender, RoutedEventArgs e)
        {
            var parameters = SettingsView.CameraParameters;

            await client.SetupCameraAsync(parameters);
        }

        private async void OnCaptureCamera(object sender, RoutedEventArgs e)
        {
            var data = await client.CaptureImageAsync();
            DisplayView.AddSnapshot(data);
        }
    }
}
