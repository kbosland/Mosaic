using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mosaic.Controllers;
using Mosaic.Models;

namespace Mosaic.Repository
{
    public class StudentRepository
    {
        MosaicContext _context;

        public StudentRepository (MosaicContext mosaicContext)
        {
            _context = mosaicContext;
        }

        public void AddStudent (Student s)
        {
            _context.Student.Add(s);
            _context.SaveChanges();
        }
    }
}
