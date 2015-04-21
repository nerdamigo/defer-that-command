using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_service
{
	public class WebApiMethodAttribute : Attribute
	{
		public WebApiMethodAttribute(string aMethod)
		{
			this.Method = aMethod;
		}

		public string Method { get; set; }

		public static string GetMethod(Type aType)
		{
			var tAttributes = aType.GetCustomAttributes(typeof(WebApiMethodAttribute), false);
			return tAttributes.Count() > 0 ? (tAttributes[0] as WebApiMethodAttribute).Method : null;
		}
	}
}
