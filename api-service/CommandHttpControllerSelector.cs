using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace api_service
{
	public class CommandHttpControllerSelector : DefaultHttpControllerSelector
	{
		private readonly CommandControllerDescriptorProvider mCommandControllerDescriptorProvider;

		public CommandHttpControllerSelector(CommandControllerDescriptorProvider aCommandControllerDescriptorProvider) :
			base(System.Web.Http.GlobalConfiguration.Configuration)
		{
			this.mCommandControllerDescriptorProvider = aCommandControllerDescriptorProvider;
		}

		public override HttpControllerDescriptor SelectController(System.Net.Http.HttpRequestMessage request)
		{
			string tControllerName = this.GetControllerName(request);
			string tActionName = request.GetRouteData().Values["action"] as string ?? string.Empty;

			CommandControllerDescriptor tDescriptor = this.mCommandControllerDescriptorProvider.GetControllerDescriptor(tControllerName, tActionName, request.Method.Method);

			return tDescriptor != null ? tDescriptor.ControllerDescriptor : base.SelectController(request);
		}
	}
}
