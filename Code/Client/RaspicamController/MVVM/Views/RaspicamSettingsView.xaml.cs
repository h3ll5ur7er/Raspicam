using System;
using System.Collections.Generic;
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
using RaspicamServer;

namespace RaspicamController.MVVM.Views
{
    public partial class RaspicamSettingsView : UserControl
    {
        public RaspicamParameters CameraParameters => ((RaspicamSettingsViewModel) DataContext).Parameters;

        public RaspicamSettingsView()
        {
            InitializeComponent();
        }
    }
}
