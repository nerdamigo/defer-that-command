using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace api_service
{
	public class CommandControllerDescriptor : HttpControllerDescriptor
	{
		public CommandControllerDescriptor(HttpControllerDescriptor aDescriptor, string aActionName, Type aMessageType, string aMethod)
		{
			this.ControllerDescriptor = aDescriptor;
			this.ControllerName = aDescriptor.ControllerName;
			this.ActionName = aActionName;
			this.MessageType = aMessageType;
			this.HttpRequestMethod = aMethod;
		}

		public HttpControllerDescriptor ControllerDescriptor { get; private set; }
		public string ActionName { get; private set; }
		public Type MessageType { get; private set; }
		public string HttpRequestMethod { get; private set; }

		public IEnumerable<CommandControllerParameter> GetWebApiParameters()
		{
			return GetWebApiParameters(this.MessageType);
		}

		private IEnumerable<CommandControllerParameter> GetWebApiParameters(Type aType)
		{
			foreach (var property in aType.GetProperties())
			{
				Type propertyType = property.PropertyType;

				if (propertyType.IsPrimitive || propertyType == typeof(string) ||
					(propertyType.Namespace.StartsWith("System") && propertyType.IsValueType))
				{
					yield return new CommandControllerParameter(property.Name, property.PropertyType);
				}
				else
				{
					foreach (var subProperty in this.GetWebApiParameters(property.PropertyType))
					{
						yield return new CommandControllerParameter(property.Name + "." + subProperty.Name, subProperty.Type);
					}
				}
			}
		}
	}
}
