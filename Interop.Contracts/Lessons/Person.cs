using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interop.BaseTypes;

namespace Interop.Contracts.Lessons
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("TextTransform", "12.0.21005.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://aamva.org/xsd/nmvtis/exchange/pvhi/3.1")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://aamva.org/xsd/nmvtis/exchange/pvhi/3.1", IsNullable = true)]
	public class Person
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public Tracking Tracking { get; set; }
	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("TextTransform", "12.0.21005.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://aamva.org/xsd/nmvtis/exchange/pvhi/3.1")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://aamva.org/xsd/nmvtis/exchange/pvhi/3.1", IsNullable = true)]
	public class Instructor : Person
	{
		public string Office { get; set; }
	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("TextTransform", "12.0.21005.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://aamva.org/xsd/nmvtis/exchange/pvhi/3.1")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://aamva.org/xsd/nmvtis/exchange/pvhi/3.1", IsNullable = true)]
	public class Student : Person
	{
		public int StudentId { get; set; }
	}
}
