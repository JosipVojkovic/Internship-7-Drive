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

        public static bool MapCommands<TEnum>(Dictionary<TEnum, (string, string)> dictionary) where TEnum : Enum
        {
            string? goBack = "";

            foreach (var item in dictionary)
            {
                Console.WriteLine($"  {item.Value.Item1}:");
                Console.WriteLine($"    - {item.Value.Item2}");
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

        public static bool ConfirmDialog()
        {
            var decision = "";
            Console.Clear();

            while(decision != "y" && decision != "n")
            {
                Console.Write("Jeste li sigurni? (y/n): ");
                decision = Console.ReadLine();

                if (decision != "y" && decision != "n")
                {
                    Console.Clear();
                    Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                }   
            }

            Console.Clear();
            return decision == "y";    
        }

        public static (TEnum, string) MultiLineInput<TEnum>(Dictionary<TEnum, (string, string)> commands, string fileName) where TEnum : Enum
        {
            List<string> lines = new List<string>();
            StringBuilder currentLine = new StringBuilder();

            int currentLineIndex = 0;

            while(true)
            {
                Console.Clear();
                Console.WriteLine($"Unesite novi sadrzaj datoteke {fileName}:\n");

                foreach(var line in lines)
                {
                    Console.WriteLine(line);
                }

                Console.Write(currentLine.ToString());
                var keyInfo = Console.ReadKey(intercept: true);

                if(commands.Values.Any(t => t.Item1 == currentLine.ToString()))
                {
                    Console.Clear();
                    var command = commands.FirstOrDefault(kvp => kvp.Value.Item1 == currentLine.ToString()).Key;
                    return (command, string.Join("\n", lines));
                }
                if(keyInfo.Key == ConsoleKey.Enter)
                {
                    lines.Add(currentLine.ToString());
                    currentLine.Clear();
                    currentLineIndex++;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if(currentLine.Length == 0 && currentLineIndex > 0)
                    {
                        currentLine = new StringBuilder(lines[currentLineIndex - 1]);
                        lines.RemoveAt(currentLineIndex - 1);
                        currentLineIndex--;
                    }
                    else if (currentLine.Length > 0)
                    {
                        currentLine.Remove(currentLine.Length - 1, 1);
                    }
                }
                else
                {
                    if (keyInfo.KeyChar != 0)
                    {
                        currentLine.Append(keyInfo.KeyChar);
                    }
                }
            }
        }
    }
}
