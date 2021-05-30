using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Saugumas5Darbas
{
    public static class ExtensionMethods
    {
        public static byte[] ComputeMessageHash(this string value)
        {
            //The ComputeMessageHash() method is not complex and simply returns the hash for the message received.

            using (SHA256 sha = SHA256.Create())

            {

                return sha.ComputeHash(Encoding.UTF8.GetBytes(value));

            }

        }
    }
}
