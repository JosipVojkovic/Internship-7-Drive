using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DriveApp.Presentation.Helpers
{
    public static class InputValidator
    {
        public static bool EmailValidation(string email)
        {
            var emailPattern = @"^[^@]+@[a-zA-Z0-9.-]{2,}\.[a-zA-Z]{3,}$";

            return Regex.IsMatch(email, emailPattern);
        }
    }
}
