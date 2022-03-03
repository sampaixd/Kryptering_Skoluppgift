

namespace Kryptering
{
    internal class PasswordEncryptor : AesEncryption
    {
        public PasswordEncryptor() : base()
        { }
        public string PublicEncryptPassword(string password)
        {
            return EncryptPassword(password);
        }
    }
}
