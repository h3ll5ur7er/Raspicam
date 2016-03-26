using System;
using System.IO;
using CameraClient.CameraServiceReference;
using RaspicamServer;

namespace CameraServer
{
    class Program
    {
        static void Main(string[] args)
        {
            CameraServiceClient client = new CameraServiceClient("BasicHttpBinding_ICameraService");
            client.Open();
            Console.WriteLine("client open");
            Console.WriteLine("setting up camera");
            client.SetupCamera(new RaspicamParameters(effect:Effect.none));

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("capturing");
                File.WriteAllBytes($@"C:\temp\snap{new Random().Next(0, 9999).ToString("D4")}.bmp", client.CaptureImage());
                Console.WriteLine("image stored");
            }
        }
    }
}
