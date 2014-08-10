using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace SampleMessages
{
	public class AMessage : IMessage
	{
		public String SomethingToSay { get; set; }
	}
}
