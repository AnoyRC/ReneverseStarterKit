using System.Runtime.CompilerServices;
using System.Text;

namespace ReneVerse
{
    public static class Extensions
    {
        public static string AddPoint(this string str)
        {
            return str + ".";
        }
        public static bool IsEmail(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            try
            {
                var address = new System.Net.Mail.MailAddress(str);
                return address.Address == str;
            }
            catch
            {
                return false;
            }
        }
        public static string SeparateCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(str[0]);

            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]))
                {
                    builder.Append(' ');
                }

                builder.Append(str[i]);
            }

            return builder.ToString();
        }
        
        public static string UnderscoresToSpaces(this string str)
        {
            return str.Replace("_", " ");
        }
        
        public static string ToLoginUrl(this string input)
        {
            input = input.Replace("api", "app");
            return "http://" + input.Trim() + "/login";
        }
        
        public static string AddEnterYour(this string input)
        {
            return "Enter your " + input;
        }
        
        public static string GetName<T>(this T instance, [CallerMemberName]string memberName = "")
        {
            return memberName;
        }
    }
}


