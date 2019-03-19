using System;
using System.Collections.Generic;

namespace Mosaic.Models
{
    public partial class Student
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClassOne { get; set; }
        public string ClassTwo { get; set; }

        public Class ClassOneNavigation { get; set; }
        public Class ClassTwoNavigation { get; set; }
    }
}
