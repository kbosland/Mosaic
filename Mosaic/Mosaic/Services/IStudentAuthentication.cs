using Mosaic.Models;
using System.Collections.Generic;

namespace Mosaic.Services
{
    public interface IStudentAuthentication
    {
        bool AllowLogin(string username, string password);
        string EncryptPassword(string password);
        Student VerifyChangePassword(string username, string oldPass, string newPass);
        List<string> ReturnAllUsernames();

    }
}
