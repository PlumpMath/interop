﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Common.Services.Wrappers
{
	internal class WrapperMethodBuilder
	{
		private readonly Type _realObjectType;
		private readonly TypeBuilder _wrapperBuilder;
		private readonly bool _ignoreParameterType;
		private readonly string _defaultMethod;
		private IDictionary<string, string> _methodsMap;

		public WrapperMethodBuilder(Type realObjectType, TypeBuilder proxyBuilder, string defaultMethod, bool ignoreParameterType)
		{
			_realObjectType = realObjectType;
			_wrapperBuilder = proxyBuilder;
			_defaultMethod = defaultMethod;
			_ignoreParameterType = ignoreParameterType;
			_methodsMap = null;
		}
		public WrapperMethodBuilder(Type realObjectType, TypeBuilder proxyBuilder, IDictionary<string, string> methodsMap)
		{
			_realObjectType = realObjectType;
			_wrapperBuilder = proxyBuilder;
			_defaultMethod = null;
			_ignoreParameterType = true;
			_methodsMap = methodsMap;
		}

		public void Generate(MethodInfo newMethod)
		{
			if (newMethod.IsGenericMethod)
				newMethod = newMethod.GetGenericMethodDefinition();

			FieldInfo srcField = typeof(DynamicWrapper.DynamicWrapperBase).GetField("RealObject", BindingFlags.Instance | BindingFlags.NonPublic);
			var parameters = newMethod.GetParameters();
			var parameterTypes = parameters.Select(parameter => parameter.ParameterType).ToArray();
			var methodBuilder = _wrapperBuilder.DefineMethod(
				newMethod.Name,
				MethodAttributes.Public | MethodAttributes.Virtual,
				newMethod.ReturnType,
				parameterTypes);

			if (newMethod.IsGenericMethod)
			{
				methodBuilder.DefineGenericParameters(
					newMethod.GetGenericArguments().Select(arg => arg.Name).ToArray());
			}

			ILGenerator ilGenerator = methodBuilder.GetILGenerator();
			LoadRealObject(ilGenerator, srcField);
			PushParameters(parameters, ilGenerator);
			ExecuteMethod(newMethod, parameterTypes, ilGenerator);
			Return(ilGenerator);
		}

		private static void Return(ILGenerator ilGenerator)
		{
			ilGenerator.Emit(OpCodes.Ret);
		}

		private void ExecuteMethod(MethodBase newMethod, Type[] parameterTypes, ILGenerator ilGenerator)
		{
			MethodInfo srcMethod = GetMethod(newMethod, parameterTypes);
			if (srcMethod == null)
			{
				if (string.IsNullOrEmpty(_defaultMethod) == false)
				{
					srcMethod = _realObjectType.GetMethod(_defaultMethod);
				}
				else if (_methodsMap != null)
				{
					string realObjectMethod;
					if (_methodsMap.TryGetValue(newMethod.Name, out realObjectMethod))
					{
						srcMethod = _realObjectType.GetMethod(realObjectMethod);
					}
				}
			}
			if (srcMethod == null) throw new MissingMethodException("Unable to find method " + newMethod.Name + " in " + _realObjectType.FullName);
			ilGenerator.Emit(OpCodes.Call, srcMethod);
		}

		private MethodInfo GetMethod(MethodBase realMethod, Type[] parameterTypes)
		{
			if (realMethod.IsGenericMethod)
				return _ignoreParameterType ? _realObjectType.GetGenericMethod(realMethod.Name) : _realObjectType.GetGenericMethod(realMethod.Name, parameterTypes);

			return _ignoreParameterType ? _realObjectType.GetMethod(realMethod.Name) : _realObjectType.GetMethod(realMethod.Name, parameterTypes);
		}

		private static void PushParameters(ICollection<ParameterInfo> parameters, ILGenerator ilGenerator)
		{
			for (int i = 1; i < parameters.Count + 1; i++)
				ilGenerator.Emit(OpCodes.Ldarg, i);
		}

		private static void LoadRealObject(ILGenerator ilGenerator, FieldInfo srcField)
		{
			ilGenerator.Emit(OpCodes.Ldarg_0);
			ilGenerator.Emit(OpCodes.Ldfld, srcField);
		}
	}
}
