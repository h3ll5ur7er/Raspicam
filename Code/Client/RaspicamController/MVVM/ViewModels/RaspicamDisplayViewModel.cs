using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using MVVMBase.VM;
using RaspicamController.Properties;

namespace RaspicamController.MVVM.ViewModels
{
    public class RaspicamDisplayViewModel : ViewModel
    {
        public ObservableCollection<Bitmap> Snapshots { get; set; }
        public IEnumerable<BitmapImage> SnapshotSources => Snapshots.Select(GetBindableSource);
        public string BindingID => (Parent as RaspiCamViewModel)?.BindingID;
        public RaspicamDisplayViewModel() : this(null)
        {
        }
        public RaspicamDisplayViewModel(ViewModel parent) : base(parent)
        {
            Snapshots = new ObservableCollection<Bitmap>();
        }

        public void AddSnapshot(byte[] data)
        {
            AddSnapshot((Bitmap) Image.FromStream(new MemoryStream(data)));
        }
        public void AddSnapshot(Bitmap bmp)
        {
            Snapshots.Add(bmp);
            OnPropertyChanged(nameof(Snapshots));
            OnPropertyChanged(nameof(SnapshotSources));
        }


        public BitmapImage GetBindableSource(Bitmap bmp)
        {
            var bi = new BitmapImage();
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Bmp);
                ms.Position = 0;

                bi.BeginInit();
                bi.StreamSource = ms;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
            }
            return bi;
        }

        protected override void SetupEvents()
        {
        }

        protected override void SetupHandlers()
        {
        }

        public void SaveAllSnapshots()
        {
            foreach (var snapshot in Snapshots)
            {
                var file = new FileInfo(Path.Combine(Settings.Default.BasePath, BindingID, Guid.NewGuid()+".bmp"));

                if (!file.Directory.Exists)
                {
                    if (!file.Directory.Parent.Exists)
                    {
                        file.Directory.Parent.Create();
                    }
                    file.Directory.Create();
                }
                
                snapshot.Save(file.FullName, ImageFormat.Bmp);
            }
        }
        public override string[] PropertyNames { get; } =
            {
                nameof(Snapshots),
                nameof(SnapshotSources)
            };

    }
}