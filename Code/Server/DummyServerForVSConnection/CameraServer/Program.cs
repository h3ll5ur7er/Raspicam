using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace RaspicamServer
{
    public class MainClass
    {

        public static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    ServiceHost host = new ServiceHost(typeof(CameraService));
                    host.Open();
                    Console.ReadLine();
                    host.Close();
                }
                catch (Exception ex)
                {
                    // improove errorhandling
                    Console.WriteLine(ex);
                    Debugger.Break();
                }
            }
        }
    }


    public class RaspicamWrapperMock : RaspicamWrapper
    {
        const string testImagePath = "";
        
        public RaspicamWrapperMock(string parameters) : base()
        {
            Console.WriteLine(parameters);
        }
        public RaspicamWrapperMock(params IParameter[] parameters) : this(string.Join(" ", parameters.Select(x => x.CollectString)) + " -k -n -o -")
        {
        }

        /// <summary>
        ///		
        ///	-?, --help      : This help information
        ///	-w, --width     : Set image width <size>
        ///	-h, --height    : Set image height <size>
        ///	-q, --quality   : Set jpeg quality <0 to 100>
        ///	-r, --raw       : Add raw bayer data to jpeg metadata
        ///	-o, --output    : Output filename <filename> (to write to stdout, use '-o -'). If not specified, no file is saved
        ///	-l, --latest    : Link latest complete image to filename <filename>
        ///	-v, --verbose   : Output verbose information during run
        ///	-t, --timeout   : Time (in ms) before takes picture and shuts down (if not specified, set to 5s)
        ///	-th, --thumb    : Set thumbnail parameters (x:y:quality) or none
        ///	-d, --demo      : Run a demo mode (cycle through range of camera options, no capture)
        ///	-e, --encoding  : Encoding to use for output file (jpg, bmp, gif, png)
        ///	-x, --exif      : EXIF tag to apply to captures (format as 'key=value') or none
        ///	-tl, --timelapse        : Timelapse mode. Takes a picture every <t>ms
        ///	-fp, --fullpreview      : Run the preview using the still capture resolution (may reduce preview fps)
        ///	-k, --keypress  : Wait between captures for a ENTER, X then ENTER to exit
        ///	-s, --signal    : Wait between captures for a SIGUSR1 from another process
        ///	-g, --gl        : Draw preview to texture instead of using video render component
        ///	-gc, --glcapture        : Capture the GL frame-buffer instead of the camera image
        ///	-set, --settings        : Retrieve camera settings and write to stdout
        ///	-cs, --camselect        : Select camera <number>. Default 0
        ///	-bm, --burst    : Enable 'burst capture mode'
        ///	-md, --mode     : Force sensor mode. 0=auto. See docs for other modes available
        ///	-dt, --datetime : Replace frame number in file name with DateTime (MonthDayHourMinSec)
        ///	-ts, --timestamp        : Replace frame number in file name with unix timestamp (seconds since 1900)
        ///	-fs, --framestart       : Starting frame number in output pattern
        ///	----------------------------------------------------------------------------------------
        /// -p, --preview	: Preview window settings <'x,y,w,h'>
        /// -f, --fullscreen	: Fullscreen preview mode
        /// -op, --opacity	: Preview window opacity (0-255)
        /// -n, --nopreview	: Do not display a preview window
        ///	----------------------------------------------------------------------------------------
        ///	-sh, --sharpness        : Set image sharpness (-100 to 100)
        ///	-co, --contrast : Set image contrast (-100 to 100)
        ///	-br, --brightness       : Set image brightness (0 to 100)
        ///	-sa, --saturation       : Set image saturation (-100 to 100)
        ///	-ISO, --ISO     : Set capture ISO
        ///	-vs, --vstab    : Turn on video stabilisation
        ///	-ev, --ev       : Set EV compensation - steps of 1/6 stop
        ///	-ex, --exposure : Set exposure mode (see Notes)
        ///	-awb, --awb     : Set AWB mode (see Notes)
        ///	-ifx, --imxfx   : Set image effect (see Notes)
        ///	-cfx, --colfx   : Set colour effect (U:V)
        ///	-mm, --metering : Set metering mode (see Notes)
        ///	-rot, --rotation        : Set image rotation (0-359)
        ///	-hf, --hflip    : Set horizontal flip
        ///	-vf, --vflip    : Set vertical flip
        ///	-roi, --roi     : Set region of interest (x,y,w,d as normalised coordinates [0.0-1.0])
        ///	-ss, --shutter  : Set shutter speed in microseconds
        ///	-awbg, --awbgains       : Set AWB gains - AWB mode must be off
        ///	-drc, --drc     : Set DRC Level
        ///	-st, --stats    : Force recomputation of statistics on stills capture pass
        ///	-a, --annotate  : Enable/Set annotate flags or text
        ///	-3d, --stereo   : Select stereoscopic mode
        ///	-dec, --decimate        : Half width/height of stereo image
        ///	-3dswap, --3dswap       : Swap camera order for stereoscopic
        ///	-ae, --annotateex       : Set extra annotation parameters (text size, text colour(hex YUV), bg colour(hex YUV))
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="jpegQuality">0 - 100 : default(100)</param>
        /// <param name="addRawBayerData">add raw bayer data to image metadata</param>
        /// <param name="encoding">image format</param>
        /// <param name="mode">Sensor mode</param>
        /// <param name="sharpness">-100 - 100 : default(0)</param>
        /// <param name="contrast">-100 - 100 : default(0)</param>
        /// <param name="brightness">0 - 100 : default(50)</param>
        /// <param name="saturation">-100 - 100 : default(0)</param>
        /// <param name="ISO">100 - 800</param>
        /// <param name="ev">-10 - 10 : default(0)</param>
        /// <param name="exposure">default:auto, not all will allways work</param>
        /// <param name="awb">default:auto</param>
        /// <param name="effect">default: none</param>
        /// <param name="colorFxU"></param>
        /// <param name="colorFxV"></param>
        /// <param name="metering"></param>
        /// <param name="rotation">0 - 359</param>
        /// <param name="hFlip">h flip</param>
        /// <param name="vFlip">v flip</param>
        /// <param name="roiX">ROI x</param>
        /// <param name="roiY">ROI y</param>
        /// <param name="roiW">ROI width</param>
        /// <param name="roiH">ROI height</param>
        /// <param name="awbgB">AWB Gains B</param>
        /// <param name="awbgR">AWB Gains R</param>
        /// <param name="drc">DynamicRangeCompression default(off)</param>
        /// <param name="stats">If set to <c>true</c> stats.</param>
        public RaspicamWrapperMock(
            int width = 2592,
            int height = 1944,
            int jpegQuality = 100,
            bool addRawBayerData = false,
            PictureEncoding encoding = PictureEncoding.bmp,
            SensorMode mode = SensorMode.Auto,
            int sharpness = 0,
            int contrast = 0,
            int brightness = 50,
            int saturation = 0,
            int ISO = 450,
            int ev = 0,
            Exposure exposure = Exposure.auto,
            AWB awb = AWB.auto,
            Effect effect = Effect.none,
            //byte colorFxU = 128,
            //byte colorFxV = 128,
            Metering metering = Metering.average,
            int rotation = 0,
            bool hFlip = false,
            bool vFlip = false,
            float roiX = 0,
            float roiY = 0,
            float roiW = 1,
            float roiH = 1,
            float awbgB = 1,
            float awbgR = 1,
            DRC drc = DRC.off,
            bool stats = false)
            : this(
                new GenericParameter<int>("width", width),
                new GenericParameter<int>("height", height),
                new GenericParameter<int>("quality", jpegQuality),
                new ConditionalParameter("raw", addRawBayerData),
                new GenericParameter<int>("timeout", int.MaxValue),
                new GenericParameter<PictureEncoding>("encoding", encoding),
                new CalculatedParameter<SensorMode>("mode", mode, m => ((int)m).ToString()),
                new GenericParameter<int>("sharpness", sharpness),
                new GenericParameter<int>("contrast", contrast),
                new GenericParameter<int>("brightness", brightness),
                new GenericParameter<int>("saturation", saturation),
                new GenericParameter<int>("ISO", ISO),
                new GenericParameter<int>("ev", ev),
                new GenericParameter<Exposure>("exposure", exposure),
                new GenericParameter<AWB>("awb", awb),
                new GenericParameter<Effect>("imxfx", effect),
                //new CalculatedParameter<Tuple<byte,byte>>("colfx", new Tuple<byte,byte>(colorFxU, colorFxV), t=>t.Item1+":"+t.Item2),
                new GenericParameter<Metering>("metering", metering),
                new GenericParameter<int>("rotation", rotation),
                new ConditionalParameter("hflip", hFlip),
                new ConditionalParameter("vflip", vFlip),
                new CalculatedParameter<Tuple<float, float, float, float>>("roi", new Tuple<float, float, float, float>(roiX, roiY, roiW, roiH), t => t.Item1 + "," + t.Item2 + "," + t.Item3 + "," + t.Item4),
                new CalculatedParameter<Tuple<float, float>>("awbgains", new Tuple<float, float>(awbgB, awbgR), t => t.Item1 + "," + t.Item2),
                new GenericParameter<DRC>("drc", drc),
                new ConditionalParameter("stats", stats)
                )
        {

        }
        public RaspicamWrapperMock(RaspicamParameters p)
            : this(p.width, p.height, p.jpegQuality, p.addRawBayerData, p.encoding, p.mode, p.sharpness, p.contrast, p.brightness, p.saturation, p.ISO, p.ev, p.exposure, p.awb, p.effect, p.metering, p.rotation, p.hFlip, p.vFlip, p.roiX, p.roiY, p.roiW, p.roiH, p.awbgB, p.awbgR, p.drc, p.stats)
        {

        }


        public new byte[] Capture()
        {
            return File.ReadAllBytes(@"c:\temp\faces\0008\08.jpg");
        }

        public new void Close()
        {
        }
    }

    public class RaspicamWrapper
    {
        public Process raspistill;
        public byte[] buffer;

        protected RaspicamWrapper()
        {
        }

        public RaspicamWrapper(string parameters)
        {
            raspistill = Process.Start(new ProcessStartInfo("raspistill", parameters) { RedirectStandardInput = true, RedirectStandardOutput = true, UseShellExecute = false });
            buffer = new byte[15116598];
        }

        public RaspicamWrapper(params IParameter[] parameters) : this(string.Join(" ", parameters.Select(x => x.CollectString)) + " -k -n -o -")
        {
        }

        /// <summary>
        ///		
        ///	-?, --help      : This help information
        ///	-w, --width     : Set image width <size>
        ///	-h, --height    : Set image height <size>
        ///	-q, --quality   : Set jpeg quality <0 to 100>
        ///	-r, --raw       : Add raw bayer data to jpeg metadata
        ///	-o, --output    : Output filename <filename> (to write to stdout, use '-o -'). If not specified, no file is saved
        ///	-l, --latest    : Link latest complete image to filename <filename>
        ///	-v, --verbose   : Output verbose information during run
        ///	-t, --timeout   : Time (in ms) before takes picture and shuts down (if not specified, set to 5s)
        ///	-th, --thumb    : Set thumbnail parameters (x:y:quality) or none
        ///	-d, --demo      : Run a demo mode (cycle through range of camera options, no capture)
        ///	-e, --encoding  : Encoding to use for output file (jpg, bmp, gif, png)
        ///	-x, --exif      : EXIF tag to apply to captures (format as 'key=value') or none
        ///	-tl, --timelapse        : Timelapse mode. Takes a picture every <t>ms
        ///	-fp, --fullpreview      : Run the preview using the still capture resolution (may reduce preview fps)
        ///	-k, --keypress  : Wait between captures for a ENTER, X then ENTER to exit
        ///	-s, --signal    : Wait between captures for a SIGUSR1 from another process
        ///	-g, --gl        : Draw preview to texture instead of using video render component
        ///	-gc, --glcapture        : Capture the GL frame-buffer instead of the camera image
        ///	-set, --settings        : Retrieve camera settings and write to stdout
        ///	-cs, --camselect        : Select camera <number>. Default 0
        ///	-bm, --burst    : Enable 'burst capture mode'
        ///	-md, --mode     : Force sensor mode. 0=auto. See docs for other modes available
        ///	-dt, --datetime : Replace frame number in file name with DateTime (MonthDayHourMinSec)
        ///	-ts, --timestamp        : Replace frame number in file name with unix timestamp (seconds since 1900)
        ///	-fs, --framestart       : Starting frame number in output pattern
        ///	----------------------------------------------------------------------------------------
        /// -p, --preview	: Preview window settings <'x,y,w,h'>
        /// -f, --fullscreen	: Fullscreen preview mode
        /// -op, --opacity	: Preview window opacity (0-255)
        /// -n, --nopreview	: Do not display a preview window
        ///	----------------------------------------------------------------------------------------
        ///	-sh, --sharpness        : Set image sharpness (-100 to 100)
        ///	-co, --contrast : Set image contrast (-100 to 100)
        ///	-br, --brightness       : Set image brightness (0 to 100)
        ///	-sa, --saturation       : Set image saturation (-100 to 100)
        ///	-ISO, --ISO     : Set capture ISO
        ///	-vs, --vstab    : Turn on video stabilisation
        ///	-ev, --ev       : Set EV compensation - steps of 1/6 stop
        ///	-ex, --exposure : Set exposure mode (see Notes)
        ///	-awb, --awb     : Set AWB mode (see Notes)
        ///	-ifx, --imxfx   : Set image effect (see Notes)
        ///	-cfx, --colfx   : Set colour effect (U:V)
        ///	-mm, --metering : Set metering mode (see Notes)
        ///	-rot, --rotation        : Set image rotation (0-359)
        ///	-hf, --hflip    : Set horizontal flip
        ///	-vf, --vflip    : Set vertical flip
        ///	-roi, --roi     : Set region of interest (x,y,w,d as normalised coordinates [0.0-1.0])
        ///	-ss, --shutter  : Set shutter speed in microseconds
        ///	-awbg, --awbgains       : Set AWB gains - AWB mode must be off
        ///	-drc, --drc     : Set DRC Level
        ///	-st, --stats    : Force recomputation of statistics on stills capture pass
        ///	-a, --annotate  : Enable/Set annotate flags or text
        ///	-3d, --stereo   : Select stereoscopic mode
        ///	-dec, --decimate        : Half width/height of stereo image
        ///	-3dswap, --3dswap       : Swap camera order for stereoscopic
        ///	-ae, --annotateex       : Set extra annotation parameters (text size, text colour(hex YUV), bg colour(hex YUV))
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="jpegQuality">0 - 100 : default(100)</param>
        /// <param name="addRawBayerData">add raw bayer data to image metadata</param>
        /// <param name="encoding">image format</param>
        /// <param name="mode">Sensor mode</param>
        /// <param name="sharpness">-100 - 100 : default(0)</param>
        /// <param name="contrast">-100 - 100 : default(0)</param>
        /// <param name="brightness">0 - 100 : default(50)</param>
        /// <param name="saturation">-100 - 100 : default(0)</param>
        /// <param name="ISO">100 - 800</param>
        /// <param name="ev">-10 - 10 : default(0)</param>
        /// <param name="exposure">default:auto, not all will allways work</param>
        /// <param name="awb">default:auto</param>
        /// <param name="effect">default: none</param>
        /// <param name="colorFxU"></param>
        /// <param name="colorFxV"></param>
        /// <param name="metering"></param>
        /// <param name="rotation">0 - 359</param>
        /// <param name="hFlip">h flip</param>
        /// <param name="vFlip">v flip</param>
        /// <param name="roiX">ROI x</param>
        /// <param name="roiY">ROI y</param>
        /// <param name="roiW">ROI width</param>
        /// <param name="roiH">ROI height</param>
        /// <param name="awbgB">AWB Gains B</param>
        /// <param name="awbgR">AWB Gains R</param>
        /// <param name="drc">DynamicRangeCompression default(off)</param>
        /// <param name="stats">If set to <c>true</c> stats.</param>
        public RaspicamWrapper(
            int width = 2592,
            int height = 1944,
            int jpegQuality = 100,
            bool addRawBayerData = false,
            PictureEncoding encoding = PictureEncoding.bmp,
            SensorMode mode = SensorMode.Auto,
            int sharpness = 0,
            int contrast = 0,
            int brightness = 50,
            int saturation = 0,
            int ISO = 450,
            int ev = 0,
            Exposure exposure = Exposure.auto,
            AWB awb = AWB.auto,
            Effect effect = Effect.none,
            //byte colorFxU = 128,
            //byte colorFxV = 128,
            Metering metering = Metering.average,
            int rotation = 0,
            bool hFlip = false,
            bool vFlip = false,
            float roiX = 0,
            float roiY = 0,
            float roiW = 1,
            float roiH = 1,
            float awbgB = 1,
            float awbgR = 1,
            DRC drc = DRC.off,
            bool stats = false)
            : this(
                new GenericParameter<int>("width", width),
                new GenericParameter<int>("height", height),
                new GenericParameter<int>("quality", jpegQuality),
                new ConditionalParameter("raw", addRawBayerData),
                new GenericParameter<int>("timeout", int.MaxValue),
                new GenericParameter<PictureEncoding>("encoding", encoding),
                new CalculatedParameter<SensorMode>("mode", mode, m => ((int)m).ToString()),
                new GenericParameter<int>("sharpness", sharpness),
                new GenericParameter<int>("contrast", contrast),
                new GenericParameter<int>("brightness", brightness),
                new GenericParameter<int>("saturation", saturation),
                new GenericParameter<int>("ISO", ISO),
                new GenericParameter<int>("ev", ev),
                new GenericParameter<Exposure>("exposure", exposure),
                new GenericParameter<AWB>("awb", awb),
                new GenericParameter<Effect>("imxfx", effect),
                //new CalculatedParameter<Tuple<byte,byte>>("colfx", new Tuple<byte,byte>(colorFxU, colorFxV), t=>t.Item1+":"+t.Item2),
                new GenericParameter<Metering>("metering", metering),
                new GenericParameter<int>("rotation", rotation),
                new ConditionalParameter("hflip", hFlip),
                new ConditionalParameter("vflip", vFlip),
                new CalculatedParameter<Tuple<float, float, float, float>>("roi", new Tuple<float, float, float, float>(roiX, roiY, roiW, roiH), t => t.Item1 + "," + t.Item2 + "," + t.Item3 + "," + t.Item4),
                new CalculatedParameter<Tuple<float, float>>("awbgains", new Tuple<float, float>(awbgB, awbgR), t => t.Item1 + "," + t.Item2),
                new GenericParameter<DRC>("drc", drc),
                new ConditionalParameter("stats", stats)
                )
        {

        }
        public RaspicamWrapper(RaspicamParameters p)
            : this(p.width, p.height, p.jpegQuality, p.addRawBayerData, p.encoding, p.mode, p.sharpness, p.contrast, p.brightness, p.saturation, p.ISO, p.ev, p.exposure, p.awb, p.effect, p.metering, p.rotation, p.hFlip, p.vFlip, p.roiX, p.roiY, p.roiW, p.roiH, p.awbgB, p.awbgR, p.drc, p.stats)
        {

        }

        public byte[] Capture()
        {
            raspistill.StandardOutput.DiscardBufferedData();
            raspistill.StandardInput.WriteLine("\n");
            var bytes = 0;

            do
            {
                bytes += raspistill.StandardOutput.BaseStream.Read(buffer, bytes, buffer.Length - bytes);
            } while (bytes < buffer.Length);

            return buffer;
        }

        public void Close()
        {
            raspistill.Kill();
        }
    }

    public interface IParameter
    {
        string CollectString { get; }
    }
    public class GenericParameter<T> : IParameter
    {
        private string name;
        private T arg;
        public string CollectString { get { return "--" + name + " " + arg.ToString(); } }

        public GenericParameter(string name, T argument)
        {
            this.name = name;
            this.arg = argument;
        }
    }
    public class ConditionalParameter : IParameter
    {
        private string name;
        private bool value;
        public string CollectString { get { return value ? "" : "--" + name; } }

        public ConditionalParameter(string name, bool value)
        {
            this.name = name;
            this.value = value;
        }
    }

    public class CalculatedParameter<T> : IParameter
    {
        private string name;
        private T value;
        private Func<T, string> converter;

        public string CollectString { get { return "--" + name + " " + converter(value); } }

        public CalculatedParameter(string name, T value, Func<T, string> converter)
        {
            this.converter = converter;
            this.name = name;
            this.value = value;
        }
    }

    public class CameraService : ICameraService
    {
        bool setUp = false;
        RaspicamWrapperMock camera;
        public void SetupCamera(RaspicamParameters parameters)
        {
            if (camera != null)
                camera.Close();
            camera = new RaspicamWrapperMock(parameters);
            setUp = true;
        }
        public byte[] CaptureImage()
        {
            if (!setUp)
                return new byte[0];
            return camera.Capture();
        }
    }
}
