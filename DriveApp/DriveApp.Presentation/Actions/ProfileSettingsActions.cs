using DriveApp.Domain.Enums;
using DriveApp.Domain.Factories;
using DriveApp.Domain.Repositories;
using DriveApp.Presentation.Enums;
using DriveApp.Presentation.Helpers;

namespace DriveApp.Presentation.Actions
{
    public class ProfileSettingsActions
    {
        private readonly UserRepository _userRepository;

        public ProfileSettingsActions()
        {
            _userRepository = RepositoryFactory.Create<UserRepository>();
        }

        public void MainMenu(int userId)
        {
            var user = _userRepository.GetById(userId);
            Console.WriteLine($"{user.FirstName} {user.LastName} => POSTAVKE PROFILA\n");

            var menuOptions = new Dictionary<ProfileSettingsMenu, string>
            {
                { ProfileSettingsMenu.ChangePassword, "Promjena lozinke" },
                { ProfileSettingsMenu.ChangeEmail, "Promjena emaila" },
                { ProfileSettingsMenu.Back, "Natrag" },
            };

            var decision = EnumMapper.MapMenuOptions<ProfileSettingsMenu>(menuOptions);
            Console.Clear();

            switch (decision)
            {
                case ProfileSettingsMenu.ChangePassword:
                    ChangePassword(userId);
                    return;
                case ProfileSettingsMenu.ChangeEmail:
                    ChangeEmail(userId);
                    return;
                case ProfileSettingsMenu.Back:
                    var mainMenuActions = new MainMenuActions();
                    mainMenuActions.UserMenu(userId);
                    return;
            }
        }

        public void ChangePassword(int userId)
        {
            var newPassword = InputValidator
                .GetValidInput(InputValidator.PasswordValidation, "Nova lozinka (> 6 znakova): ");

            Console.Clear();

            var confirmPassword = InputValidator
                .GetValidInput(InputValidator.IsNotEmptyValidation, "Ponovi lozinku: ");

            var response = _userRepository.Update(userId, newPassword, confirmPassword);

            Console.Clear();
            if (response.Equals(ResponseResultType.ValidationError))
            {
                Console.WriteLine("Operacija neuspjesna. Unesene lozinke se ne podudaraju.\n");
                MainMenu(userId);
                return;
            }
            else if(response.Equals(ResponseResultType.AlreadyExists))
            {
                Console.WriteLine("Operacija neuspjesna. Nova lozinka i trenutacna lozinka ne smiju biti jednaki.\n");
                MainMenu(userId);
                return;
            }

            Console.WriteLine("Lozinka uspjesno promijenjana!\n");
            MainMenu(userId);
            return;
        }

        public void ChangeEmail(int userId)
        {
            Console.Write("Novi email ili 0 za natrag: ");
            var newEmail = Console.ReadLine();
            Console.Clear();

            if (newEmail == "0")
            {
                MainMenu(userId); 
                return;
            }
            else if (newEmail is null || !InputValidator.EmailValidation(newEmail))
            {
                ChangeEmail(userId);
                return;
            }

            var response = _userRepository.Update(userId, newEmail);

            if (response.Equals(ResponseResultType.ValidationError))
            {
                Console.WriteLine("Novi email i trenutacni email ne smiju biti jednaki. Pokusajte ponovno.\n");
                ChangeEmail(userId);
                return;
            }
            else if(response.Equals(ResponseResultType.AlreadyExists))
            {
                Console.WriteLine("Uneseni novi email je vec zauzet. Pokusajte ponovno.\n");
                ChangeEmail(userId);
                return;
            }

            Console.WriteLine("Email uspjesno promijenjen!\n");
            MainMenu(userId);
            return;
        }
    }
}
