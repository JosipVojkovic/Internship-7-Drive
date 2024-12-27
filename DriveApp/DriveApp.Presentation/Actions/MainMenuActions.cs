using DriveApp.Domain.Factories;
using DriveApp.Domain.Repositories;
using DriveApp.Presentation.Enums;
using DriveApp.Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Presentation.Actions
{
    public class MainMenuActions
    {
        private readonly UserRepository _userRepository;

        public MainMenuActions()
        {
            _userRepository = RepositoryFactory.Create<UserRepository>();
        }

        public void MainMenu()
        {
            var decision = EnumMapper.MapEnum<StartMenu>();

            switch (decision) 
            {
                case StartMenu.Prijava:
                    Login();
                    return;
                case StartMenu.Registracija:
                    Register();
                    return;
                case StartMenu.Izlaz:
                    // ExitApp();
                    return;
            }
        }

        public void Login()
        {
            Console.WriteLine("PRIJAVA\n");
            Console.Write("Unesite svoj email ili 0 za natrag: ");
            var email = Console.ReadLine();

            if (email == "0")
            {
                Console.Clear();
                MainMenu();
                return;
            }

            Console.Write("Unesite svoju lozinku: ");
            var password = Console.ReadLine();

            var user = _userRepository.GetUser(email, password);

            if (user is null)
            {
                for (int i = 30; i >= 0; i--)
                {
                    Console.Clear();
                    Console.WriteLine("Ne postoji korisnik sa ovim podatcima.\n");
                    Console.WriteLine($"Molimo vas, pricekajte {i}s da provjerimo da niste bot...");
                    Task.Delay(1000).Wait();
                }
                Console.Clear();
                Console.WriteLine("Pokusajte ponovno.\n");
                Login();
                return;
            }

        }

        public void Register()
        {
            Console.WriteLine("REGISTRACIJA\n");
            Console.Write("Unesite svoj email: ");
            var email = Console.ReadLine();

            Console.Write("Unesite svoju lozinku: ");
            var password = Console.ReadLine();

            Console.Write("Ponovno upisite lozinku: ");
            var confirmPassword = Console.ReadLine();

            var captcha = EnumMapper.GenerateCaptcha();
            Console.WriteLine($"\nKodna rijec: {captcha}\n");
            Console.Write("Kopirajte kodnu rijec kako bismo znali da niste bot ili 0 za natrag: ");
            var enteredCaptcha = Console.ReadLine();

            if(enteredCaptcha == "0")
            {
                Console.Clear();
                MainMenu();
                return;
            }
            else if(enteredCaptcha != captcha)
            {
                Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
            }
        }
    }
}
