using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace RaspicamServer
{
	
	public class ConditionalParameter:IParameter
	{
		private string name;
		private bool value;
		public string CollectString { get{return value?"":"--"+name; } }

		public ConditionalParameter (string name, bool value)
		{
			this.name = name;
			this.value = value;
		}
	}
	
}
