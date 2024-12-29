using DriveApp.Domain.Enums;
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
            var menuOptions = new Dictionary<StartMenu, string>
            {
                { StartMenu.Login, "Prijava" },
                { StartMenu.Registration, "Registracija" },
                { StartMenu.Exit, "Izlaz" },
            };
            var decision = EnumMapper.MapMenuOptions<StartMenu>(menuOptions);

            switch (decision) 
            {
                case StartMenu.Login:
                    Login();
                    return;
                case StartMenu.Registration:
                    Register();
                    return;
                case StartMenu.Exit:
                    Console.Clear();
                    Console.WriteLine("Hvala vam na koristenju Drive aplikacije!");
                    return;
            }
        }

        public void Login()
        {
            var email = InputValidator
                .GetValidInput(InputValidator.IsNotEmptyValidation, "PRIJAVA\n\nUnesite svoj email ili 0 za natrag: ");

            if (email == "0")
            {
                Console.Clear();
                MainMenu();
                return;
            }

            Console.Clear();
            var password = InputValidator
                .GetValidInput(InputValidator.IsNotEmptyValidation, $"PRIJAVA\n\nEmail: {email}\nUnesite svoju lozinku: ");

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

            Console.Clear();
            UserMenu(user.Id);
            return;
        }

        public void Register()
        {
            var email = InputValidator
                .GetValidInput(InputValidator.EmailValidation, "REGISTRACIJA\n\nEmail: ");
            Console.Clear();

            var password = InputValidator
                .GetValidInput(InputValidator.PasswordValidation,$"REGISTRACIJA\n\nLozinka (> 6 znakova): ");
            Console.Clear();

            string? confirmPassword;
            do
            {
                Console.Write($"REGISTRACIJA\n\nPonovi lozinku: ");

                confirmPassword = Console.ReadLine();
                if (confirmPassword != password)
                {
                    Console.Clear();
                    Console.WriteLine("Lozinke se ne podudaraju. Pokusajte ponovno.\n");
                }
            }
            while (confirmPassword != password);

            Console.Clear();
            var firstName = InputValidator
                .GetValidInput(InputValidator.IsNotEmptyValidation,$"REGISTRACIJA\n\nIme: ");

            Console.Clear();
            var lastName = InputValidator
                .GetValidInput(InputValidator.IsNotEmptyValidation,$"REGISTRACIJA\n\nPrezime: ");

            var captcha = EnumMapper.GenerateCaptcha();
            Console.Clear();

            string? enteredCaptcha;
            do
            {
                Console.WriteLine($"REGISTRACIJA\n\nKodna rijec: {captcha}\n");
                Console.Write("Kopirajte kodnu rijec kako bismo znali da niste bot ili 0 za natrag: ");

                enteredCaptcha = Console.ReadLine();
                if (enteredCaptcha != captcha)
                {
                    captcha = EnumMapper.GenerateCaptcha();
                    Console.Clear();
                    Console.WriteLine("Kodne rijeci se ne podudaraju. Pokusajte ponovno.\n");
                }
            }
            while (enteredCaptcha != captcha && enteredCaptcha != "0");
            Console.Clear();

            if(enteredCaptcha == captcha)
            {
                var response = _userRepository.Add(email, password, firstName, lastName);
                if (response == ResponseResultType.AlreadyExists)
                    Console.WriteLine("Registracija neuspjesna. Korisnik sa tim emailom vec postoji.\n");
                else
                    Console.WriteLine("Korisnik uspjesno registriran.\n");
            }

            MainMenu();
            return;
        }

        public void UserMenu(int userId)
        {
            var user = _userRepository.GetById(userId);
            Console.WriteLine($"Dobrodosli {user?.FirstName} {user?.LastName}!\n");

            var menuOptions = new Dictionary<UserMenu, string>
            {
                { Enums.UserMenu.MyDisc, "Moj disk" },
                { Enums.UserMenu.SharedItems, "Dijeljeno sa mnom" },
                { Enums.UserMenu.ProfileSettings, "Postavke profila" },
                { Enums.UserMenu.Logout, "Odjava iz profila" }
            };

            var decision = EnumMapper.MapMenuOptions<UserMenu>(menuOptions);
            var myDiscActions = new MyDiscActions();

            switch (decision)
            {
                case Enums.UserMenu.MyDisc:
                    myDiscActions.CurrentLocation(user.Id, null);
                    return;
                case Enums.UserMenu.SharedItems:
                    //SharedItems.MainMenu();
                    return;
                case Enums.UserMenu.ProfileSettings:
                    //ProfileSettings.MainMenu();
                    return;
                case Enums.UserMenu.Logout:
                    MainMenu();
                    return;
                    
            }
        }
    }
}
