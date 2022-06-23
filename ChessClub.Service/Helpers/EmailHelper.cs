namespace ChessClub.Service.Helpers
{
    internal static class EmailHelper
    {
        internal static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (string.IsNullOrWhiteSpace(trimmedEmail) || trimmedEmail.EndsWith("."))
            {
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
