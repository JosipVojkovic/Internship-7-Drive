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
    public class EnterFileActions
    {
        private readonly UserRepository _userRepository;
        private readonly FileRepository _fileRepository;
        private readonly CommentRepository _commentRepository;

        public EnterFileActions()
        {
            _userRepository = RepositoryFactory.Create<UserRepository>();
            _fileRepository = RepositoryFactory.Create<FileRepository>();
            _commentRepository = RepositoryFactory.Create<CommentRepository>();
        }

        public Dictionary<EnterFileCommands, (string, string)> Commands = new Dictionary<EnterFileCommands, (string, string)>
        {
            {EnterFileCommands.Help, ("pomoc", "ispis svih komandi")},
            {EnterFileCommands.ShowComments, ("otvori komentare", "ispisivanje svih komentara za datoteku")},
            {EnterFileCommands.Back, ("povratak", "povratak na prijasnji korak")},
        };

        public Dictionary<CommentsCommands, (string, string)> ShowCommentsCommands = new Dictionary<CommentsCommands, (string, string)>
        {
            {CommentsCommands.Help, ("pomoc", "ispis svih komandi")},
            {CommentsCommands.AddComment, ("dodaj komentar", "dodavanje komentara u dokument")},
            {CommentsCommands.DeleteComment, ("izbrisi komentar 'id komentara'", "brisanje komentara dokumenta")},
            {CommentsCommands.UpdateComment, ("uredi komentar 'id komentara'", "uredivanje komentara dokumenta")},
            {CommentsCommands.Back, ("povratak", "povratak na prijasnji korak")},
        };

        public void Help<TEnum>(Dictionary<TEnum, (string, string)> commands) where TEnum : Enum
        {
            Console.WriteLine($"Datoteka komande (ne ukljucujuci :):");

            var isValid = EnumMapper.MapCommands(commands);
            Console.Clear();

            if (!isValid)
            {
                Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                Help(commands);
                return;
            }
            return;
        }

        public void ShowComments(int userId, int fileId)
        {   
            
            var comments = _commentRepository.GetComments(fileId);

            if (comments.Count < 1)
                Console.WriteLine("Komentari: nema komentara");
            else
            {
                Console.WriteLine("Komentari (id - email - datum):\n");
                foreach (var comment in comments)
                {
                    Console.WriteLine($"{comment.Id} - {_userRepository.GetById(comment.UserId).Email} - {comment.CreatedAt}");
                    Console.WriteLine($"Sadrzaj komentara: {comment.Content}\n");
                }
            }

            Console.Write("Unesite komandu ('pomoc' za ispis svih komandi): ");
            var commandInput = Console.ReadLine();
            var command = CommandsValidator.ValidateCommand(ShowCommentsCommands, commandInput);
            Console.Clear();

            if (command is null)
            {
                Console.WriteLine("Pogresan unos komande. Pokusajte ponovno.\n");
                ShowComments(userId, fileId);
                return;
            }

            switch (command.Value.Key)
            {
                case CommentsCommands.Help:
                    Help(ShowCommentsCommands);
                    ShowComments(userId, fileId);
                    return;
                case CommentsCommands.AddComment:
                    AddComment(userId, fileId);
                    ShowComments(userId, fileId);
                    return;
                case CommentsCommands.DeleteComment:
                    DeleteComment(fileId, command.Value.Value[0]);
                    ShowComments(userId, fileId);
                    return;
                case CommentsCommands.UpdateComment:
                    UpdateComment(fileId, command.Value.Value[0]);
                    ShowComments(userId, fileId);
                    return;
                case CommentsCommands.Back:
                    return;
            }

            return;
        }

        public void AddComment(int userId, int fileId)
        {
            var content = InputValidator
                .GetValidInput(InputValidator.IsNotEmptyValidation, "Unesite svoj komentar: ");
            _commentRepository.Add(fileId, userId, content);

            Console.Clear();

            Console.WriteLine($"Komentar uspjesno kreiran!\n");

            return;
        }

        public void DeleteComment(int fileId, string commentId)
        {
            var id = InputValidator.IsNumberValidation(commentId);
            if (id < 0)
            {
                Console.WriteLine("Pogresan unos. Unos mora biti pozitivan cijeli broj.\n");
                return;
            }
            var response = _commentRepository.Delete(fileId, id);
            Console.Clear();

            if (response.Equals(ResponseResultType.NotFound))
                Console.WriteLine($"Operacija neuspjesna. Komentar s id-{commentId} ne postoji.\n");
            else
                Console.WriteLine($"Komentar id-{commentId} uspjesno izbrisan!\n");

            return;
        }

        public void UpdateComment(int fileId, string commentId)
        {
            var id = InputValidator.IsNumberValidation(commentId);
            if (id < 0)
            {
                Console.WriteLine("Pogresan unos. Unos mora biti pozitivan cijeli broj.\n");
                return;
            }
            var comment = _commentRepository.GetComment(fileId, id);
            Console.WriteLine($"{fileId} - {id}");

            if(comment is null)
            {
                Console.WriteLine($"Operacija neuspjesna. Ne postoji komentar id-{commentId}.\n");
                return;
            }

            var newContent = InputValidator
                .GetValidInput(InputValidator.IsNotEmptyValidation, "Unesite novi sadrzaj komentara: ");

            _commentRepository.Update(id, newContent);
            Console.WriteLine($"Komentar id-{commentId} uspjesno ureden!\n");

            return;
        }
    }
}
