using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace RaspicamServer
{

	public class CalculatedParameter<T>:IParameter
	{
		private string name;
		private T value;
		private Func<T, string> converter;

		public string CollectString { get{return "--"+name+" "+ converter(value); } }

		public CalculatedParameter (string name, T value, Func<T,string> converter)
		{
			this.converter = converter;
			this.name = name;
			this.value = value;
		}
	}
	
}
