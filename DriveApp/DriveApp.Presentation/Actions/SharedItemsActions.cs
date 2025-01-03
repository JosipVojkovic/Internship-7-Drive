using DriveApp.Data.Entities.Models;
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
    public class SharedItemsActions
    {
        private readonly UserRepository _userRepository;
        private readonly ItemRepository _itemRepository;
        private readonly FolderRepository _folderRepository;
        private readonly FileRepository _fileRepository;
        private readonly SharedItemRepository _sharedItemRepository;

        public SharedItemsActions()
        {
            _userRepository = RepositoryFactory.Create<UserRepository>();
            _itemRepository = RepositoryFactory.Create<ItemRepository>();
            _folderRepository = RepositoryFactory.Create<FolderRepository>();
            _fileRepository = RepositoryFactory.Create<FileRepository>();
            _sharedItemRepository = RepositoryFactory.Create<SharedItemRepository>();
        }

        Dictionary<SharedItemsCommands, (string, string)> Commands = new Dictionary<SharedItemsCommands, (string, string)>
        {
            {SharedItemsCommands.Help, ("pomoc", "ispis svih komandi")},
            {SharedItemsCommands.EnterFolder, ("udi u mapu 'ime mape'", "ulazak u mapu na trenutnoj lokaciji")},
            {SharedItemsCommands.EnterFile, ("udi u datoteku 'ime datoteke'", "ulazak u datoteku na trenutnoj lokaciji")},
            {SharedItemsCommands.EditFile, ("uredi datoteku 'ime datoteke'", "uredivanje datoteke na trenutnoj lokaciji")},
            {
                SharedItemsCommands.DeleteFolder,
                (
                    "izbrisi mapu 'ime mape'",
                    "brisanje mape na trenutnoj lokaciji"
                )
            },
            {
                SharedItemsCommands.DeleteFile,
                (
                    "izbrisi datoteku 'ime datoteke'",
                    "brisanje datoteke na trenutnoj lokaciji"
                )
            },
            {SharedItemsCommands.Back, ("povratak", "povratak na prijasnji korak")}
        };

        public void RootLocation(int userId)
        {
            var user = _userRepository.GetById(userId);

            _sharedItemRepository.GetRootSharedItems(userId, out var sharedFolders, out var sharedFiles);

            Console.WriteLine($"{user.FirstName} {user.LastName} => DIJELJENO SA MNOM\n");

            if (sharedFolders.Count > 0)
            {
                Console.WriteLine("Mape ([id - ime - email vlasnika]):");

                foreach (var folder in sharedFolders)
                {
                    Console.WriteLine($"  - [{folder.Id} - {folder.Name} - {_userRepository.GetById(folder.OwnerId).Email}]");
                }
            }
            else
                Console.WriteLine("Mape: nema mapa");

            if (sharedFiles.Count > 0)
            {
                Console.WriteLine("Datoteke ([id - ime - email vlasnika]):");

                foreach (var file in sharedFiles)
                {
                    Console.WriteLine($"  - [{file.Id} - {file.Name} - {_userRepository.GetById(file.OwnerId).Email}]");
                }
            }
            else
                Console.WriteLine("Datoteke: nema datoteka");


            Console.Write("\nUnesite komandu ('pomoc' za ispis svih komandi): ");
            var commandInput = Console.ReadLine();
            var command = CommandsValidator.ValidateCommand(Commands, commandInput);
            Console.Clear();

            if (command is null)
            {
                Console.WriteLine("Pogresan unos komande. Pokusajte ponovno.\n");
                RootLocation(userId);
                return;
            }

            var myDiscActions = new MyDiscActions();

            switch (command.Value.Key)
            {
                case SharedItemsCommands.Help:
                    myDiscActions.Help(Commands);
                    break;
                case SharedItemsCommands.EnterFolder:
                    var folder = sharedFolders.FirstOrDefault(f => f.Name == command.Value.Value[0]);
                    EnterFolder(folder.OwnerId, folder.ParentId, command.Value.Value[0]);
                    return;
                case SharedItemsCommands.EditFile:
                    var file = sharedFolders.FirstOrDefault(f => f.Name == command.Value.Value[0]);
                    myDiscActions.EditFile(file.OwnerId, file.ParentId, command.Value.Value[0]);
                    break;
                case SharedItemsCommands.DeleteFolder:
                    //DeleteFolder(userId, parentId, command.Value.Value[0]);
                    break;
                case SharedItemsCommands.DeleteFile:
                    //DeleteFile(userId, parentId, command.Value.Value[0]);
                    break;
                case SharedItemsCommands.Back:
                    //Back(userId, parentId);
                    return;
            }

            RootLocation(userId);
            return;
        }

        public void CurrentLocation(int userId, int? parentId)
        {
            var user = _userRepository.GetById(userId);

            var sharedFolders = _folderRepository.GetFolders(parentId);
            var sharedFiles = _fileRepository.GetFiles(parentId);

            Console.WriteLine($"{user.FirstName} {user.LastName} => DIJELJENO SA MNOM\n");

            if (sharedFolders.Count > 0)
            {
                Console.WriteLine("Mape:");

                foreach (var folder in sharedFolders)
                {
                    Console.WriteLine("  - " + folder.Name);
                }
            }
            else
                Console.WriteLine("Mape: nema mapa");

            if (sharedFiles.Count > 0)
            {
                Console.WriteLine("Datoteke:");

                foreach (var file in sharedFiles)
                {
                    Console.WriteLine("  - " + file.Name);
                }
            }
            else
                Console.WriteLine("Datoteke: nema datoteka");


            Console.Write("\nUnesite komandu ('pomoc' za ispis svih komandi): ");
            var commandInput = Console.ReadLine();
        }

        public void EnterFolder(int userId, int? parentId, string name)
        {
            var folder = _folderRepository.GetFolder(name, parentId, userId);

            if (folder is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Mapa {name} ne postoji na trenutnoj lokaciji.\n");
                CurrentLocation(userId, parentId);
            }
            else
                CurrentLocation(userId, folder.Id);

            return;
        }

        public void EnterFile(int userId, int? parentId, string name, int sharedUserId)
        {
            var file = _fileRepository.GetFile(name, parentId, userId);

            if (file is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Datoteka {name} ne postoji na trenutnoj lokaciji.\n");
                CurrentLocation(userId, parentId);
                return;
            }

            Console.WriteLine($"Ime datoteka: {file.Name}\n\nSadrzaj datoteke:\n{file.Content}\n");
            Console.Write("Unesite komandu ('pomoc' za ispis svih komandi): ");
            var commandInput = Console.ReadLine();
            var enterFileActions = new EnterFileActions();
            Console.Clear();

            if (commandInput == "pomoc")
            {
                enterFileActions.Help(enterFileActions.Commands);
            }
            else if (commandInput == "otvori komentare")
            {
                enterFileActions.ShowComments(userId, file.Id);
            }
            else if (commandInput == "povratak")
            {
                if (_sharedItemRepository.GetSharedItem(sharedUserId, file.Id) is null)
                {
                    CurrentLocation(userId, parentId);
                    return;
                }

                RootLocation(sharedUserId);
                return;
            }
            else
            {
                Console.WriteLine("Pogresan unos komande. Pokusajte ponovno.\n");
            }

            EnterFile(userId, parentId, name, sharedUserId);
            return;
        }

        public void EnterFile(int userId, int? parentId, string name)
        {
            var file = _fileRepository.GetFile(name, parentId, userId);

            if (file is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Datoteka {name} ne postoji na trenutnoj lokaciji.\n");
                CurrentLocation(userId, parentId);
                return;
            }

            Console.WriteLine($"Ime datoteka: {file.Name}\n\nSadrzaj datoteke:\n{file.Content}\n");
            Console.Write("Unesite komandu ('pomoc' za ispis svih komandi): ");
            var commandInput = Console.ReadLine();
            var enterFileActions = new EnterFileActions();
            Console.Clear();

            if (commandInput == "pomoc")
            {
                enterFileActions.Help(enterFileActions.Commands);
            }
            else if (commandInput == "otvori komentare")
            {
                enterFileActions.ShowComments(userId, file.Id);
            }
            else if (commandInput == "povratak")
            {
                CurrentLocation(userId, parentId);
                return;
            }
            else
            {
                Console.WriteLine("Pogresan unos komande. Pokusajte ponovno.\n");
            }

            EnterFile(userId, parentId, name);
            return;
        }

        public void Back()
        {

        }
    }
}
