using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Interop.Contracts.Orders
{
	[DataContract]
	public class Order
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string User { get; set; }
		[DataMember]
		public ShippingInfo Shipping { get; set; }
		[DataMember]
		public List<OrderDetail> OrderDetails { get; set; }
		[DataMember]
		public decimal ShippingCost { get; set; }

	}
}
