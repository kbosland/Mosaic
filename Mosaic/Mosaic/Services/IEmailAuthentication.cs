using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mosaic.Services
{
    public interface IEmailAuthentication
    {
        bool AllowLogin(string username, string password, int type);
        string EncryptPassword(string password);
    }
}
