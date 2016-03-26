using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RaspicamController.MVVM.ViewModels;

namespace RaspicamController.MVVM.Views
{
    public partial class CameraDisplayView : UserControl
    {
        public CameraDisplayView()
        {
            InitializeComponent();
        }

        public void AddSnapshot(Bitmap bmp)
        {
            ((RaspicamDisplayViewModel)DataContext).AddSnapshot(bmp);
        }
        public void AddSnapshot(byte[] data)
        {
            ((RaspicamDisplayViewModel)DataContext).AddSnapshot(data);
        }
        private void OnSaveAllSnapshots(object sender, RoutedEventArgs e)
        {
            ((RaspicamDisplayViewModel) DataContext).SaveAllSnapshots();
        }
    }
}
