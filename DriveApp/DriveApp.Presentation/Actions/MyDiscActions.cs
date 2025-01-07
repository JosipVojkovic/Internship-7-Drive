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
using System.Collections;
using DriveApp.Data.Entities.Models;

namespace DriveApp.Presentation.Actions
{
    public class MyDiscActions
    {
        private readonly UserRepository _userRepository;
        private readonly ItemRepository _itemRepository;
        private readonly FolderRepository _folderRepository;
        private readonly FileRepository _fileRepository;
        private readonly SharedItemRepository _sharedItemRepository;

        public MyDiscActions()
        {
            _userRepository = RepositoryFactory.Create<UserRepository>();
            _itemRepository = RepositoryFactory.Create<ItemRepository>();
            _folderRepository = RepositoryFactory.Create<FolderRepository>();
            _fileRepository = RepositoryFactory.Create<FileRepository>();
            _sharedItemRepository = RepositoryFactory.Create<SharedItemRepository>();
        }

        Dictionary<MyDiscCommands, (string, string)> Commands = new Dictionary<MyDiscCommands, (string, string)>
        {
            {MyDiscCommands.Help, ("pomoc", "ispis svih komandi")},
            {MyDiscCommands.CreateFolder, ("stvori mapu 'ime mape'", "stvaranje mape na trenutnoj lokaciji")},
            {MyDiscCommands.CreateFile, ("stvori datoteku 'ime datoteke'", "stvaranje datoteke na trenutnoj lokaciji")},
            {MyDiscCommands.EnterFolder, ("udi u mapu 'ime mape'", "ulazak u mapu na trenutnoj lokaciji")},
            {MyDiscCommands.EnterFile, ("udi u datoteku 'ime datoteke'", "ulazak u datoteku na trenutnoj lokaciji")},
            {MyDiscCommands.EditFile, ("uredi datoteku 'ime datoteke'", "uredivanje datoteke na trenutnoj lokaciji")},
            {
                MyDiscCommands.DeleteFolder, 
                (
                    "izbrisi mapu 'ime mape'", 
                    "brisanje mape na trenutnoj lokaciji"
                )
            },
            {
                MyDiscCommands.DeleteFile,
                (
                    "izbrisi datoteku 'ime datoteke'",
                    "brisanje datoteke na trenutnoj lokaciji"
                )
            },
            {
                MyDiscCommands.ChangeFolderName, 
                (
                    "promijeni ime mape 'ime mape' u 'novo ime mape'", 
                    "mijenjanje naziva mape na trenutnoj lokaciji"
                )
            },
            {
                MyDiscCommands.ChangeFileName,
                (
                    "promijeni ime datoteke 'ime datoteke' u 'novo ime datoteke'",
                    "mijenjanje naziva datoteke na trenutnoj lokaciji"
                )
            },
            {
                MyDiscCommands.ShareFolder,
                (
                    "podijeli mapu 'ime mape' s 'email korisnika'",
                    "dijeljenje mape s drugim korisnikom"
                )
            },
            {
                MyDiscCommands.ShareFile,
                (
                    "podijeli datoteku 'ime datoteke' s 'email korisnika'",
                    "dijeljenje datoteke s drugim korisnikom"
                )
            },
            {
                MyDiscCommands.StopSharingFolder,
                (
                    "prestani dijeliti mapu 'ime mape' s 'email korisnika'",
                    "prestanak dijeljenja mape s drugim korisnikom"
                )
            },
            {
                MyDiscCommands.StopSharingFile,
                (
                    "prestani dijeliti datoteku 'ime datoteke' s 'email korisnika'",
                    "prestanak dijeljenja datoteke s drugim korisnikom"
                )
            },
            {MyDiscCommands.Back, ("povratak", "povratak na prijasnji korak")}
        };
        public void CurrentLocation(int userId, int? parentId)
        {
            var user = _userRepository.GetById(userId);
            _itemRepository.GetItems(userId, parentId, out var folders, out var files);
            Console.WriteLine($"{user.FirstName} {user.LastName} => MOJ DISK\n");

            
            if(folders.Count > 0)
            {   
                Console.WriteLine("Mape:");

                foreach (var folder in folders)
                {
                    Console.WriteLine("  - " + folder.Name);
                }
            }
            else
                Console.WriteLine("Mape: nema mapa");
            
            if(files.Count > 0)
            {
                Console.WriteLine("Datoteke:");

                foreach (var file in files)
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
                CurrentLocation(userId, parentId);
                return;
            }

            switch (command.Value.Key)
            {
                case MyDiscCommands.Help:
                    Help(Commands);
                    break;
                case MyDiscCommands.CreateFolder:
                    CreateFolder(userId, parentId, command.Value.Value[0]);
                    break;
                case MyDiscCommands.CreateFile:
                    CreateFile(userId, parentId, command.Value.Value[0]);
                    break;
                case MyDiscCommands.EnterFolder:
                    EnterFolder(userId, parentId, command.Value.Value[0]);
                    return;
                case MyDiscCommands.EnterFile:
                    EnterFile(userId, parentId, command.Value.Value[0]);
                    return;
                case MyDiscCommands.EditFile:
                    EditFile(userId, parentId, command.Value.Value[0]);
                    break;
                case MyDiscCommands.DeleteFolder:
                    DeleteFolder(userId, parentId, command.Value.Value[0]);
                    break;
                case MyDiscCommands.DeleteFile:
                    DeleteFile(userId, parentId, command.Value.Value[0]);
                    break;
                case MyDiscCommands.ChangeFolderName:
                    ChangeFolderName(userId, parentId, command.Value.Value[0], command.Value.Value[1]);
                    break;
                case MyDiscCommands.ChangeFileName:
                    ChangeFileName(userId, parentId, command.Value.Value[0], command.Value.Value[1]);
                    break;
                case MyDiscCommands.ShareFolder:
                    ShareFolder(userId, parentId, command.Value.Value[0], command.Value.Value[1]);
                    break;
                case MyDiscCommands.ShareFile:
                    ShareFile(userId, parentId, command.Value.Value[0], command.Value.Value[1]);
                    break;
                case MyDiscCommands.StopSharingFolder:
                    StopSharingFolder(userId, parentId, command.Value.Value[0], command.Value.Value[1]);
                    break;
                case MyDiscCommands.StopSharingFile:
                    StopSharingFile(userId, parentId, command.Value.Value[0], command.Value.Value[1]);
                    break;
                case MyDiscCommands.Back:
                    Back(userId, parentId);
                    return;
            }

            CurrentLocation(userId, parentId);
            return;
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

        public void CreateFolder(int userId, int? parentId, string name)
        {
            var actionConfirmation = EnumMapper.ConfirmDialog();

            if (actionConfirmation)
            {
                var response = _folderRepository.Add(name, parentId, userId);

                if (response == ResponseResultType.AlreadyExists)
                    Console.WriteLine($"Operacija neuspjesna. Mapa {name} vec postoji.\n");
                else
                    Console.WriteLine($"Mapa {name} uspjesno kreirana!\n");
            }

            return;
        }

        public void CreateFile(int userId, int? parentId, string name)
        {
            Console.Write("Unesite sadrzaj datoteke: ");
            var content = Console.ReadLine();

            if (content is null)
                content = "";

            var actionConfirmation = EnumMapper.ConfirmDialog();

            if (actionConfirmation)
            {
                var response = _fileRepository.Add(name, parentId, userId, content);
                Console.Clear();

                if (response == ResponseResultType.AlreadyExists)
                    Console.WriteLine($"Operacija neuspjesna. Datoteka {name} vec postoji.\n");
                else
                    Console.WriteLine($"Datoteka {name} uspjesno kreirana!\n");
            }    

            return;
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
            else if(commandInput == "otvori komentare")
            {
                enterFileActions.ShowComments(userId, file.Id);
            }
            else if(commandInput == "povratak")
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

        public void EditFile(int userId, int? parentId, string name)
        {
            var file = _fileRepository.GetFile(name, parentId, userId);

            if (file is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Datoteka {name} ne postoji na trenutnoj lokaciji.\n");
                return;
            }

            var updateFileActions = new UpdateFileActions();
            var command = EnumMapper.MultiLineInput(updateFileActions.Commands, name);
            Console.Clear();

            switch (command.Item1)
            {
                case UpdateFileCommands.Help:
                    updateFileActions.Help();
                    EnumMapper.MultiLineInput(updateFileActions.Commands, name);
                    return;
                case UpdateFileCommands.Save:
                    _fileRepository.UpdateContent(file, command.Item2);
                    Console.WriteLine($"Datoteka {name} uspjesno uredena!\n");
                    break;
            }

            return;
        }

        public void DeleteFolder(int userId, int? parentId, string name)
        {
            var actionConfirmation = EnumMapper.ConfirmDialog();

            if (actionConfirmation)
            {
                var response = _folderRepository.Delete(name, parentId, userId);

                if (response == ResponseResultType.NotFound)
                    Console.WriteLine($"Operacija neuspjesna. Mapa {name} ne postoji.\n");
                else
                    Console.WriteLine($"Mapa {name} uspjesno obrisana!\n");
            }

            return;
        }

        public void DeleteFile(int userId, int? parentId, string name)
        {
            var actionConfirmation = EnumMapper.ConfirmDialog();

            if (actionConfirmation)
            {
                var response = _fileRepository.Delete(name, parentId, userId);

                if (response == ResponseResultType.NotFound)
                    Console.WriteLine($"Operacija neuspjesna. Datoteka {name} ne postoji.\n");
                else
                    Console.WriteLine($"Datoteka {name} uspjesno obrisana!\n");
            }

            return;
        }

        public void ChangeFolderName(int userId, int? parentId, string name, string newName)
        {
            var actionConfirmation = EnumMapper.ConfirmDialog();

            if (actionConfirmation)
            {
                var folder = _folderRepository.GetFolder(name, parentId, userId);

                if(folder is null)
                {
                    Console.WriteLine($"Operacija neuspjesna. Mapa {name} ne postoji.\n");
                    return;
                }

                var isNameTaken = _folderRepository.GetFolder(newName, parentId, userId);

                if (isNameTaken is not null)
                {
                   Console.WriteLine($"Operacija neuspjesna. Mapa {newName} vec postoji.\n");
                    return;
                }
 

                _folderRepository.Update(folder, newName);
                Console.WriteLine($"Ime mape {name} uspjesno promijenjeno u novo ime {newName}!\n");
            }
                    
            return;
        }

        public void ChangeFileName(int userId, int? parentId, string name, string newName)
        {
            var actionConfirmation = EnumMapper.ConfirmDialog();

            if (actionConfirmation) 
            { 
                var file = _fileRepository.GetFile(name, parentId, userId);

                if (file is null)
                {
                    Console.WriteLine($"Operacija neuspjesna. Datoteka {name} ne postoji.\n");
                    return;
                }

                var isNameTaken = _fileRepository.GetFile(newName, parentId, userId);

                if (isNameTaken is not null)
                {
                    Console.WriteLine($"Operacija neuspjesna. Datoteka {newName} vec postoji.\n");
                    return;
                }

               _fileRepository.Update(file, newName);
                Console.WriteLine($"Ime datoteke {name} uspjesno promijenjeno u novo ime {newName}!\n");
            }
 
            return;
        }

        public void ShareFolder(int userId, int? parentId, string name, string email)
        {
            Console.Clear();
            var folder = _folderRepository.GetFolder(name, parentId, userId);

            if (folder is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Mapa {name} ne postoji.\n");
                return;
            }

            var sharedUser = _userRepository.GetUser(email);

            if (sharedUser is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Korisnik {email} ne postoji.\n");
                return;
            }

            var response = _sharedItemRepository.Add(folder.Id, userId, sharedUser.Id);

            if (response.Equals(ResponseResultType.AlreadyExists))
            {
                Console.WriteLine($"Operacija neuspjesna. Mapa {name} je vec podijeljena s korisnikom {email}.\n");
            }
            else
            {
                Console.WriteLine($"Mapa {name} uspjesno podijeljena s korisnikom {email}!\n");
            }

            return;
        }

        public void ShareFile(int userId, int? parentId, string name, string email)
        {
            var file = _fileRepository.GetFile(name, parentId, userId);

            if (file is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Datoteka {name} ne postoji.\n");
                return;
            }

            var sharedUser = _userRepository.GetUser(email);

            if (sharedUser is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Korisnik {email} ne postoji.\n");
                return;
            }

            var response = _sharedItemRepository.Add(file.Id, userId, sharedUser.Id);

            if (response.Equals(ResponseResultType.AlreadyExists))
            {
                Console.WriteLine($"Operacija neuspjesna. Datoteka {name} je vec podijeljena s korisnikom {email}.\n");
            }
            else
            {
                Console.WriteLine($"Datoteka {name} uspjesno podijeljena s korisnikom {email}!\n");
            }

            return;
        }

        public void StopSharingFolder(int userId, int? parentId, string name, string email)
        {
            Console.Clear();
            var folder = _folderRepository.GetFolder(name, parentId, userId);

            if (folder is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Mapa {name} ne postoji.\n");
                return;
            }

            var sharedUser = _userRepository.GetUser(email);

            if (sharedUser is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Korisnik {email} ne postoji.\n");
                return;
            }

            var response = _sharedItemRepository.Delete(folder.Id, userId, sharedUser.Id);

            if (response.Equals(ResponseResultType.NotFound))
            {
                Console.WriteLine($"Operacija neuspjesna. Mapa {name} nije podijeljena s korisnikom {email}.\n");
            }
            else
            {
                Console.WriteLine($"Mapa {name} uspjesno vise nije podijeljena s korisnikom {email}!\n");
            }

            return;
        }

        public void StopSharingFile(int userId, int? parentId, string name, string email)
        {
            var file = _fileRepository.GetFile(name, parentId, userId);

            if (file is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Datoteka {name} ne postoji.\n");
                return;
            }

            var sharedUser = _userRepository.GetUser(email);

            if (sharedUser is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Korisnik {email} ne postoji.\n");
                return;
            }

            var response = _sharedItemRepository.Delete(file.Id, userId, sharedUser.Id);

            if (response.Equals(ResponseResultType.NotFound))
            {
                Console.WriteLine($"Operacija neuspjesna. Datoteka {name} nije podijeljena s korisnikom {email}.\n");
            }
            else
            {
                Console.WriteLine($"Datoteka {name} uspjesno vise nije podijeljena s korisnikom {email}!\n");
            }

            return;
        }

        public void Back(int userId, int? parentId)
        {
            var mainMenuActions = new MainMenuActions();
            Console.Clear();

            if (parentId is null)
            {
                mainMenuActions.UserMenu(userId);
                return;
            }

            var folder = _folderRepository.GetById(parentId);

            if (folder is null)
            {
                Console.WriteLine("Dogodila se greska. Pokusajte ponovno.\n");
                mainMenuActions.UserMenu(userId);
                return;
            }

            CurrentLocation(userId, folder.ParentId);
            return;
        }
    }
}
