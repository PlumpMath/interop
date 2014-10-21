using Common.Services.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TechTalk.SpecFlow;

namespace Common.Services.Tests.Steps
{
	[Binding]
	public class DynamicHostSteps
	{
		[Given(@"wsdl url '(.*)'")]
		public void GivenWsdlUrl(string wsdlUrl)
		{
			ScenarioContext.Current.Set(wsdlUrl, "MetaDataUrl");
		}

		[When(@"I should discover (.*) interface with name '(.*)'")]
		public void ThenIShouldDiscoverInterfaceWithName(int interfaceCount, string contractName)
		{
			var wsdlAssemblyFilePath = ScenarioContext.Current.Get<string>("WsdlAssemblyFilePath");
			Assert.IsTrue(!string.IsNullOrEmpty(wsdlAssemblyFilePath) && File.Exists(wsdlAssemblyFilePath), "Unable to find wsdl assembly file");
			Assembly assembly = Assembly.LoadFrom(wsdlAssemblyFilePath);
			ScenarioContext.Current.Set(assembly, "WsdlAssembly");
			var interfaceTypes = assembly.GetTypes().Where(t => t.IsInterface && t.GetMethods().Any()).ToList();
			Assert.AreEqual(interfaceCount, interfaceTypes.Count, "Number of interface do not agree");
			var contractType = interfaceTypes.FirstOrDefault(t => t.Name == contractName);
			Assert.IsNotNull(contractType, "Unable to find contract type");
			contractType.Log("Contract from compiled WSDL assembly");
			ScenarioContext.Current.Set(contractType, "ContractType");
		}

		[When(@"there should be a method with name '(.*)'")]
		public void ThenThereShouldBeAMethodWithName(string methodName)
		{
			Type contractType = ScenarioContext.Current.Get<Type>("ContractType");
			Assert.IsNotNull(contractType, "Unable to get contract type");
			var method = contractType.GetMethods().FirstOrDefault(m => m.Name == methodName);
			Assert.IsNotNull(method, "Unable to get method " + methodName);
			var args = method.GetParameters().ToList();
			LogFactory.GetLog().Information(
				"Method {0} have {1} arguments, return type = {2}", 
				methodName, args.Count, method.ReturnType.Name);
			foreach(var arg in args)
			{
				string direction = arg.IsOut ? "out" : "in";
				if (arg.ParameterType.IsByRef)
					direction = "ref";
				LogFactory.GetLog().Information("Argument: type={0}, direction={1}", arg.ParameterType.Name, direction);
			}
			//int expectedArgCount = table.RowCount;
			
			//Assert.AreEqual(expectedArgCount, args.Count, "Method arg count do not agree");
			//foreach(TableRow row in table.Rows)
			//{
			//	string argType = row["Type"];
			//	var arg = method.GetParameters().FirstOrDefault(p => p.ParameterType.Name == argType);
			//	Assert.IsNotNull(arg, "Unable to find method argument of type " + argType);
			//}
		}

		[Then(@"the assembly used for wsdl host should contain non-wrapped methods")]
		public void ThenTheAssemblyUsedForWsdlHostShouldContainNon_WrappedMethods()
		{
			var wsdlAssembly = ScenarioContext.Current.Get<Assembly>("WsdlAssembly");
			Assert.IsNotNull(wsdlAssembly);
			var contractType = ScenarioContext.Current.Get<Type>("ContractType");
			Assert.IsNotNull(contractType, "Unable to get contract type");
			var contractType2 = wsdlAssembly.GetTypes().FirstOrDefault(t => t.Name == contractType.Name);
			Assert.IsNotNull(contractType2, "Unable to retrieve contract type from wsdl assembly");
			Assert.IsFalse(ReflectionUtil.IsInterfaceWrapped(contractType2), "Interface method should not be wrapped");
		}

	}
}
