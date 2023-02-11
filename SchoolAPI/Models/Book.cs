﻿namespace SchoolAPI.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public string BookLoanDay { get; set; }
        public string BookReturnDay { get; set; }
        public string StudentName { get; set; }
        public string Photo { get; set; }
    }
}
