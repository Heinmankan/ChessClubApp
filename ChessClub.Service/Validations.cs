using ChessClub.Service.Helpers;

namespace ChessClub.Service
{
    internal static class Validations
    {
        internal static bool IsValidName(string name) => !string.IsNullOrWhiteSpace(name);

        internal static bool IsValidSurname(string surname) => !string.IsNullOrWhiteSpace(surname);

        internal static bool IsValidEmailAddress(string email) => EmailHelper.IsValidEmail(email);

        internal static bool IsValidBirthday(DateTime birthday) => birthday < DateTime.Now.AddYears(-1);
    }
}
