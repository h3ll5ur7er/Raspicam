using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace RaspicamServer
{

	public class CameraService : ICameraService
	{
		bool setUp = false;
		RaspicamWrapper camera;
		public void SetupCamera(RaspicamParameters parameters)
		{
			if (camera != null)
				camera.Close ();
			camera = new RaspicamWrapper (parameters);
			setUp = true;
		}
		public byte[] CaptureImage()
		{
			if (!setUp)
				return new byte[0];
			return camera.Capture ();
		}
	}
}
