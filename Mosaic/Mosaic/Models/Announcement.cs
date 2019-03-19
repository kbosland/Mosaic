using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mosaic.Models
{
    public class Announcement
    {
        public int ID { get; set; }
        public string AnnouncementText { get; set; }
        public string ClassCode { get; set; }
        public string ProfUsername { get; set; }
    }
}
