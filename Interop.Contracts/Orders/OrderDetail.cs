using System.Runtime.Serialization;

namespace Interop.Contracts.Orders
{
	[DataContract]
	public class OrderDetail
	{
		[DataMember]
		public Product Product { get; set; }
		[DataMember]
		public int Quantity { get; set; }
	}
}
