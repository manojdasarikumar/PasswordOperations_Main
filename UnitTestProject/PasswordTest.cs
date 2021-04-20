using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using PasswordStrengthCheck.Controllers;
using PasswordStrengthCheck.Services;
using Moq;

namespace UnitTestProject
{
    [TestClass]
    public class PasswordTest
    {
        [TestMethod]
        public void TestVeryWeakPassword()
        {
            Assert.AreEqual("VeryWeak", GetPasswordStrength("abc"));
        }

        [TestMethod]
        public void TestWeakPassword()
        {
            Assert.AreEqual("Weak", GetPasswordStrength("Abcdef"));
        }

        [TestMethod]
        public void TestMediumPassword()
        {
            Assert.AreEqual("Medium", GetPasswordStrength("abcdef@ac"));
        }

        [TestMethod]
        public void TestStrongPassword()
        {
            Assert.AreEqual("Strong", GetPasswordStrength("ABcdef@ac"));
        }

        [TestMethod]
        public void TestVeryStrongPassword()
        {
            Assert.AreEqual("VeryStrong", GetPasswordStrength("Abcdef@123"));
        }

        [TestMethod]
        public void TestPwnedPassword()
        {
            int pwnedCount = IsPawnedPassword("abc");
            Assert.IsTrue(pwnedCount > 0);
        }

        private static string GetPasswordStrength(string password)
        {
            var client = new Mock<IHttpClientFactory>();
            var mockRepo = new PasswordStrengthService(client.Object);
            var psc = new PasswordStrengthController(mockRepo);
            return psc.GetPasswordStrength(password);
        }

        private static int IsPawnedPassword(string password)
        {
            var client = new Mock<IHttpClientFactory>();
            var mockRepo = new PasswordStrengthService(client.Object);
            var psc = new PasswordStrengthController(mockRepo);
            return psc.GetPwnedCount(password).Result;
        }

        



    }
}
