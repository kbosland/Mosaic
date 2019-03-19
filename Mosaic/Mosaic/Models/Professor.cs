using System;
using System.Collections.Generic;

namespace Mosaic.Models
{
    public partial class Professor
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClassOne { get; set; }

        public Class ClassOneNavigation { get; set; }
    }
}
