using DriveApp.Presentation.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Presentation.Helpers
{
    public static class EnumMapper
    {
        public static TEnum MapMenuOptions<TEnum>(Dictionary<TEnum, string> dictionary) where TEnum : Enum
        {
            TEnum selectedOption;

            while(true)
            {
                foreach (var item in dictionary)
                {
                    Console.WriteLine($"{Convert.ToInt32(item.Key)} - {item.Value}");
                }

                Console.Write("\nOdaberite radnju: ");
                var entry = Console.ReadLine();
                Console.Clear();

                if (int.TryParse(entry, out int decision))
                {
                    if (Enum.IsDefined(typeof(TEnum), decision))
                    {
                        selectedOption = (TEnum)Enum.ToObject(typeof(TEnum), decision);
                        return selectedOption;
                    }
                    else
                    {
                        Console.WriteLine("Pogresan unos, pokusajte ponovno.\n");
                    }
                }
                else
                {
                    Console.WriteLine("Neispravan broj, pokušajte ponovno.\n");
                }
            }
            
        }

        public static bool MapCommands<TEnum>(Dictionary<TEnum, string> dictionary) where TEnum : Enum
        {
            string? goBack = "";

            foreach (var item in dictionary)
            {
                Console.WriteLine($"{item.Value}");
            }

            Console.Write("\nUnesite 0 za natrag: ");
            goBack = Console.ReadLine();
            Console.Clear();

            if (goBack != "0")
                return false;

            return true;
        }

        public static string GenerateCaptcha()
        {
            var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var digits = "0123456789";

            var random = new Random();
            var captchaLetters = new string(Enumerable.Range(0, 3)
                .Select(_ => letters[random.Next(letters.Length)]).ToArray());

            var captchaDigits = new string(Enumerable.Range(0, 3)
                .Select(_ => digits[random.Next(digits.Length)]).ToArray());

            var captcha = captchaLetters + captchaDigits;
            var shuffledCaptcha = new string(captcha.OrderBy(c => random.Next()).ToArray());

            return shuffledCaptcha;
        }
    }
}
