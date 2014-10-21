using System.Runtime.Serialization;

namespace Interop.Contracts.Orders
{
	[DataContract]
	public class Product
	{
		[DataMember]
		public string Sku { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public decimal Price { get; set; }
	}
}
