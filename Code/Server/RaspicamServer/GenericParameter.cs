using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace RaspicamServer
{
	
	public class GenericParameter<T>:IParameter
	{
		private string name;
		private T arg;
		public string CollectString { get{return "--"+name+" "+arg.ToString(); } }

		public GenericParameter (string name, T argument)
		{
			this.name = name;
			this.arg = argument;
		}
	}
	
}
