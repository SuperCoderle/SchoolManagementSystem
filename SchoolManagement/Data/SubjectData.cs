namespace SchoolManagement.Data
{
    public class SubjectData
    {
        public string Title { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string SchoolDay { get; set; }
        public string Teacher { get; set; }
        public string Classroom { get; set; }
    }
}
