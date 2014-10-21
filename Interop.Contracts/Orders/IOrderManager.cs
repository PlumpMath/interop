using System.ServiceModel;
using Interop.Contracts.Orders.Errors;

namespace Interop.Contracts.Orders
{
	[ServiceContract(Namespace = "AAMVA.Orders")]
	public interface IOrderManager
	{
		[OperationContract]
		[FaultContract(typeof(InvalidAuthTokenError))]
		string SubmitOrder(byte[] authToken, Order myOrder);

		[OperationContract]
		[FaultContract(typeof(OrderNotFoundError))]
		[FaultContract(typeof(InvalidAuthTokenError))]
		OrderConfirmation ConfirmOrder(string correlation, byte[] authToken);
	}
}
