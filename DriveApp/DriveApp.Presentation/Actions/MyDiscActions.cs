using DriveApp.Domain.Factories;
using DriveApp.Domain.Repositories;
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

        public void MainMenu(int userId)
        {
            var user = _userRepository.GetById(userId);
        }

    }
}
