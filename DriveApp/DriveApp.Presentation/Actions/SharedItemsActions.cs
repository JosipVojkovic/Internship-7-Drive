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

        public void CurrentLocation(int userId, int? parentId)
        {
            var user = _userRepository.GetById(userId);

            var sharedFolders = parentId is null?
                                    _sharedItemRepository.GetSharedFolders(userId): 
                                    _folderRepository.GetFolders(userId, parentId);
            var sharedFiles = parentId is null? 
                                _sharedItemRepository.GetSharedFiles(userId): 
                                _fileRepository.GetFiles(userId,parentId);
            
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
            /*var command = CommandsValidator.ValidateCommand(Commands, commandInput);
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

            CurrentLocation(userId, parentId); */
            return;
        }
    }
}
