
using Aamva.Nmvtis.EwsR2.JurServiceInterface;
using Common.Services;
using System;
using System.Threading;

namespace ServiceClientsNoProxy
{
	class Program
	{
		static void Main(string[] args)
		{
			Thread.Sleep(TimeSpan.FromSeconds(3));
			//Type contractType = typeof(INmvtisReceiveResponses);
			ServiceEndpointConfiguration config = new ServiceEndpointConfiguration()
			{
				ServiceAddress = "http://localhost:54321/DynamicHost_NMVTIS"
			};
			INmvtisReceiveResponses proxy = DynamicProxy.Create<INmvtisReceiveResponses>(config);
			try
			{
				proxy.UsedVehicleInquiryResponse(null, null, null);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.ReadLine();
		}
	}
}
