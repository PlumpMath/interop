namespace Common.Services.Wrappers
{
	public static class ObjectExtensions
	{
		public static T As<T>(this object realObject) where T : class
		{
			if (realObject is T)
				return realObject as T;

			var wraper = new DynamicWrapper();
			return wraper.CreateWrapper<T>(realObject);
		}

		public static T AsReal<T>(this object wrapper) where T : class
		{
			if (wrapper is T)
				return wrapper as T;

			if (wrapper is DynamicWrapper.DynamicWrapperBase)
				return (T)(wrapper as DynamicWrapper.DynamicWrapperBase).RealObject;

			return null;
		}
	}
}
