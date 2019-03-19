using System;
using System.Collections.Generic;

namespace Mosaic.Models
{
    public partial class Class
    {
        public Class()
        {
            Professor = new HashSet<Professor>();
            StudentClassOneNavigation = new HashSet<Student>();
            StudentClassTwoNavigation = new HashSet<Student>();
        }

        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public int NumEnrolled { get; set; }
        public int MaxEnroll { get; set; }
        public string ProfessorId { get; set; }

        public ICollection<Professor> Professor { get; set; }
        public ICollection<Student> StudentClassOneNavigation { get; set; }
        public ICollection<Student> StudentClassTwoNavigation { get; set; }
    }
}
