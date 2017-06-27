using System;
using System.IO;
using System.Security.Cryptography;

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
				using (FileStream msEncrypt = new FileStream(outputFile, FileMode.Create))
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (FileStream fsIn = new FileStream(sourceFile, FileMode.Open))
						{
							int data;
							while ((data = fsIn.ReadByte()) != -1)
							{
								csEncrypt.WriteByte((byte)data);
							}
						}
					}
				}

				// read key and IV
				using (BinaryWriter writer = new BinaryWriter(File.Open(keyFile, FileMode.OpenOrCreate)))
				{
					writer.Write(alg.Key.Length);
					writer.Write(alg.Key);
					writer.Write(alg.IV.Length);
					writer.Write(alg.IV);
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
				using (BinaryReader reader = new BinaryReader(File.Open(keyFile, FileMode.Open)))
				{
					if (reader.PeekChar() > -1)
					{
						int lenght = reader.ReadInt32();
						alg.Key = reader.ReadBytes(lenght);
						lenght = reader.ReadInt32();
						alg.IV = reader.ReadBytes(lenght);
					}
				}

				using (FileStream fsCrypt = new FileStream(sourceFile, FileMode.Open))
				{
					using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
					{
						using (ICryptoTransform decryptor = alg.CreateDecryptor(alg.Key, alg.IV))
						{
							using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
							{
								int data;
								while ((data = cs.ReadByte()) != -1)
								{
									fsOut.WriteByte((byte)data);
								}
							}
						}
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
