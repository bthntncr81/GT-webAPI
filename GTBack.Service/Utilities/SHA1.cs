using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;

namespace GTBack.Service.Utilities
{
    public static class SHA1
    {
        public static string Generate(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }
            return hash.ToLower();
        }

        public static bool Verify(string text, string textHashed)
        {
            if (Generate(text).Equals(textHashed))
            {
                return true;
            }
            else
            {
                return false;
            }
            return Generate(text) == textHashed;
        }
    }
}
