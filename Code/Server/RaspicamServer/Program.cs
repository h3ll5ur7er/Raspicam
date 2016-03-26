using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace RaspicamServer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			while (true)
			{
				try
				{
					using(ServiceHost host = new ServiceHost (typeof(CameraService), new Uri ("http://192.168.1.35:9000/Capture")))
					{
						ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
						smb.HttpGetEnabled = true;
						host.Description.Behaviors.Add(smb);
						
						host.Description.Endpoints.Add(new ServiceEndpoint(ContractDescription.GetContract(typeof(ICameraService)), new BasicHttpBinding{},new EndpointAddress("http://192.168.1.35:9000/Capture.svc")));
						//host.Description.Endpoints.Add(new ServiceEndpoint(ContractDescription.GetContract(typeof(IMetadataExchange)), MetadataExchangeBindings.CreateMexHttpBinding(), new EndpointAddress("http://192.168.1.35:9000/Capture.mex")));

						Console.WriteLine ("starting host");
						host.Open();
						Console.WriteLine ("listening\npress enter to restart");
						Console.ReadLine();
						host.Close();
					}
				}
				catch (Exception ex)
				{
					// improove errorhandling
					Console.WriteLine (ex);
					Debugger.Break ();
				}
			}
		}
	}
}
