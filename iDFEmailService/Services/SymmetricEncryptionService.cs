using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace iDFEmailService.Services
{
    public class SymmetricEncryptionService
    {
        public string Key { get; } = "66c967fefd5b99f57ad86bb7eda79587b5dbd76ec413c6aada2d498fb3eb1cac";
        public string IV { get; } = "15e337b170380151236fafdcbc24e36e";
        private const char OPEN_BRACE = '|';
        private const char CLOSE_BRACE = '~';
        public SymmetricEncryptionService() //: this(string.Empty, string.Empty)
        {

        }
        //public SymmetricEncryptionService(string key, string iv)
        //{
        //    if (!string.IsNullOrEmpty(key)
        //        && !string.IsNullOrEmpty(iv))
        //    {
        //        Key = key;
        //        IV = iv;
        //        return;
        //    }
        //    using (var aes = Aes.Create())
        //    {
        //        if (aes == null) return;
        //        aes.GenerateIV();
        //        Key = ByteArrayToString(aes.Key);
        //        IV = ByteArrayToString(aes.IV);
        //    }

        //    if (!string.IsNullOrEmpty(key))
        //    {
        //        Key = key;
        //    }
        //}
        public string ByteArrayToString(IReadOnlyCollection<byte> ba)
        {
            var hex = new StringBuilder(ba.Count * 2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public byte[] StringToByteArray(string hex)
        {
            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }



        public byte[] Encrypt(string plainText)
        {
            var key = StringToByteArray(Key);
            var iv = StringToByteArray(IV);
            byte[] encrypted;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.WriteLine(plainText.Replace('{', OPEN_BRACE).Replace('}', CLOSE_BRACE), Encoding.Unicode);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            return encrypted;

        }

        public string Decrypt(byte[] cipherText)
        {
            var key = StringToByteArray(Key.Trim());
            var iv = StringToByteArray(IV.Trim());

            string plaintext = null;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext.Replace(OPEN_BRACE, '{').Replace(CLOSE_BRACE, '}');

        }

        public string GetHash(byte[] b)
        {
            ulong result = 0;
            if (b != null || b.Length != 0)
            {
                unchecked
                {
                    result = 0xcbf29ce484222325u; //init prime
                    for (var i = 0; i < b.Length; i++)
                    {
                        result = (result ^ b[i]) * 0x100000001b3; //xor with new and mul with prime
                    }
                }
            }
            return result.ToString();
        }

    }
}
