using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain
{
	public interface ICommandHandler<TCommand> where TCommand : ICommand
	{
		TCommand Execute(TCommand aCommand);
	}
}
