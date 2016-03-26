using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace RaspicamServer
{
	
	public interface IParameter
	{
		string CollectString { get; }
	}
	
}
