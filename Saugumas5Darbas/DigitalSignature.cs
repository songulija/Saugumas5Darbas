using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Saugumas5Darbas
{
    public static class DigitalSignature
    {
        //public static string ContainerName { get; private set; }
        public static string ContainerName = "KeyContainer";

        public static byte[] SignMessage(string message)
        {

            using (SHA256 sha = SHA256Managed.Create())

            {

                byte[] hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));

                CspParameters csp = new CspParameters();

                csp.KeyContainerName = ContainerName;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);

                var formatter = new RSAPKCS1SignatureFormatter(rsa);

                formatter.SetHashAlgorithm("SHA256");

                return formatter.CreateSignature(hashValue);

            }

        }

        public static bool VerifySignedMessage(byte[] hash, byte[] signature)
        {

            CspParameters csp = new CspParameters();

            csp.KeyContainerName = ContainerName;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp))

            {

                var deformatter = new RSAPKCS1SignatureDeformatter(rsa);

                deformatter.SetHashAlgorithm("SHA256");

                return deformatter.VerifySignature(hash, signature);

            }

        }
    }
}
