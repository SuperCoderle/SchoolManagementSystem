namespace SchoolAPI.Models
{
	public class Exam
	{
		public int ExamID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
		public int Score { get; set; }
		public string TimeStart { get; set; }
		public string TimeFinish { get; set; }
		public int NumberQuest { get; set; }
		public int Coef { get; set; }
	}
}
