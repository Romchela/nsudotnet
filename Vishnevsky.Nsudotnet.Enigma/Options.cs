namespace Crypto
{
	public enum CryptMode
	{
		Encrypt,
		Decrypt
	};

	public class Options
	{
		public CryptMode Mode { get; set; }
		public AlgorithmType Type { get; set; }
		public string SourceFilePath { get; set; }
		public string TargetFilePath { get; set; }
		public string KeyFilePath { get; set; }
	}
}
