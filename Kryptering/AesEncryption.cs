
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Kryptering
{
    /*
     * this class is used to encrypt passwords,
     * all methods are either protected or private.
     * This is my attempt to implement access modifiers in
     * a way that makes the encryption keys as secure as possible.
     */

    /*  NOTE
     * I already know that placing keys in a text folder is a terrible decision
     * however I use it since I had a really hard time finding
     * a good way of storing them, so except for that part I have
     * done my best in order to make this class as secure as possible
     */
    abstract class AesEncryption
    {
        Aes aes;
        string path;
        
        protected AesEncryption()
        {
            this.path = "C:/Kryptering_Pr/key.txt";
            this.aes = Aes.Create();
            byte[][] tempByteArray = new byte[2][];
            tempByteArray = GetKeyAndIV();

            aes.Key = tempByteArray[0];
            aes.IV = tempByteArray[1];
        }

        private byte[][] GetKeyAndIV()
        {
            // gets password encryption key
            StreamReader keyFile = new StreamReader(path);
            string keyString = keyFile.ReadLine();
            string ivString = keyFile.ReadLine();
            keyFile.Close();

            byte[] keyBytes = new byte[32];
            byte[] ivBytes = new byte[16];

            //Console.WriteLine($"Keystring: {keyString}\nIVstring: {ivString}");
            keyBytes = Encoding.UTF8.GetBytes(keyString);
            ivBytes = Encoding.UTF8.GetBytes(ivString);

            byte[][] encryptionVals = {keyBytes, ivBytes};
            return encryptionVals;
        }

        protected string EncryptPassword(string password)
        {
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encryptedPasswordBytes = new byte[256];
            string encryptedPassword = "";

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Write all data to the stream.
                        swEncrypt.Write(password);
                    }
                    encryptedPasswordBytes = msEncrypt.ToArray();
                    foreach (byte b in encryptedPasswordBytes) 
                        encryptedPassword += b; 
                }
            }

            return encryptedPassword;
        }
    }
}
