using Mosaic.Models;
using System.Collections.Generic;

namespace Mosaic.Services
{
    public interface IProfAuthentication
    {
        bool AllowLogin(string username, string password);
        string EncryptPassword(string password);
        Professor VerifyChangePassword(string username, string oldPass, string newPass);
        List<string> ReturnAllUsernames();

    }
}
