using System;
using System.IO;

namespace Crypto
{
	class CommandLineParser
	{
		public static Options Parse(string[] args)
		{
			if (args.Length == 4 || args.Length == 5)
			{
				var opt = new Options()
				{
					Mode = ParseMode(args[0]),
					SourceFilePath = args[1],
					Type = ParseCryptoType(args[2]),
					TargetFilePath = args[3]
				};

				if (opt.Mode == CryptMode.Decrypt)
				{
					if (args.Length == 5)
					{
						opt.KeyFilePath = args[4];
					}
					else
					{
						throw new ArgumentException("Key file path is absent.");
					}
				}
				else
				{
					string extention = Path.GetExtension(opt.SourceFilePath);
					opt.KeyFilePath = Path.ChangeExtension(opt.SourceFilePath, "key" + extention);
				}

				return opt;
			}
			else
			{
				throw new ArgumentException("Amount of arguments are not valid.");
			}
		}

		public static void ShowUsage()
		{
			Console.WriteLine("{0} encrypt sourceFilePath aes|des|rc2|Rijndael  outputFilePath", System.AppDomain.CurrentDomain.FriendlyName);
			Console.WriteLine("{0} decrypt sourceFilePath aes|des|rc2|Rijndael  keyFilePath outputFilePath", System.AppDomain.CurrentDomain.FriendlyName);
		}

		public static AlgorithmType ParseCryptoType(string type)
		{
			switch (type.ToUpper())
			{
				case "AES":
					return AlgorithmType.AES;
				case "DES":
					return AlgorithmType.DES;
				case "RC2":
					return AlgorithmType.RC2;
				case "RIJNDAEL":
					return AlgorithmType.Rijndael;
				default:
					throw new ArgumentException("Unknown crypto method {0}", type);
			}
		}

		public static CryptMode ParseMode(string mode)
		{
			switch (mode.ToUpper())
			{
				case "ENCRYPT":
					return CryptMode.Encrypt;
				case "DECRYPT":
					return CryptMode.Decrypt;
				default:
					throw new ArgumentException("Unknown mode {0}", mode);
			}
		}
	}
}
