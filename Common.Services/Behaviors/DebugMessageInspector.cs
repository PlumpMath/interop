using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Behaviors
{
	public class DebugMessageInspector : IClientMessageInspector, IDispatchMessageInspector
	{
		#region client
		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			MessageBuffer buffer = request.CreateBufferedCopy(int.MaxValue);
			request = buffer.CreateMessage();
			Message m = buffer.CreateMessage();
			m.LogMessage("Client: BeforeSendRequest");
			return request;
		}

		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
			MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
			reply = buffer.CreateMessage();
			Message m = buffer.CreateMessage();
			m.LogMessage("Client: AfterReceiveReply");
		}
		#endregion

		#region dispatcher
		public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
		{
			MessageBuffer buffer = request.CreateBufferedCopy(int.MaxValue);
			request = buffer.CreateMessage();
			Message m = buffer.CreateMessage();
			m.LogMessage("Dispatcher: AfterReceiveRequest");
			return request;
		}

		public void BeforeSendReply(ref Message reply, object correlationState)
		{
			MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
			reply = buffer.CreateMessage();
			Message m = buffer.CreateMessage();
			m.LogMessage("Dispatcher: BeforeSendReply");
		}
		#endregion
	}
}
