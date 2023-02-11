namespace SchoolManagement.Data
{
	public class ExamData
	{
		public string Modal_Title { get; set; }
		public int ExamID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
		public int Score { get; set; }
		public DateTime TimeStart { get; set; }
		public DateTime TimeFinish { get; set; }
		public int NumberQuest { get; set; }
		public int Coef { get; set; }
	}
}
