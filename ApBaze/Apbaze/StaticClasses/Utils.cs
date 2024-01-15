using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Apbaze.StaticClasses
{
    public static class Utils
    {
        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public static bool IsValidPhoneNumber(string phoneString)
        {
            string pattern = @"^(\+\d{1,2}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
            return Regex.IsMatch(phoneString, pattern);
        }

        public static bool IsValidEmail(string email)
        {
            bool isValid = false;

            try
            {
                MailAddress address = new MailAddress(email);
                isValid = (address.Address == email);
                return isValid = true;
                // or
                // isValid = string.IsNullOrEmpty(address.DisplayName);
            }
            catch (FormatException)
            {
                return isValid = false;
            }
        }
    }
}
