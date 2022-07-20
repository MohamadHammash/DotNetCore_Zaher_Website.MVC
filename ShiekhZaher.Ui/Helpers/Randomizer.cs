using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Zaher.Ui.Helpers
{
    public static class Randomizer
    {
        public static string GetCryptedRandomString(int length, bool onlyNumbers)
        {
            string valid;
            if (onlyNumbers)
            {
                valid = "1234567890";
            }
            else
            {
                valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            }

            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return res.ToString();
        }

        public static string GetAUniqueString(List<string> list, int length, bool onlyNumbers)
        {
            var str = GetCryptedRandomString(length, onlyNumbers);

            var exists = list.Any(s => s == str);

            if (exists)
            {
                str = GetAUniqueString(list, length, onlyNumbers);
                return str;
            }
            return str;
        }
    }
}
