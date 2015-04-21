using domain.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.CommandHandler
{
	public class CreateMediaCommandHandler : ICommandHandler<CreateMediaCommand>
	{
		public CreateMediaCommand Execute(CreateMediaCommand aCommand)
		{
			throw new NotImplementedException();
		}
	}
}
