using System;

namespace Crypto
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var opt = CommandLineParser.Parse(args);

				if (opt.Mode == CryptMode.Encrypt)
				{
					FileEncryptor.Encrypt(opt.SourceFilePath, opt.TargetFilePath, opt.KeyFilePath, opt.Type);
				}
				else if (opt.Mode == CryptMode.Decrypt)
				{
					FileEncryptor.Decrypt(opt.SourceFilePath, opt.TargetFilePath, opt.KeyFilePath, opt.Type);
				}
				else
				{
					CommandLineParser.ShowUsage();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}