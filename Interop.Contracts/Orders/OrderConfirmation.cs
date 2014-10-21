using System;
using System.Runtime.Serialization;

namespace Interop.Contracts.Orders
{
	[DataContract]
	public class OrderConfirmation
	{
		[DataMember]
		public string ConfirmationNumber { get; set; }
		[DataMember]
		public DateTime ShippingDate { get; set; }
		[DataMember]
		public decimal TotalCost { get; set; }
	}
}
