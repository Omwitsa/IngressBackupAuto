using System.Security.Cryptography;
using System.Text;

namespace IngressBkpAutomation.Utilities
{
    public class Decryptor
    {
        private const string InitVector = "6BFBE0E1CA96C459";
        private const string PassPhrase = "f2we528192aa23ea283cfcb45627xd30zc784c43";
        private const int KeySize = 256;

        public static string Encrypt(string plainText)
        {
            var initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var password = new PasswordDeriveBytes(PassPhrase, null);
            var keyBytes = password.GetBytes(KeySize / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            var cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string cipherText)
        {
            var initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            var password = new PasswordDeriveBytes(PassPhrase, null);
            var keyBytes = password.GetBytes(KeySize / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];
            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public static string Decript_String(string str)
        {
            int strLen, j, ChVal;
            string Ch, EncptStr;

            strLen = str.Length;
            EncptStr = "";
            for (j = 0; j < strLen; ++j)
            {
                Ch = str.Substring(j, 1);
                char CH = Convert.ToChar(Ch);// problem
                ChVal = (int)CH;

                Char myChar = Convert.ToChar(Decript_Char_Value(ChVal));// was commented
                EncptStr = EncptStr + Convert.ToChar(Decript_Char_Value(ChVal));
            }
            return EncptStr;
        }

        public static int Decript_Char_Value(int Encpt)
        {
            int MaxCharval, c;
            c = 32;
            MaxCharval = 128;
            return MaxCharval - Encpt + c;
        }
    }
}
