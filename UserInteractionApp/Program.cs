using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UserInteractionApp
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        
        static void Main(string[] args)
        {
            Console.Write("Enter Password:");
            string password = Console.ReadLine();
            var passwordStrength = GetPasswordStrength(password).Result;
            Console.WriteLine(passwordStrength);
            if(password.Length>0)
            {
                var pwnPasswordResult = GetPwnedPasswordCount(password).Result;
                int pwnPasswordCount = Int32.Parse(pwnPasswordResult);
                if (pwnPasswordCount > 0)
                {
                    Console.WriteLine("This password has previously appeared in " + pwnPasswordCount + " data breaches and should never be used. If you've ever used it anywhere before, change it!");
                }
                else
                {
                    Console.WriteLine("This password is safe to use!");
                }
            }
        }

        static async Task<string> GetPasswordStrength(string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("https://localhost:44379/api/PasswordStrengthController/GetPasswordStrength?password=" + password));
            var result = await SendAsync(request);
            return result;
        }

        static async Task<string> GetPwnedPasswordCount(string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("https://localhost:44379/api/PasswordStrengthController/GetPwnedCount?password=" + password));
            var result = await SendAsync(request);
            return result;
        }

        private static async Task<string> SendAsync(HttpRequestMessage request)
        {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
        }
    }
}
