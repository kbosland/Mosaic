using System;
using System.Collections.Generic;

namespace Mosaic.Models
{
    public partial class Email
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
