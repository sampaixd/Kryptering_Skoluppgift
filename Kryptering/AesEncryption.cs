using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace Kryptering
{
    abstract class AesEncryption
    {
        Aes aes;
        
        protected AesEncryption()
        {
            this.aes = Aes.Create();
            byte[][] tempByteArray = new byte[2][];
            tempByteArray = GetKeyAndIV();

            aes.Key = tempByteArray[0];
            aes.IV = tempByteArray[1];
        }

        private byte[][] GetKeyAndIV()
        {
            // gets password encryption key
            StreamReader keyFile = new StreamReader("C:/Kryptering_Pr/key.txt");
            string keyString = keyFile.ReadLine();
            string ivString = keyFile.ReadLine();
            keyFile.Close();

            byte[] keyBytes = new byte[256];
            byte[] ivBytes = new byte[256];

            keyBytes = Encoding.UTF8.GetBytes(keyString);
            ivBytes = Encoding.UTF8.GetBytes(ivString);

            byte[][] encryptionVals = {keyBytes, ivBytes};
            return encryptionVals;
        }

        protected byte[] EncryptPassword(string password)
        {
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encryptedPassword = new byte[256];
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Write all data to the stream.
                        swEncrypt.Write(password);
                    }
                    encryptedPassword = msEncrypt.ToArray();
                }
            }

            return encryptedPassword;
        }
    }
}
