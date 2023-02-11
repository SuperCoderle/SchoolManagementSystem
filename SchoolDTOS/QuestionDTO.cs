using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDTOS
{
	public class QuestionDTO
	{
		public int ID { get; set; }
		public string Question { get; set; }
		public string Type { get; set; }
		public string RightAnswer { get; set; }
		public string WrongAnswer1 { get; set; }
		public string WrongAnswer2 { get; set; }
		public string WrongAnswer3 { get; set; }
	}
}
