using System.Runtime.Serialization;

namespace Common.Services.Templates
{
	[DataContract]
	public class NotImplementedError
	{
		public string MethodName { get; set; }

		public NotImplementedError(string methodName)
		{
			this.MethodName = methodName;
		}
	}
}
