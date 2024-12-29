using DriveApp.Domain.Factories;
using DriveApp.Domain.Repositories;
using DriveApp.Presentation.Enums;
using DriveApp.Presentation.Helpers;
using DriveApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Presentation.Actions
{
    public class MyDiscActions
    {
        private readonly UserRepository _userRepository;
        private readonly FolderRepository _folderRepository;
        private readonly FileRepository _fileRepository;
        private readonly ItemRepository _itemRepository;

        public MyDiscActions()
        {
            _userRepository = RepositoryFactory.Create<UserRepository>();
            _folderRepository = RepositoryFactory.Create<FolderRepository>();
            _fileRepository = RepositoryFactory.Create<FileRepository>();
            _itemRepository = RepositoryFactory.Create<ItemRepository>();
        }

        public void CurrentLocation(int userId, int? parentId)
        {
            var user = _userRepository.GetById(userId);
            var folders = _folderRepository.GetFolders(userId, parentId);
            var files = _fileRepository.GetFiles(userId, parentId);
            Console.WriteLine($"{user.FirstName} {user.LastName} => MOJ DISK\n");

            Console.WriteLine("Mape:");
            foreach (var folder in folders)
            {
                Console.WriteLine("  - " + folder.Name);
            }
            Console.WriteLine("Datoteke:");
            foreach (var file in files)
            {
                Console.WriteLine("  - " + file.Name);
            }

            Console.Write("\nUnesite komandu ('pomoc' za ispis svih komandi): ");
            var command = Console.ReadLine();

            switch (command)
            {
                case "pomoc":
                    Console.Clear();
                    Help(userId, parentId, "Moj disk");
                    return;
                case "":
                    Console.Clear();
                    Console.WriteLine("Unos ne smije biti prazan. Pokusajte ponovno.\n");
                    CurrentLocation(userId, parentId);
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                    CurrentLocation(userId, parentId);
                    return;
            }

        }

        public void Help(int userId, int? parentId, string menuOption)
        {
            Console.WriteLine($"{menuOption} komande:");
            var commands = new Dictionary<MyDiscCommands, string>
            {
                { MyDiscCommands.Help, "    pomoc => ispis svih komandi" },
                { MyDiscCommands.CreateFolder, "    stvori mapu 'ime mape' => stvaranje mape na trenutnoj lokaciji" },
                { MyDiscCommands.CreateFile, "    stvori datoteku 'ime datoteke' => " + 
                                               "stvaranje datoteke na trenutnoj lokaciji" },
                { MyDiscCommands.EnterFolder, "    udi u mapu 'ime mape' => ulazak u mapu" },
                { MyDiscCommands.EditFile, "    uredi datoteku 'ime datoteke' => uredivanje datoteke" },
                { MyDiscCommands.DeleteFolder, "    izbrisi mapu 'ime mape' => brisanje mape" },
                { MyDiscCommands.DeleteFile, "    izbrisi datoteku 'ime datoteke' => brisanje datoteke" },
                { MyDiscCommands.ChangeFolderName, "    promjeni naziv mape 'ime mape' u 'novo ime mape' => " +
                                                    "mijenjanje naziva mape" },
                { MyDiscCommands.ChangeFileName, "    promjeni naziv datoteke 'ime datoteke' u 'novo ime datoteke' => " +
                                                    "mijenjanje naziva datoteke" },
                { MyDiscCommands.Back, "    povratak => povratak na prijasnji korak" }
            };

            var isValid = EnumMapper.MapCommands<MyDiscCommands>(commands);
            Console.Clear();

            if (!isValid)
            {
                
                Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                Help(userId, parentId, menuOption);
                return;
            }
            CurrentLocation(userId, parentId);
            return;
        }

        public void CreateMap(int userId, int parentId, string command)
        {
            string prefix = "stvori mapu ";
            string folderName = command.Substring(prefix.Length);

            var resposne = _folderRepository.Add(folderName, parentId, userId);
            Console.Clear();

            if (resposne == ResponseResultType.AlreadyExists)
            {
                Console.WriteLine("Greska, mapa s istim imenom već postoji u trenutnom direktoriju.\n");
            }
            else
            {
                Console.WriteLine($"Mapa {folderName} uspjesno kreirana.\n");
            }

            CurrentLocation(userId, parentId);
            return;
        }
    }
}
