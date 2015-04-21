using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_service
{
	public class CommandControllerParameter
	{
		public CommandControllerParameter(string name, Type type)
		{
			this.Name = name;
			this.Type = type;
		}

		public string Name { get; private set; }
		public Type Type { get; private set; }
	}
}
