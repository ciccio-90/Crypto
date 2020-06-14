using System;
using Dot.Net.Infrastructure.Helpers;

namespace Crypto
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || (args != null && args.Length != 2))
            {
                throw new ArgumentNullException(nameof(args));
            }

            switch (args[0])
            {
                case "encrypt":
                    Console.WriteLine(CryptoHelper.Encrypt(args[1]));

                    break;
                case "decrypt":
                    Console.WriteLine(CryptoHelper.Decrypt(args[1]));

                    break;
                case "decrypt-pwd":
                    Console.WriteLine(CryptoHelper.DecryptPwd(args[1]));

                    break;
                default:
                    throw new InvalidOperationException(args[0]);
            }
        }
    }
}
