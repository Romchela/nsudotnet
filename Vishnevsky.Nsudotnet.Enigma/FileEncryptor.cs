using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Crypto
{

	class FileEncryptor
	{
		public static void Encrypt(string sourceFile, string outputFile, string keyFile, AlgorithmType algType)
		{
			// Check arguments.
			if (sourceFile == null)
				throw new ArgumentNullException("sourceFile");

			if (outputFile == null)
				throw new ArgumentNullException("outputFile");

			if (keyFile == null)
				throw new ArgumentNullException("keyFile");


			using (SymmetricAlgorithm alg = CreateSymmetricAlgorithm(algType))
			{
				// Create a decrytor to perform the stream transform.
				ICryptoTransform encryptor = alg.CreateEncryptor(alg.Key, alg.IV);

				// Create the streams used for encryption.
				using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
				{
					using (CryptoStream cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
					{
						using (FileStream inputStream = new FileStream(sourceFile, FileMode.Open))
						{
							inputStream.CopyTo(cryptoStream);
							inputStream.Close();
						}
					}
					outputStream.Close();
				}

				// write key
				using (var keyStream = new FileStream(keyFile, FileMode.Create))
				{
					var iv = Encoding.ASCII.GetBytes(Convert.ToBase64String(alg.IV));
					var key = Encoding.ASCII.GetBytes(Convert.ToBase64String(alg.Key));
					var endline = Encoding.ASCII.GetBytes("\n");
					keyStream.Write(iv, 0, iv.Length);
					keyStream.Write(endline, 0, 1);
					keyStream.Write(key, 0, key.Length);
					keyStream.Close();
				}
			}
		}


		public static void Decrypt(string sourceFile, string keyFile, string outputFile, AlgorithmType algType)
		{
			// Check arguments.
			if (sourceFile == null)
				throw new ArgumentNullException("sourceFile");

			if (keyFile == null)
				throw new ArgumentNullException("keyFile");

			if (outputFile == null)
				throw new ArgumentNullException("outputFile");

			using (SymmetricAlgorithm alg = CreateSymmetricAlgorithm(algType))
			{

				// read key
				using (var keyStream = new StreamReader(keyFile))
				{
					string ivString = keyStream.ReadLine();
					string keyString = keyStream.ReadLine();
					keyStream.Close();

					if (ivString == null || keyString == null)
					{
						throw new Exception("Invalid key or IV");
					}

					alg.IV = Convert.FromBase64String(ivString);
					alg.Key = Convert.FromBase64String(keyString);
				}

				using (FileStream inputStream = new FileStream(sourceFile, FileMode.Open))
				{
					using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
					{
						using (ICryptoTransform decryptor = alg.CreateDecryptor(alg.Key, alg.IV))
						{
							using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
							{
								cryptoStream.CopyTo(outputStream);
								inputStream.Close();
							}
						}
						outputStream.Close();
					}
				}

			}
		}

		public static SymmetricAlgorithm CreateSymmetricAlgorithm(AlgorithmType type)
		{
			switch (type)
			{
				case AlgorithmType.AES:
					return Aes.Create();
				case AlgorithmType.DES:
					return new TripleDESCryptoServiceProvider();
				case AlgorithmType.RC2:
					return RC2.Create();
				case AlgorithmType.Rijndael:
					return new RijndaelManaged();
				default:
					throw new NotSupportedException(String.Format("Crypto type {0} is not supported.", type));
			}
		}

	}
}
