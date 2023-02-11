namespace SchoolAPI.Models
{
    public class Subject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public string SchoolDay { get; set; }
        public string Teacher { get; set; }
        public string Classroom { get; set; }
    }
}
