﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
