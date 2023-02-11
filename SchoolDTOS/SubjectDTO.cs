using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDTOS
{
    public class SubjectDTO
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
