using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekinci.Common.Helpers
{
    public class KeyGenerator
    {
        public static string CreateRandomPassword(int length = 14)
        {
            string text = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();
            char[] array = new char[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = text[random.Next(0, text.Length)];
            }

            return new string(array);
        }

        public static int CreateRandomNumber(int min = 100000, int max = 999999)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
