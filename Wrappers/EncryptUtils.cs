using System;
using System.Text;

namespace LUXED.Wrappers
{
    internal static class EncryptUtils
    {
        public static Random Random => random;
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }
        private static readonly Random random = new Random();
        public static string RandomStringNumber(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }
        public static string ToBase64(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }
        public static byte[] RandomBytes(int length)
        {
            byte[] buffer = new byte[length];
            random.NextBytes(buffer);
            return buffer;
        }
    }
}
