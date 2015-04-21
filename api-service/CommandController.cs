using domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace api_service
{
	public class CommandController<TCommand> : ApiController where TCommand : ICommand
	{
		private static readonly HttpStatusCode mSuccessCodeForCommandType;
		private readonly ICommandHandler<TCommand> mCommandHandler;

		static CommandController()
		{
			mSuccessCodeForCommandType = WebApiResponseAttribute.GetStatusCode(typeof(TCommand));
		}

		public CommandController(ICommandHandler<TCommand> aCommandHandler)
		{
			mCommandHandler = aCommandHandler;
		}

		public HttpResponseMessage Execute([FromBody]TCommand aCommand)
		{
			if (aCommand == null)
			{
				throw new ArgumentNullException("aCommand");
			}

			TCommand tExecutedCommand;
			try
			{
				tExecutedCommand = this.mCommandHandler.Execute(aCommand);
			}
			catch (Exception ex)
			{
				HttpResponseMessage tResponse = WebApiErrorResponseBuilder.CreateErrorResponse(ex, this.Request);
				if (tResponse != null)
				{
					return tResponse;
				}
				throw;
			}
			return this.Request.CreateResponse<TCommand>(mSuccessCodeForCommandType, tExecutedCommand);
		}
	}
}
