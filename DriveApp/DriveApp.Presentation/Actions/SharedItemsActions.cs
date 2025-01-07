using DriveApp.Data.Entities.Models;
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
using System.Xml.Linq;

namespace DriveApp.Presentation.Actions
{
    public class SharedItemsActions
    {
        private readonly UserRepository _userRepository;
        private readonly FolderRepository _folderRepository;
        private readonly FileRepository _fileRepository;
        private readonly SharedItemRepository _sharedItemRepository;

        public SharedItemsActions()
        {
            _userRepository = RepositoryFactory.Create<UserRepository>();
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

                foreach (var sharedFolder in sharedFolders)
                {
                    Console.WriteLine($"  - [{sharedFolder.Id} - {sharedFolder.Name} - {_userRepository.GetById(sharedFolder.OwnerId).Email}]");
                }
            }
            else
                Console.WriteLine("Mape: nema mapa");

            if (sharedFiles.Count > 0)
            {
                Console.WriteLine("Datoteke ([id - ime - email vlasnika]):");

                foreach (var sharedFile in sharedFiles)
                {
                    Console.WriteLine($"  - [{sharedFile.Id} - {sharedFile.Name} - {_userRepository.GetById(sharedFile.OwnerId).Email}]");
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
                    Help(Commands);
                    break;
                case SharedItemsCommands.EnterFolder:
                    var folder = sharedFolders.FirstOrDefault(f => f.Name == command.Value.Value[0]);
                    if (folder is null)
                    {
                        Console.WriteLine($"Operacija neuspjesna. Mapa {command.Value.Value[0]} ne postoji na trenutnoj lokaciji.\n");
                        RootLocation(userId);
                        return;
                    }
                    EnterFolder(userId, folder.OwnerId, folder.ParentId, command.Value.Value[0]);
                    return;
                case SharedItemsCommands.EnterFile:
                    var file = sharedFiles.FirstOrDefault(f => f.Name == command.Value.Value[0]);
                    if (file is null)
                    {
                        Console.WriteLine($"Operacija neuspjesna. Datoteka {command.Value.Value[0]} ne postoji na trenutnoj lokaciji.\n");
                        RootLocation(userId);
                        return;
                    }
                    EnterFile(userId, file.OwnerId, file?.ParentId, command.Value.Value[0]);
                    return;
                case SharedItemsCommands.EditFile:
                    var file1 = sharedFiles.FirstOrDefault(f => f.Name == command.Value.Value[0]);
                    if (file1 is null)
                    {
                        Console.WriteLine($"Operacija neuspjesna. Datoteka {command.Value.Value[0]} ne postoji na trenutnoj lokaciji.\n");
                        RootLocation(userId);
                        return;
                    }
                    myDiscActions.EditFile(file1.OwnerId, file1.ParentId, command.Value.Value[0]);
                    break;
                case SharedItemsCommands.DeleteFolder:
                    var folder1 = sharedFolders.FirstOrDefault(f => f.Name == command.Value.Value[0]);
                    if (folder1 is null)
                    {
                        Console.WriteLine($"Operacija neuspjesna. Mapa {command.Value.Value[0]} ne postoji na trenutnoj lokaciji.\n");
                        RootLocation(userId);
                        return;
                    }
                    DeleteFolder(userId, folder1.ParentId, command.Value.Value[0]);
                    break;
                case SharedItemsCommands.DeleteFile:
                    var file2 = sharedFiles.FirstOrDefault(f => f.Name == command.Value.Value[0]);
                    if (file2 is null)
                    {
                        Console.WriteLine($"Operacija neuspjesna. Datoteka {command.Value.Value[0]} ne postoji na trenutnoj lokaciji.\n");
                        RootLocation(userId);
                        return;
                    }
                    DeleteFile(userId, file2.ParentId, command.Value.Value[0]);
                    break;
                case SharedItemsCommands.Back:
                    var mainMenuActions = new MainMenuActions();
                    mainMenuActions.UserMenu(userId);
                    return;
            }

            RootLocation(userId);
            return;
        }

        public void CurrentLocation(int sharedUserId, int userId, int? parentId)
        {
            var user = _userRepository.GetById(sharedUserId);

            var allSharedFolderIds = _sharedItemRepository
                                        .GetSharedFolders(sharedUserId)
                                        .Select(f => f.Id)
                                        .ToHashSet();
            var allSharedFileIds = _sharedItemRepository
                                        .GetSharedFiles(sharedUserId)
                                        .Select(f => f.Id)
                                        .ToHashSet();

            var sharedFolders = _folderRepository
                .GetFolders(parentId)
                .Where(f => allSharedFolderIds.Contains(f.Id))
                .ToList();

            var sharedFiles = _fileRepository
                .GetFiles(parentId)
                .Where(f => allSharedFileIds.Contains(f.Id))
                .ToList();

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
            var command = CommandsValidator.ValidateCommand(Commands, commandInput);
            Console.Clear();

            if (command is null)
            {
                Console.WriteLine("Pogresan unos komande. Pokusajte ponovno.\n");
                CurrentLocation(sharedUserId, userId, parentId);
                return;
            }

            switch (command.Value.Key)
            {
                case SharedItemsCommands.Help:
                    Help(Commands);
                    break;
                case SharedItemsCommands.EnterFolder:
                    EnterFolder(sharedUserId, userId, parentId, command.Value.Value[0]);
                    return;
                case SharedItemsCommands.EnterFile:
                    EnterFile(sharedUserId, userId, parentId, command.Value.Value[0]);
                    return;
                case SharedItemsCommands.EditFile:
                    var myDiscActions = new MyDiscActions();
                    myDiscActions.EditFile(userId, parentId, command.Value.Value[0]);
                    break;
                case SharedItemsCommands.DeleteFolder:
                    DeleteFolder(sharedUserId, parentId, command.Value.Value[0]);
                    break;
                case SharedItemsCommands.DeleteFile:
                    DeleteFile(sharedUserId, parentId, command.Value.Value[0]);
                    break;
                case SharedItemsCommands.Back:
                    Back(sharedUserId, userId, parentId);
                    return;
            }

            CurrentLocation(sharedUserId, userId, parentId);
        }

        public void Help<TEnum>(Dictionary<TEnum, (string, string)> commands) where TEnum : Enum
        {
            Console.WriteLine($"Moj disk komande (ne ukljucujuci :):");

            var isValid = EnumMapper.MapCommands(Commands);
            Console.Clear();

            if (!isValid)
            {
                Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                Help(commands);
                return;
            }
            return;
        }

        public void EnterFolder(int sharedUserId, int userId, int? parentId, string name)
        {
            var folder = _folderRepository.GetFolder(name, parentId);

            if (folder is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Mapa {name} ne postoji na trenutnoj lokaciji.\n");
                CurrentLocation(sharedUserId, userId, parentId);
            }
            else
                CurrentLocation(sharedUserId, userId, folder.Id);

            return;
        }

        public void EnterFile(int sharedUserId, int userId, int? parentId, string name)
        {
            var file = _fileRepository.GetFile(name, parentId);
            _sharedItemRepository.GetRootSharedItems(sharedUserId, out var folders, out var files);

            if (file is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Datoteka {name} ne postoji na trenutnoj lokaciji.\n");
                if (folders.Any(f => f.Id == parentId))
                {
                    RootLocation(sharedUserId);
                    return;
                }
                CurrentLocation(sharedUserId, userId, parentId);
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
                if (folders.Any(f => f.Id == parentId))
                {
                    RootLocation(sharedUserId);
                    return;
                }

                CurrentLocation(sharedUserId, userId, parentId);
                return;
            }
            else
            {
                Console.WriteLine("Pogresan unos komande. Pokusajte ponovno.\n");
            }

            EnterFile(sharedUserId, userId, parentId, name);
            return;
        }

        public void DeleteFolder(int sharedUserId, int? parentId, string name)
        {
            var actionConfirmation = EnumMapper.ConfirmDialog();

            if (actionConfirmation)
            {
                var folder = _folderRepository.GetFolder(name, parentId);
                var sharedFolder = _sharedItemRepository.GetSharedItem(sharedUserId, folder.Id);
                var response = _sharedItemRepository.Delete(sharedFolder.Id, sharedFolder.OwnerId, sharedUserId);

                if (response == ResponseResultType.NotFound)
                    Console.WriteLine($"Operacija neuspjesna. Mapa {name} ne postoji.\n");
                else
                    Console.WriteLine($"Mapa {name} uspjesno obrisana!\n");
            }

            return;
        }

        public void DeleteFile(int sharedUserId, int? parentId, string name)
        {
            var actionConfirmation = EnumMapper.ConfirmDialog();

            if (actionConfirmation)
            {
                var file = _fileRepository.GetFile(name, parentId);
                var sharedFile = _sharedItemRepository.GetSharedItem(sharedUserId, file.Id);
                var response = _sharedItemRepository.Delete(sharedFile.Id, sharedFile.OwnerId, sharedUserId);

                if (response == ResponseResultType.NotFound)
                    Console.WriteLine($"Operacija neuspjesna. Datoteka {name} ne postoji.\n");
                else
                    Console.WriteLine($"Datoteka {name} uspjesno obrisana!\n");
            }

            return;
        }

        public void Back(int sharedUserId, int userId, int? parentId)
        {
            Console.Clear();

            var folder = _folderRepository.GetById(parentId);
            _sharedItemRepository.GetRootSharedItems(sharedUserId, out var folders, out var files);

            if (folder is null)
            {
                Console.WriteLine("Dogodila se greska. Pokusajte ponovno.\n");
                RootLocation(sharedUserId);
                return;
            }

            if (folders.Any(f => f.Id == parentId))
            {
                RootLocation(sharedUserId);
                return;
            }

            CurrentLocation(sharedUserId, userId, folder.ParentId);
            return;
        }
    }
}
