using PasswordStrengthCheck.Extensions;
using PasswordStrengthCheck.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PasswordStrengthCheck.Services
{
    public class PasswordStrengthService : IPasswordStrengthService
    {
        public IHttpClientFactory ClientFactory { get; }

        public PasswordStrengthService(IHttpClientFactory _clientFactory)
        {
            ClientFactory = _clientFactory;
        }

        public string  GetPasswordStrength(string password)
        {
            int score = this.CalculatePassWordStrength(password);
            return ((PasswordStrengthEnum)score).ToString();
        }

        public async Task<int> GetPwnedCountForPassword(string password)
        {
            var pwnedCount = await this.GetPwnedCount(password);
            return pwnedCount;
        }

        private int CalculatePassWordStrength(string password)
        {
            int score = 0;
            
            if (!string.IsNullOrWhiteSpace(password))
            {
                if (HasMinimumLength(password, 1))
                {
                    score++;
                }

                if (HasMinimumLength(password, 8))
                {
                    score++;
                }

                if (HasUpperCaseLetter(password) && HasLowerCaseLetter(password))
                {
                    score++;
                }

                if (HasDigit(password))
                {
                    score++;
                }

                if (HasSpecialChar(password))
                {
                    score++;
                }
            }


            return score;
        }

        private async Task<int> GetPwnedCount(string password)
        {
            var pwnedCount = 0;
            var encryptedPassword = password.SHA1Encryption();            
            var shortEncryptedPassword = encryptedPassword.Substring(0, 5);
            var restPassword = encryptedPassword.Substring(5);

            var resultContent = await this.CreateRequest(shortEncryptedPassword);
            var result = this.GetResultDictionary(resultContent);

            if (result.Any() && result.ContainsKey(restPassword))
            {
                pwnedCount = result[restPassword];
            }

            return pwnedCount;

        }


        private async Task<string> CreateRequest(string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("https://api.pwnedpasswords.com/range/" + password));
            var result = await SendAsync(request);
            return result;
        }

        private async Task<string> SendAsync(HttpRequestMessage request)
        {
            using (var client = this.ClientFactory.CreateClient())
            {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
            }
        }

        private IDictionary<string, int> GetResultDictionary(string content)
        {
            var dictionary = new Dictionary<string, int>();
            var replacedString = content.Trim().Replace("\r\n", ";");


            replacedString.Split(";").ToList().ForEach(line =>
            {
                var items = line.Replace("\r\n", "").Split(":");
                dictionary.Add(items[0], Convert.ToInt32(items[1]));
            });

            return dictionary;
        }


        #region Helper Functions

        private bool HasMinimumLength(string password, int minLength)
        {
            return password.Length >= minLength;
        }

        private bool HasDigit(string password)
        {
            return password.Any(c => char.IsDigit(c));
        }

        private bool HasSpecialChar(string password)
        {
            return password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1;
        }

        private bool HasUpperCaseLetter(string password)
        {
            return password.Any(c => char.IsUpper(c));
        }
        private bool HasLowerCaseLetter(string password)
        {
            return password.Any(c => char.IsLower(c));
        }

        #endregion
    }
}
