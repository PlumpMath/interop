using System.ServiceModel;
using Interop.Contracts.Orders.Errors;

namespace Interop.Contracts.Orders
{
	[ServiceContract(Namespace = "AAMVA.Orders")]
	public interface ISecurityManager
	{
		[OperationContract]
		[FaultContract(typeof(InvalidUserError))]
		byte[] Authenticate(string user, string password);

		[OperationContract]
		[FaultContract(typeof(InvalidAuthTokenError))]
		bool ValidateToken(byte[] authToken);
	}
}
