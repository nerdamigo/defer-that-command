using domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace api_service
{
	public class CommandControllerDescriptorProvider
	{
		private readonly Dictionary<HttpControllerDescriptor, CommandControllerDescriptor> mDescriptorIndex;
		private readonly Dictionary<string, CommandControllerDescriptor> mDescriptorsByNameActionAndRequestMethod;
		private readonly Dictionary<Type, CommandControllerDescriptor> mDescriptorByMessageType;

		public CommandControllerDescriptorProvider(params Assembly[] aAssemblies)
		{
			IEnumerable<Type> tConcreteTypes = aAssemblies.SelectMany(s => s.GetExportedTypes())
				.Where(w => !w.IsAbstract && !w.IsGenericTypeDefinition && w.GetInterfaces().Contains(typeof(ICommand)))
				.Select(s => s);

			IEnumerable<CommandControllerDescriptor> tCommandDescriptors =
				tConcreteTypes.Where(w => w.Name.EndsWith("Command"))
				.Select(type =>
				{
					string tControllerName = type.Namespace.Split('.').Skip(1).Take(1).First();
					string tActionName = type.Name.Replace("Command", "");
					return new CommandControllerDescriptor(
						aDescriptor: BuildHttpDescriptor(type, tControllerName),
						aActionName: tActionName,
						aMessageType: type,
						aMethod: WebApiMethodAttribute.GetMethod(type) ?? "POST"
					);
				});

			this.mDescriptorIndex = tCommandDescriptors.ToDictionary(descriptor => descriptor.ControllerDescriptor, descriptor => descriptor);
			this.mDescriptorByMessageType = tCommandDescriptors.ToDictionary(descriptor => descriptor.MessageType, descriptor => descriptor);
			this.mDescriptorsByNameActionAndRequestMethod = tCommandDescriptors.ToDictionary(
				descriptor => CreateDescriptorKey(descriptor.ControllerName, descriptor.ActionName, descriptor.HttpRequestMethod),
				descriptor => descriptor
			);
		}

		private string CreateDescriptorKey(string aControllerName, string aActionName, string aHttpMethod)
		{
			return String.Format("{0}.{1} [{2}]", aControllerName, aActionName, aHttpMethod).ToLowerInvariant();
		}

		private HttpControllerDescriptor BuildHttpDescriptor(Type aCommandType, string tControllerName)
		{
			return new HttpControllerDescriptor(GlobalConfiguration.Configuration, tControllerName, typeof(CommandController<>).MakeGenericType(aCommandType));
		}

		public CommandControllerDescriptor GetControllerDescriptor(Type aMessageType)
		{
			CommandControllerDescriptor tDescriptor;
			return mDescriptorByMessageType.TryGetValue(aMessageType, out tDescriptor) ? tDescriptor : null;
		}

		public CommandControllerDescriptor GetControllerDescriptor(string aControllerName, string aActionName, string aHttpMethod)
		{
			CommandControllerDescriptor tDescriptor;
			return mDescriptorsByNameActionAndRequestMethod.TryGetValue(CreateDescriptorKey(aControllerName, aActionName, aHttpMethod), out tDescriptor) ? tDescriptor : null;
		}

		public CommandControllerDescriptor GetControllerDescriptor(HttpControllerDescriptor aHttpControllerDescriptor)
		{
			CommandControllerDescriptor tDescriptor;
			return mDescriptorIndex.TryGetValue(aHttpControllerDescriptor, out tDescriptor) ? tDescriptor : null;
		}

		public IEnumerable<CommandControllerDescriptor> GetDescriptors()
		{
			return this.mDescriptorIndex.Values;
		}
	}
}
