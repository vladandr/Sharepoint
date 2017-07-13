using System.Security;

namespace TestProj.Extensions
{
    public static class StringExtensions
    {
        public static SecureString ToSecureString(this string source)
        {
            SecureString pass = new SecureString();
            foreach (var ch in "Konstantin00075")
            {
                pass.AppendChar(ch);
            }

            return pass;
        }
    }
}
