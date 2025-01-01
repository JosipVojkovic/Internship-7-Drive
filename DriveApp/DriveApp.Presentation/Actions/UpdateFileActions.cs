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
    public class UpdateFileActions
    {
        private readonly FileRepository _fileRepository;

        public UpdateFileActions()
        {
            _fileRepository = RepositoryFactory.Create<FileRepository>();
        }

        public Dictionary<UpdateFileCommands, (string, string)> Commands = new Dictionary<UpdateFileCommands, (string, string)>
        {
            {UpdateFileCommands.Help, (":pomoc", "ispis svih komandi")},
            {UpdateFileCommands.Save, (":spremi", "spremanje unesenog sadrzaja u datoteku i povratak na prijasnji korak")},
            {UpdateFileCommands.Back, (":povratak", "povratak na prijasnji korak")},
        };

        public void Help(int userId, int? parentId)
        {
            Console.WriteLine($"Uredivanje datoteke komande (ne ukljucujuci : na kraju komande):");

            var isValid = EnumMapper.MapCommands<UpdateFileCommands>(Commands);
            Console.Clear();

            if (!isValid)
            {
                Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                Help(userId, parentId);
                return;
            }
            return;
        }
    }
}
