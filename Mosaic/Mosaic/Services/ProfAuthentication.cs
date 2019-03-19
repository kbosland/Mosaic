using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mosaic.Models;
using System.Security.Cryptography;
using System.Text;

namespace Mosaic.Services
{
    public class ProfAuthentication : IProfAuthentication
    {
        private readonly Models.MosaicContext _context;

        public ProfAuthentication (Models.MosaicContext context)
        {
            _context = context;
        }

        public List<string> ReturnAllUsernames()
        {
            List<string> usernames = new List<string>();
            List<Student> students = _context.Student.ToList();
            foreach (Student s in students)
            {
                usernames.Add(s.Username);
            }
            List<Professor> profs = _context.Professor.ToList();
            foreach (Professor p in profs)
            {
                usernames.Add(p.Username);
            }

            return usernames;
        }
        public bool AllowLogin(string username, string password)
        {
            var prof = _context.Professor.SingleOrDefault(m => m.Username == username);
            if (prof != null)
            {
                if (prof.Password.Equals(EncryptPassword(password)))
                {
                    return true;
                }
            }

            return true;
        }

        public string EncryptPassword(string password)
        {
            string encrypted = "";
            using (SHA512 crypto = new SHA512Managed())
            {
                byte[] passwordInBytes = Encoding.ASCII.GetBytes(password);
                byte[] hash = crypto.ComputeHash(passwordInBytes);
                encrypted = Encoding.ASCII.GetString(hash);
            }
            return encrypted;
        }
        public Professor VerifyChangePassword(string username, string oldPass, string newPass)
        {
            var prof = _context.Professor.SingleOrDefault(m => m.Username == username);
            if (prof.Password.Equals(EncryptPassword(oldPass)))
            {
                prof.Password = this.EncryptPassword(newPass);
                return prof;
            }
            return null;
        }
    }
}
