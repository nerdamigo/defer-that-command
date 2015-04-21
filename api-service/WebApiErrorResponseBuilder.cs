using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_service
{
	public class WebApiErrorResponseBuilder
	{
		public static System.Net.Http.HttpResponseMessage CreateErrorResponse(Exception aThrownException, System.Net.Http.HttpRequestMessage aRequestMessage)
		{
			//handle various business layer exceptions and return meaningful data to the client
			//examples: Validation, Authorization, IDNotFound, etc etc


			//return null if we can't handle this (hammer time)
			return null;
		}
	}
}
