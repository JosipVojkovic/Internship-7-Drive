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
        public static bool IsNotEmptyValidation(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                Console.Clear();
                Console.WriteLine("Unos ne smije biti prazan. Pokusajte ponovno.\n");
                return false;
            }
            return true;
        }
        public static bool EmailValidation(string email)
        {
            if(!IsNotEmptyValidation(email))
                return false;

            var emailPattern = @"^[^@]+@[a-zA-Z0-9.-]{2,}\.[a-zA-Z]{3,}$";

            if(!Regex.IsMatch(email, emailPattern))
            {
                Console.Clear();
                Console.WriteLine("Email nije u ispravnom formatu. Pokusajte ponovno.\n");
                return false;
            }

            return true;
        }

        public static bool PasswordValidation(string password)
        {
            if(!IsNotEmptyValidation(password))
                return false;
            else if(password.Length < 7)
            {
                Console.Clear();
                Console.WriteLine("Lozinka nije dovoljno dugacka. Pokusajte ponovno.\n");
                return false;
            }

            return true;  
        }

        public static bool ConfirmPasswordValidation(string password, string confirmPassword)
        {
            if (!IsNotEmptyValidation(password))
                return false;
            else if (password != confirmPassword)
            {
                Console.Clear();
                Console.WriteLine("Lozinke se ne podudaraju. Pokusajte ponovno.");
                return false;
            }  
            return true;
        }

        public static string GetValidInput(Func<string, bool> validate, string message)
        {
            string? input;

            do
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (input is null)
                {
                    input = "";
                }
            }
            while (!validate(input));

            return input;
        }

        public static int IsNumberValidation(string input)
        {
            if (int.TryParse(input, out int result))
            {
                return result;
            }

            return -1;
        }
    }
}
