using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordStrengthCheck.Services
{
    public interface IPasswordStrengthService
    {
        string GetPasswordStrength(string password);
        Task<int> GetPwnedCountForPassword(string password);
    }
}
