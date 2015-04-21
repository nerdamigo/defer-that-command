using domain;
using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.WebHost;
using System.Web.Security;
using System.Web.SessionState;

namespace api_service
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			Container tContainer = new Container();

			//register command handlers, and generic command controller
			CommandControllerDescriptorProvider provider = new CommandControllerDescriptorProvider(typeof(ICommand).Assembly);
			tContainer.RegisterManyForOpenGeneric(typeof(ICommandHandler<>), typeof(ICommand).Assembly);
			tContainer.RegisterSingle<CommandControllerDescriptorProvider>(provider);
			tContainer.RegisterSingle<IHttpControllerSelector, CommandHttpControllerSelector>();
			tContainer.RegisterSingle<IHttpActionSelector, CommandHttpActionSelector>();

			//ensure command handlers all get properly verified by container
			foreach (var commandDescriptor in provider.GetDescriptors())
			{
				tContainer.Register(commandDescriptor.ControllerDescriptor.ControllerType);
			}

			tContainer.RegisterWebApiControllers(GlobalConfiguration.Configuration);
			GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(tContainer);

			tContainer.Verify();

			GlobalConfiguration.Configuration.Routes.MapHttpRoute(
				name: "Command",
				routeTemplate: "api/{controller}/{action}"
			);
		}
	}
}