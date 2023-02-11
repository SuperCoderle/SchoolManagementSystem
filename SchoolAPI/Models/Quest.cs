namespace SchoolAPI.Models
{
	public class Quest
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
