using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mosaic.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Mosaic.Services
{
    public class EmailAuthentication : IEmailAuthentication
    {
        private readonly MosaicContext _context;

        public EmailAuthentication (MosaicContext context)
        {
            _context = context;
        }

        public bool AllowLogin(string username, string password, int type)
        {
 
            if (type == 0)
            {
                var student = _context.Student.SingleOrDefault(m => m.Username == username);
                if (student != null)
                {

                }
            } else if (type == 1)
            {
                var professor = _context.Professor.SingleOrDefault(m => m.Username == username);
                if (professor != null)
                {

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
    }
}
