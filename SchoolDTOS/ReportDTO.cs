using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDTOS
{
    public class ReportDTO
    {
        public int ID { get; set; }
        public string Classroom { get; set; }
        public string SubjectName { get; set; }
        public string Letter { get; set; }
        public int Point { get; set; }
        public int StudentID { get; set; }
    }
}
