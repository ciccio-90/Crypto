using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Crypto
{
    public static class CryptoHelper
    {
        private const string pwd = "&t7&z{!Z9<dTT[B9";
        private const string pwdPattern = "<pwd>(.+?)</pwd>";

        public static string Encrypt(string encryptString)
        {
            if (!string.IsNullOrWhiteSpace(encryptString))
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);

                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(pwd, new byte[] {
                        0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                    });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                        }

                        encryptString = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            return encryptString;
        }

        public static string Decrypt(string cipherString)
        {
            if (!string.IsNullOrWhiteSpace(cipherString))
            {
                cipherString = cipherString.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherString);

                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(pwd, new byte[] {
                        0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                    });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                        }

                        cipherString = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }

            return cipherString;
        }

        public static string DecryptPwd(string pwd)
        {
            if (!string.IsNullOrWhiteSpace(pwd) && Regex.IsMatch(pwd, pwdPattern))
            {
                Match match = Regex.Match(pwd, pwdPattern);

                if (match != null && match.Success)
                {                    
                    return Regex.Replace(pwd, match.Groups?[0]?.ToString(), CryptoHelper.Decrypt(match.Groups?[1]?.ToString()));
                }
                else
                {
                    return pwd;
                }
            }
            else
            {
                return pwd;
            }
        }
    }
}