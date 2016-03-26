using System.Collections;
using System.Collections.ObjectModel;
using MVVMBase.VM;

namespace RaspicamController.MVVM.ViewModels
{
    public class MainViewModel: ViewModel
    {
        //private ObservableCollection<RaspiCamViewModel> cameras;
        public RaspiCamViewModel Cam1 { get; set; }
        public RaspiCamViewModel Cam2 { get; set; }
        public RaspiCamViewModel Cam3 { get; set; }
        public RaspiCamViewModel Cam4 { get; set; }

        //public ObservableCollection<RaspiCamViewModel> Cameras
        //{
        //    get { return cameras; }
        //    set
        //    {
        //        if (Equals(value, cameras)) return;
        //        cameras = value;
        //        OnPropertyChanged();
        //    }
        //}

        public MainViewModel() : base(null)
        {
            //cameras = new ObservableCollection<RaspiCamViewModel>();
            //AddCamera();
            //AddCamera();
            //AddCamera();
            //AddCamera();


            Cam1 = new RaspiCamViewModel(this, "Camera1");
            Cam2 = new RaspiCamViewModel(this, "Camera1");
            Cam3 = new RaspiCamViewModel(this, "Camera1");
            Cam4 = new RaspiCamViewModel(this, "Camera1");

            //Cam1 = new RaspiCamViewModel(this, "Camera1");
            //Cam2 = new RaspiCamViewModel(this, "Camera2");
            //Cam3 = new RaspiCamViewModel(this, "Camera3");
            //Cam4 = new RaspiCamViewModel(this, "Camera4");
        }

        //public void AddCamera()
        //{
        //    cameras.Add(new RaspiCamViewModel(this));
        //}

        public override string[] PropertyNames { get; } =
        {
            //nameof(Cameras),
            nameof(Cam1),
            nameof(Cam2),
            nameof(Cam3),
            nameof(Cam4),
        };

        protected override void SetupEvents()
        {
        }

        protected override void SetupHandlers()
        {
        }
    }
}