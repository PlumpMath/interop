﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Common.Services.Tests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class HostWsdlServiceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "HostWsdlService.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "HostWsdlService", "", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((TechTalk.SpecFlow.FeatureContext.Current != null) 
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "HostWsdlService")))
            {
                Common.Services.Tests.Features.HostWsdlServiceFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Rehost WSDL with Data Contract and Svcutil")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "HostWsdlService")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("wsdl")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("rehost")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("data_contract")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("svcutil")]
        public virtual void RehostWSDLWithDataContractAndSvcutil()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rehost WSDL with Data Contract and Svcutil", new string[] {
                        "wsdl",
                        "rehost",
                        "data_contract",
                        "svcutil"});
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
 testRunner.Given("a referenced assembly in bin folder called \'Interop.Contracts.dll\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.And("Pick an interface with name \'IOrderManager\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 7
 testRunner.And("set serialization method to \'DataContract\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
 testRunner.And("set generated assembly namespace to \"AAMVA.Orders\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
 testRunner.And("set rehost namespace to \'AAMVA.Orders.Services\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table1.AddRow(new string[] {
                        "ServiceAddress",
                        "http://localhost:54321/WsdlPublish_DataContract"});
            table1.AddRow(new string[] {
                        "BindingType",
                        "BasicHttpBinding"});
            table1.AddRow(new string[] {
                        "SecurityMode",
                        "None"});
            table1.AddRow(new string[] {
                        "ClientCredentialType",
                        "None"});
#line 10
 testRunner.When("I generate and host service with the following setting", ((string)(null)), table1, "When ");
#line 16
 testRunner.And("I generate wsdl assembly and save them to folder \'wsdl-generated\' with name \'Orde" +
                    "rManager.wsdl.dll\' using SvcUtil", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table2.AddRow(new string[] {
                        "ServiceAddress",
                        "http://localhost:54321/WsdlHost_DataContract"});
            table2.AddRow(new string[] {
                        "BindingType",
                        "BasicHttpBinding"});
            table2.AddRow(new string[] {
                        "SecurityMode",
                        "None"});
            table2.AddRow(new string[] {
                        "ClientCredentialType",
                        "None"});
#line 17
 testRunner.And("I rehost services using generated wsdl assembly", ((string)(null)), table2, "And ");
#line 23
 testRunner.And("download metadata from rehost metadata url to folder \'wsdl-gen\\datacontract\\order" +
                    "s\\svcutil\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
 testRunner.And("call service using channel factory and the same contract interface", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
 testRunner.Then("I should be able to hit service impl class and return dummy object", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Rehost WSDL with Xml Serialization and Svcutil")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "HostWsdlService")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("wsdl")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("rehost")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("xml")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("svcutil")]
        public virtual void RehostWSDLWithXmlSerializationAndSvcutil()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rehost WSDL with Xml Serialization and Svcutil", new string[] {
                        "wsdl",
                        "rehost",
                        "xml",
                        "svcutil"});
#line 28
this.ScenarioSetup(scenarioInfo);
#line 29
 testRunner.Given("a referenced assembly in bin folder called \'Interop.Contracts.dll\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 30
 testRunner.And("set serialization method to \'Xml\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
 testRunner.And("Pick an interface with name \'IRegistrationManager\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
 testRunner.And("set generated assembly namespace to \"AAMVA.Registration\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
 testRunner.And("set rehost namespace to \'AAMVA.Registration.Services\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table3.AddRow(new string[] {
                        "ServiceAddress",
                        "http://localhost:54321/WsdlPublish_Xml"});
            table3.AddRow(new string[] {
                        "BindingType",
                        "BasicHttpBinding"});
            table3.AddRow(new string[] {
                        "SecurityMode",
                        "None"});
            table3.AddRow(new string[] {
                        "ClientCredentialType",
                        "None"});
#line 34
 testRunner.When("I generate and host service with the following setting", ((string)(null)), table3, "When ");
#line 40
 testRunner.And("I generate wsdl assembly and save them to folder \'wsdl-generated\' with name \'Regi" +
                    "strationManager.wsdl.dll\' using SvcUtil", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table4.AddRow(new string[] {
                        "ServiceAddress",
                        "http://localhost:54321/WsdlHost_Xml"});
            table4.AddRow(new string[] {
                        "BindingType",
                        "BasicHttpBinding"});
            table4.AddRow(new string[] {
                        "SecurityMode",
                        "None"});
            table4.AddRow(new string[] {
                        "ClientCredentialType",
                        "None"});
#line 41
 testRunner.And("I rehost services using generated wsdl assembly", ((string)(null)), table4, "And ");
#line 47
 testRunner.And("download metadata from rehost metadata url to folder \'wsdl-gen\\xml\\register\\svcut" +
                    "il\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
 testRunner.And("call service using channel factory and the same contract interface", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
 testRunner.Then("I should be able to hit service impl class and return dummy object", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Rehost WSDL with Data Contract and Metadataset")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "HostWsdlService")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("wsdl")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("rehost")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("data_contract")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("metadata_set")]
        public virtual void RehostWSDLWithDataContractAndMetadataset()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rehost WSDL with Data Contract and Metadataset", new string[] {
                        "wsdl",
                        "rehost",
                        "data_contract",
                        "metadata_set"});
#line 52
this.ScenarioSetup(scenarioInfo);
#line 54
 testRunner.Given("a referenced assembly in bin folder called \'Interop.Contracts.dll\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 55
 testRunner.And("Pick an interface with name \'IOrderManager\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
 testRunner.And("set serialization method to \'DataContract\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
 testRunner.And("set generated assembly namespace to \"AAMVA.Orders\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
 testRunner.And("set rehost namespace to \'AAMVA.Orders.Services\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table5.AddRow(new string[] {
                        "ServiceAddress",
                        "http://localhost:54321/WsdlPublish_DataContract"});
            table5.AddRow(new string[] {
                        "BindingType",
                        "BasicHttpBinding"});
            table5.AddRow(new string[] {
                        "SecurityMode",
                        "None"});
            table5.AddRow(new string[] {
                        "ClientCredentialType",
                        "None"});
#line 59
 testRunner.When("I generate and host service with the following setting", ((string)(null)), table5, "When ");
#line 65
 testRunner.And("I generate wsdl assembly using method MetadataSet", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table6.AddRow(new string[] {
                        "ServiceAddress",
                        "http://localhost:54321/WsdlHost_DataContract"});
            table6.AddRow(new string[] {
                        "BindingType",
                        "BasicHttpBinding"});
            table6.AddRow(new string[] {
                        "SecurityMode",
                        "None"});
            table6.AddRow(new string[] {
                        "ClientCredentialType",
                        "None"});
#line 66
 testRunner.And("I rehost services using generated wsdl assembly", ((string)(null)), table6, "And ");
#line 72
 testRunner.And("download metadata from rehost metadata url to folder \'wsdl-gen\\datacontract\\order" +
                    "s\\metadataset\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
 testRunner.And("call service using channel factory and the same contract interface", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
 testRunner.Then("I should be able to hit service impl class and return dummy object", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Rehost WSDL with Xml and Metadataset")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "HostWsdlService")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("wsdl")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("rehost")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("xml")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("metadata_set")]
        public virtual void RehostWSDLWithXmlAndMetadataset()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Rehost WSDL with Xml and Metadataset", new string[] {
                        "wsdl",
                        "rehost",
                        "xml",
                        "metadata_set"});
#line 77
this.ScenarioSetup(scenarioInfo);
#line 78
 testRunner.Given("a referenced assembly in bin folder called \'Interop.Contracts.dll\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 79
 testRunner.And("set serialization method to \'Xml\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
 testRunner.And("Pick an interface with name \'IRegistrationManager\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
 testRunner.And("set generated assembly namespace to \"AAMVA.Registration\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 82
 testRunner.And("set rehost namespace to \'AAMVA.Registration.Services\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table7.AddRow(new string[] {
                        "ServiceAddress",
                        "http://localhost:54321/WsdlPublish_Xml"});
            table7.AddRow(new string[] {
                        "BindingType",
                        "BasicHttpBinding"});
            table7.AddRow(new string[] {
                        "SecurityMode",
                        "None"});
            table7.AddRow(new string[] {
                        "ClientCredentialType",
                        "None"});
#line 83
 testRunner.When("I generate and host service with the following setting", ((string)(null)), table7, "When ");
#line 89
 testRunner.And("I generate wsdl assembly using method MetadataSet", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table8.AddRow(new string[] {
                        "ServiceAddress",
                        "http://localhost:54321/WsdlHost_Xml"});
            table8.AddRow(new string[] {
                        "BindingType",
                        "BasicHttpBinding"});
            table8.AddRow(new string[] {
                        "SecurityMode",
                        "None"});
            table8.AddRow(new string[] {
                        "ClientCredentialType",
                        "None"});
#line 90
 testRunner.And("I rehost services using generated wsdl assembly", ((string)(null)), table8, "And ");
#line 96
 testRunner.And("download metadata from rehost metadata url to folder \'wsdl-gen\\datacontract\\regis" +
                    "ter\\metadataset\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
 testRunner.And("call service using channel factory and the same contract interface", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
 testRunner.Then("I should be able to hit service impl class and return dummy object", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
