namespace SchoolAPI.Models
{
    public class Report
    {
        public int ID { get; set; }
        public string Classroom { get; set; }
        public string SubjectName { get; set; }
        public string Letter { get; set; }
        public int Point { get; set; }
        public int StudentID { get; set; }
    }
}
