using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interop.Contracts.Lessons
{
	[Serializable]
	public class ClassFullError
	{
		public Course Course { get; set; }

		public ClassFullError(Course course)
		{
			this.Course = course;
		}
	}

	[Serializable]
	public class ClassNotFoundError
	{
		public Course Course { get; set; }

		public ClassNotFoundError(Course course)
		{
			this.Course = course;
		}
	}

	[Serializable]
	public class StudentAlreadyRegisteredForThisClassError
	{
		public Course Course { get; set; }
		public Student Student { get; set; }

		public StudentAlreadyRegisteredForThisClassError(Course course, Student student)
		{
			this.Course = course;
			this.Student = student;
		}
	}
}
