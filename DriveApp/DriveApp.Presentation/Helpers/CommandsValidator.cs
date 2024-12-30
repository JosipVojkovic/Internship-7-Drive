using DriveApp.Presentation.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DriveApp.Presentation.Helpers
{
    public static class CommandsValidator
    {
        public static bool HelpValidation(string input)
        {
            return input == "pomoc";
        }

        public static KeyValuePair<MyDiscCommands, List<string>>? ValidateCommand(Dictionary<MyDiscCommands, (string, string)> commands, string commandInput)
        {
            foreach (var command in commands)
            {
                string format = command.Value.Item1;
                var placeholderRegex = new Regex(@"'([^']*)'");
                var formatPlaceholders = placeholderRegex.Matches(format);
                string dynamicRegexPattern = Regex.Escape(format);

                foreach (Match placeholder in formatPlaceholders)
                {
                    dynamicRegexPattern = dynamicRegexPattern.Replace(
                        Regex.Escape(placeholder.Value),
                        @"'([^']+)'");
                }

                dynamicRegexPattern = "^" + dynamicRegexPattern + "$";
                var commandRegex = new Regex(dynamicRegexPattern);
                var match = commandRegex.Match(commandInput);

                if (match.Success)
                {
                    List<string> valuesInsideQuotes = new List<string>();

                    for (int i = 1; i < match.Groups.Count; i++)
                    {
                        var groupValue = match.Groups[i].Value.Trim();

                        if (string.IsNullOrWhiteSpace(groupValue))
                            return null;

                        valuesInsideQuotes.Add(groupValue);
                    }

                    return new KeyValuePair<MyDiscCommands, List<string>>(command.Key, valuesInsideQuotes);
                }
            }

            return null;
        }
    }
}
