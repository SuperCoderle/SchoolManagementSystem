namespace SchoolManagement.Data
{
    public class BookData
    {
        public string Title { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime BookLoanDay { get; set; }
        public DateTime BookReturnDay { get; set; }
        public string StudentName { get; set; }
        public string Photo { get; set; }
        public string Path { get; set; }
        public string RandomColor { get; set; }
    }
}
