using System.Collections.Generic;
using MVVMBase.VM;
using RaspicamServer;

namespace RaspicamController.MVVM.ViewModels
{
    public class RaspicamSettingsViewModel : ViewModel
    {
        #region Fields
        private int _width = 2592;
        private int _height = 1944;
        private int _jpegQuality = 100;
        private bool _addRawBayerData = false;
        private PictureEncoding _encoding = PictureEncoding.bmp;
        private SensorMode _mode = SensorMode.Auto;
        private int _sharpness = 0;
        private int _contrast = 0;
        private int _brightness = 50;
        private int _saturation = 0;
        private int _iso = 450;
        private int _ev = 0;
        private Exposure _exposure = Exposure.auto;
        private AWB _awb = AWB.auto;
        private Effect _effect = Effect.none;
        //private byte _colorFxU = 128;
        //private byte _colorFxV = 128;
        private Metering _metering = Metering.average;
        private int _rotation = 0;
        private bool _hFlip = false;
        private bool _vFlip = false;
        private float _roiX = 0;
        private float _roiY = 0;
        private float _roiW = 1;
        private float _roiH = 1;
        private float _awbgB = 1;
        private float _awbgR = 1;
        private DRC _drc = DRC.off;
        private bool _stats = false;
        #endregion Fields

        public int width_min { get; } = 1;
        public int width_max { get; } = 2592;
        public int width_step { get; } = 1;
        public int width
        {
            get { return _width; }
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public int height_min { get; } = 1;
        public int height_max { get; } = 1944;
        public int height_step { get; } = 1;
        public int height
        {
            get { return _height; }
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public int jpegQuality_min { get; } = 0;
        public int jpegQuality_max { get; } = 100;
        public int jpegQuality_step { get; } = 1;
        public int jpegQuality
        {
            get { return _jpegQuality; }
            set
            {
                if (value == _jpegQuality) return;
                _jpegQuality = value;
                OnPropertyChanged();
            }
        }

        public bool addRawBayerData
        {
            get { return _addRawBayerData; }
            set
            {
                if (value == _addRawBayerData) return;
                _addRawBayerData = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<PictureEncoding> encoding_all => typeof (PictureEncoding).GetAllMembers<PictureEncoding>();
        public PictureEncoding encoding
        {
            get { return _encoding; }
            set
            {
                if (value == _encoding) return;
                _encoding = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<SensorMode> mode_all => typeof(SensorMode).GetAllMembers<SensorMode>();
        public SensorMode mode
        {
            get { return _mode; }
            set
            {
                if (value == _mode) return;
                _mode = value;
                OnPropertyChanged();
            }
        }

        public int sharpness_min { get; } = -100;
        public int sharpness_max { get; } = 100;
        public int sharpness_step { get; } = 1;
        public int sharpness
        {
            get { return _sharpness; }
            set
            {
                if (value == _sharpness) return;
                _sharpness = value;
                OnPropertyChanged();
            }
        }

        public int contrast_min { get; } = -100;
        public int contrast_max { get; } = 100;
        public int contrast_step { get; } = 1;
        public int contrast
        {
            get { return _contrast; }
            set
            {
                if (value == _contrast) return;
                _contrast = value;
                OnPropertyChanged();
            }
        }

        public int brightness_min { get; } = 0;
        public int brightness_max { get; } = 100;
        public int brightness_step { get; } = 1;
        public int brightness
        {
            get { return _brightness; }
            set
            {
                if (value == _brightness) return;
                _brightness = value;
                OnPropertyChanged();
            }
        }

        public int saturation_min { get; } = -100;
        public int saturation_max { get; } = 100;
        public int saturation_step { get; } = 1;
        public int saturation
        {
            get { return _saturation; }
            set
            {
                if (value == _saturation) return;
                _saturation = value;
                OnPropertyChanged();
            }
        }

        public int ISO_min { get; } = 100;
        public int ISO_max { get; } = 800;
        public int ISO_step { get; } = 1;
        public int ISO
        {
            get { return _iso; }
            set
            {
                if (value == _iso) return;
                _iso = value;
                OnPropertyChanged();
            }
        }

        public int ev_min { get; } = -10;
        public int ev_max { get; } = 10;
        public int ev_step { get; } = 1;
        public int ev
        {
            get { return _ev; }
            set
            {
                if (value == _ev) return;
                _ev = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Exposure> exposure_all => typeof(Exposure).GetAllMembers<Exposure>();
        public Exposure exposure
        {
            get { return _exposure; }
            set
            {
                if (value == _exposure) return;
                _exposure = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<AWB> awb_all => typeof(AWB).GetAllMembers<AWB>();
        public AWB awb
        {
            get { return _awb; }
            set
            {
                if (value == _awb) return;
                _awb = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Effect> effect_all => typeof(Effect).GetAllMembers<Effect>();
        public Effect effect
        {
            get { return _effect; }
            set
            {
                if (value == _effect) return;
                _effect = value;
                OnPropertyChanged();
            }
        }

        //public byte colorFxU_min { get; } = 0;
        //public byte colorFxU_max { get; } = 255;
        //public byte colorFxU
        //{
        //    get { return _colorFxU; }
        //    set
        //    {
        //        if (value == _colorFxU) return;
        //        _colorFxU = value;
        //        OnPropertyChanged();
        //    }
        //}

        //public byte colorFxV_min { get; } = 0;
        //public byte colorFxV_max { get; } = 255;
        //public byte colorFxV
        //{
        //    get { return _colorFxV; }
        //    set
        //    {
        //        if (value == _colorFxV) return;
        //        _colorFxV = value;
        //        OnPropertyChanged();
        //    }
        //}

        public IEnumerable<Metering> metering_all => typeof(Metering).GetAllMembers<Metering>();
        public Metering metering
        {
            get { return _metering; }
            set
            {
                if (value == _metering) return;
                _metering = value;
                OnPropertyChanged();
            }
        }

        public int rotation_min { get; } = 0;
        public int rotation_max { get; } = 359;
        public int rotation_step { get; } = 1;
        public int rotation
        {
            get { return _rotation; }
            set
            {
                if (value == _rotation) return;
                _rotation = value;
                OnPropertyChanged();
            }
        }

        public bool hFlip
        {
            get { return _hFlip; }
            set
            {
                if (value == _hFlip) return;
                _hFlip = value;
                OnPropertyChanged();
            }
        }

        public bool vFlip
        {
            get { return _vFlip; }
            set
            {
                if (value == _vFlip) return;
                _vFlip = value;
                OnPropertyChanged();
            }
        }

        public float roi_min { get; } = 0;
        public float roi_max { get; } = 1;
        public float roi_step { get; } = 0.01f;
        public float roiX
        {
            get { return _roiX; }
            set
            {
                if (value.Equals(_roiX)) return;
                _roiX = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(roiW_max));
                if (roiW > roiW_max) roiW = roiW_max;
            }
        }
        public float roiY
        {
            get { return _roiY; }
            set
            {
                if (value.Equals(_roiY)) return;
                _roiY = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(roiH_max));
                if (roiH > roiH_max) roiH = roiH_max;
            }
        }
        public float roiW_max => roi_max - roiX;
        public float roiW
        {
            get { return _roiW; }
            set
            {
                if (value.Equals(_roiW)) return;
                _roiW = value;
                OnPropertyChanged();
            }
        }
        public float roiH_max => roi_max - roiY;
        public float roiH
        {
            get { return _roiH; }
            set
            {
                if (value.Equals(_roiH)) return;
                _roiH = value;
                OnPropertyChanged();
            }
        }

        public float awbg_min { get; } = 1;
        public float awbg_max { get; } = 2;
        public float awbg_step { get; } = 0.01f;
        public float awbgB
        {
            get { return _awbgB; }
            set
            {
                if (value.Equals(_awbgB)) return;
                _awbgB = value;
                OnPropertyChanged();
            }
        }

        public float awbgR
        {
            get { return _awbgR; }
            set
            {
                if (value.Equals(_awbgR)) return;
                _awbgR = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<DRC> drc_all => typeof(DRC).GetAllMembers<DRC>();
        public DRC drc
        {
            get { return _drc; }
            set
            {
                if (value == _drc) return;
                _drc = value;
                OnPropertyChanged();
            }
        }

        public bool stats
        {
            get { return _stats; }
            set
            {
                if (value == _stats) return;
                _stats = value;
                OnPropertyChanged();
            }
        }

        public RaspicamParameters Parameters => new RaspicamParameters(width, height, jpegQuality, addRawBayerData, encoding, mode, sharpness, contrast, brightness, saturation, ISO, ev, exposure, awb, effect, metering, rotation, hFlip, vFlip, roiX, roiY, roiW, roiH, awbgB, awbgR, drc, stats);

        public RaspicamSettingsViewModel() : base(null)
        {
        }

        public RaspicamSettingsViewModel(ViewModel parent) : base(parent)
        {
        }


        protected override void SetupEvents()
        {
        }

        protected override void SetupHandlers()
        {
        }

        public override string[] PropertyNames { get; } =
            {
                nameof(width),
                nameof(height),
                nameof(jpegQuality),
                nameof(addRawBayerData),
                nameof(encoding),
                nameof(mode),
                nameof(sharpness),
                nameof(contrast),
                nameof(brightness),
                nameof(saturation),
                nameof(ISO),
                nameof(ev),
                nameof(exposure),
                nameof(awb),
                nameof(effect),
                //nameof(colorFxU),
                //nameof(colorFxV),
                nameof(metering),
                nameof(rotation),
                nameof(hFlip),
                nameof(vFlip),
                nameof(roiX),
                nameof(roiY),
                nameof(roiW),
                nameof(roiH),
                nameof(awbgB),
                nameof(awbgR),
                nameof(drc),
                nameof(stats),
            };
    }
}