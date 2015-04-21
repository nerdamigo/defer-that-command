using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace api_service
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class WebApiResponseAttribute : Attribute
	{
		public WebApiResponseAttribute(HttpStatusCode aStatusCode)
		{
			this.StatusCode = aStatusCode;
		}
		public HttpStatusCode StatusCode { get; private set; }

		public static HttpStatusCode GetStatusCode(Type aType)
		{
			var tAttributes = aType.GetCustomAttributes(typeof(WebApiResponseAttribute), false);
			return tAttributes.Count() > 0 ? (tAttributes[0] as WebApiResponseAttribute).StatusCode : HttpStatusCode.OK;
		}
	}
}
