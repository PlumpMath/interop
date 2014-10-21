using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Templates
{
	public abstract class ManagerBase
	{
		protected string UserName { get; private set; }

		public ManagerBase()
		{
			OperationContext context = OperationContext.Current;
 			if(context!=null)
			{
				UserName = context.IncomingMessageHeaders.GetHeader<string>("LoginName", "System");
			}
		}
	}
}
