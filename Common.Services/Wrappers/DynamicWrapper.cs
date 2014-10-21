using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Common.Services.Wrappers
{
	public class DynamicWrapper
	{
		public class DynamicWrapperBase
		{
			internal protected object RealObject;
		}

		private readonly ModuleBuilder _moduleBuilder;
		private bool _ignoreParameterType;
		private string _defaultMethod;
		private IDictionary<string, string> _methodsMap;

		public DynamicWrapper()
		{
			var assembly = Thread.GetDomain().DefineDynamicAssembly(new AssemblyName("DynamicWrapper"), AssemblyBuilderAccess.Run);
			_moduleBuilder = assembly.DefineDynamicModule("DynamicWrapperModule", false);
		}

		private static readonly WrapperDictionary WrapperDictionary = new WrapperDictionary();

		public Type GetWrapper(Type interfaceType, Type realObjectType)
		{
			Type wrapperType = WrapperDictionary.GetType(interfaceType, realObjectType);
			if (wrapperType == null)
			{
				wrapperType = GenerateWrapperType(interfaceType, realObjectType);
				WrapperDictionary.SetType(interfaceType, realObjectType, wrapperType);
			}
			return wrapperType;
		}

		private Type GenerateWrapperType(Type interfaceType, Type realObjectType)
		{
			var wrapperName = string.Format("{0}_{1}_Wrapper", interfaceType.Name, realObjectType.Name);

			TypeBuilder wrapperBuilder = _moduleBuilder.DefineType(
				wrapperName,
				TypeAttributes.NotPublic | TypeAttributes.Sealed,
				typeof(DynamicWrapperBase),
				new[] { interfaceType });

			var wrapperMethod = _methodsMap == null
				? new WrapperMethodBuilder(realObjectType, wrapperBuilder, _defaultMethod, _ignoreParameterType)
				: new WrapperMethodBuilder(realObjectType, wrapperBuilder, _methodsMap);
			foreach (MethodInfo method in interfaceType.AllMethods())
			{
				wrapperMethod.Generate(method);
			}
			return wrapperBuilder.CreateType();
		}

		public T CreateWrapper<T>(object realObject) where T : class
		{
			var dynamicType = GetWrapper(typeof(T), realObject.GetType());
			var dynamicWrapper = (DynamicWrapperBase)Activator.CreateInstance(dynamicType);
			dynamicWrapper.RealObject = realObject;
			return dynamicWrapper as T;
		}
		public object CreateWrapper(Type interfaceType, object realObject)
		{
			var dynamicType = GetWrapper(interfaceType, realObject.GetType());
			var dynamicWrapper = (DynamicWrapperBase)Activator.CreateInstance(dynamicType);
			dynamicWrapper.RealObject = realObject;
			return dynamicWrapper;
		}
		public object CreateWrapper(Type interfaceType, object realObject, string defaultMethod)
		{
			_defaultMethod = defaultMethod;
			_ignoreParameterType = true;
			return CreateWrapper(interfaceType, realObject);
		}
		public object CreateWrapper(Type interfaceType, object realObject, bool ignoreParameterType)
		{
			_ignoreParameterType = ignoreParameterType;
			return CreateWrapper(interfaceType, realObject);
		}
		public object CreateWrapper(Type interfaceType, object realObject, IDictionary<string, string> methodsMap)
		{
			_methodsMap = methodsMap;
			return CreateWrapper(interfaceType, realObject);
		}
	}
}
