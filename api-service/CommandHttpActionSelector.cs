using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace api_service
{
	public class CommandHttpActionSelector : ApiControllerActionSelector
	{
		private readonly CommandControllerDescriptorProvider mControllerDescriptorProvider;

		public CommandHttpActionSelector(CommandControllerDescriptorProvider aControllerDescriptorProvider)
		{
			this.mControllerDescriptorProvider = aControllerDescriptorProvider;
		}

		public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
		{
			CommandControllerDescriptor tDescriptor = this.mControllerDescriptorProvider.GetControllerDescriptor(
				controllerContext.ControllerDescriptor.ControllerName,
				controllerContext.RouteData.Values["action"] as string,
				controllerContext.Request.Method.Method
				);

			if (tDescriptor != null)
			{
				Type tControllerType = tDescriptor.ControllerDescriptor.ControllerType;

				return new ReflectedHttpActionDescriptor(controllerContext.ControllerDescriptor,
					tControllerType.GetMethod("Execute", BindingFlags.Public | BindingFlags.Instance)
				);
			}

			return base.SelectAction(controllerContext);
		}
	}
}
