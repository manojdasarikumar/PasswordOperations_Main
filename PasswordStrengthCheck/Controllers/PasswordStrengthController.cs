using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PasswordStrengthCheck.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PasswordStrengthCheck.Extensions;

namespace PasswordStrengthCheck.Controllers
{
    /// <summary>
    ///  An API which is used to check the strength and pwned status of a password
    /// </summary>
    [Route("api/PasswordStrengthController")]
    [ApiController]
    public class PasswordStrengthController : ControllerBase
    {
        public IPasswordStrengthService PasswordStrengthService { get; }

        public PasswordStrengthController(IPasswordStrengthService passwordStrengthService)
        {
            this.PasswordStrengthService = passwordStrengthService;
        }

        // GET api/<PasswordStrengthCalculatorController>/5
        /// <summary>Gets the password strength.</summary>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   Return Password Strength in string format
        /// </returns>
        [HttpGet()]
        [Route("GetPasswordStrength")]
        public string GetPasswordStrength(string password)
        {
            return this.PasswordStrengthService.GetPasswordStrength(password);
        }

        /// <summary>
        /// Gets the pwned count.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        /// Returns Pwned Count for a password
        /// </returns>
        [HttpGet()]
        [Route("GetPwnedCount")]
        public async Task<int> GetPwnedCount(string password)
        {
            return await this.PasswordStrengthService.GetPwnedCountForPassword(password);
        }

    }
}
