using MVVMBase.VM;

namespace RaspicamController.MVVM.ViewModels
{
    public class RaspiCamViewModel : ViewModel
    {
        private RaspicamSettingsViewModel cameraSettings;
        private RaspicamDisplayViewModel cameraDisplays;
        private string bindingId;

        public string BindingID
        {
            get { return bindingId; }
            set
            {
                if (value == bindingId) return;
                bindingId = value;
                OnPropertyChanged();
            }
        }

        public RaspicamSettingsViewModel Settings
        {
            get { return cameraSettings; }
            set
            {
                if (Equals(value, cameraSettings)) return;
                cameraSettings = value;
                OnPropertyChanged();
            }
        }

        public RaspicamDisplayViewModel Display
        {
            get { return cameraDisplays; }
            set
            {
                if (Equals(value, cameraDisplays)) return;
                cameraDisplays = value;
                OnPropertyChanged();
            }
        }

        public RaspiCamViewModel() : this(null, "Camera1")
        {
        }
        public RaspiCamViewModel(ViewModel parent, string binding) : base(parent)
        {
            cameraSettings = new RaspicamSettingsViewModel(this);
            cameraDisplays = new RaspicamDisplayViewModel(this);
            bindingId = binding;
        }
        

        protected override void SetupEvents()
        {
        }

        protected override void SetupHandlers()
        {
        }
        public override string[] PropertyNames { get; } =
        {
            nameof(Settings),
            nameof(Display),
            nameof(BindingID)
        };
    }
}