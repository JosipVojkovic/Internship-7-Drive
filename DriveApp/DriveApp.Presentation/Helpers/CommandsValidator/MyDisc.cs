using DriveApp.Presentation.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Presentation.Helpers.CommandsValidator
{
    public static class MyDisc
    {
        public static bool HelpValidation(string input)
        {
            return input == "pomoc";
        }

        public static bool CommandValidation(string commandText, string input)
        {
            if (input.StartsWith(commandText) && 
                !string.IsNullOrWhiteSpace(input.Substring("stvori mapu ".Length)))
                return true;
            else
                return false;
        }

        public static bool ChangeItemNameValidation(string commandText, string input)
        {
            if (input.StartsWith(commandText) &&
                !string.IsNullOrWhiteSpace(input.Substring("stvori mapu ".Length)))
                return true;
            else
                return false;
        }
    }
}
