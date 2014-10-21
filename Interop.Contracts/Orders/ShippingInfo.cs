using System.Runtime.Serialization;

namespace Interop.Contracts.Orders
{
	[DataContract]
	public class ShippingInfo
	{
		[DataMember]
		public string Street { get; set; }
		[DataMember]
		public string City { get; set; }
	}
}
