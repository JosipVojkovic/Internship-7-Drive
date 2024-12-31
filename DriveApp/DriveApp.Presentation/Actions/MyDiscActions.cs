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

namespace DriveApp.Presentation.Actions
{
    public class MyDiscActions
    {
        private readonly UserRepository _userRepository;
        private readonly FolderRepository _folderRepository;
        private readonly FileRepository _fileRepository;

        public MyDiscActions()
        {
            _userRepository = RepositoryFactory.Create<UserRepository>();
            _folderRepository = RepositoryFactory.Create<FolderRepository>();
            _fileRepository = RepositoryFactory.Create<FileRepository>();
        }

        Dictionary<MyDiscCommands, (string, string)> Commands = new Dictionary<MyDiscCommands, (string, string)>
        {
            {MyDiscCommands.Help, ("pomoc", "ispis svih komandi")},
            {MyDiscCommands.CreateFolder, ("stvori mapu 'ime mape'", "stvaranje mape na trenutnoj lokaciji")},
            {MyDiscCommands.CreateFile, ("stvori datoteku 'ime datoteke'", "stvaranje datoteke na trenutnoj lokaciji")},
            {MyDiscCommands.EnterFolder, ("udi u mapu 'ime mape'", "ulazak u mapu na trenutnoj lokaciji")},
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
            {MyDiscCommands.Back, ("povratak", "povratak na prijasnji korak")}
        };
        public void CurrentLocation(int userId, int? parentId)
        {
            var user = _userRepository.GetById(userId);
            var folders = _folderRepository.GetFolders(userId, parentId);
            var files = _fileRepository.GetFiles(userId, parentId);
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
                    Help(userId, parentId);
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
                case MyDiscCommands.Back:
                    Back(userId, parentId);
                    return;
            }

            CurrentLocation(userId, parentId);
            return;
        }

        public void Help(int userId, int? parentId)
        {
            Console.WriteLine($"Moj disk komande (ne ukljucujuci :):");

            var isValid = EnumMapper.MapCommands<MyDiscCommands>(Commands);
            Console.Clear();

            if (!isValid)
            {
                Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                Help(userId, parentId);
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
            var newFolder = _folderRepository.GetFolder(name, parentId, userId);

            if (newFolder is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Mapa {name} ne postoji na trenutnoj lokaciji.\n");
                CurrentLocation(userId, parentId);
            }
                
            else
                CurrentLocation(userId, newFolder.Id);

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

            Console.WriteLine($"Ime datoteke: {file.Name}\n\nSadrzaj datoteke:\n{file.Content}\n");

            var updateFileActions = new UpdateFileActions();
            var command = EnumMapper.MultiLineInput(updateFileActions.Commands, name);
            Console.Clear();

            switch (command.Item1)
            {
                case UpdateFileCommands.Help:
                    updateFileActions.Help(userId, parentId);
                    EnumMapper.MultiLineInput(updateFileActions.Commands, name);
                    return;
                case UpdateFileCommands.Save:
                    _fileRepository.UpdateContent(file, command.Item2);
                    Console.WriteLine($"Datoteka {name} uspjesno uredena!\n");
                    break;
            }

            CurrentLocation(userId, parentId);
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
