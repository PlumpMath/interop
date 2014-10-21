using System.ServiceModel;
using Aamva.Nmvtis.EwsR2.JurServiceInterface;
using Aamva.Nmvtis.MessageContracts.External;

namespace NmvtisServiceHost
{
	public class ServiceImpl : INmvtisReceiveResponses
	{
		public void VehicleHistoryInquiryResponse(byte[] authToken, string corrToken, PerformVehicleHistoryInquiryResponseType response)
		{
			throw new FaultException("Method VehicleHistoryInquiryResponse Is Not Implemented");
		}

		public void UsedVehicleInquiryResponse(byte[] authToken, string corrToken, PerformUsedVehicleInquiryResponseType response)
		{
			throw new FaultException("Method UsedVehicleInquiryResponse is Not Implemented");
		}
	}
}
