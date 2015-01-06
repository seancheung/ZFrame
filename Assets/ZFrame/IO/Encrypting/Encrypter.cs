using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZFrame.IO.Encrypting
{
	public static class Encrypter
	{
		#region Temp Test

		private const string KEY_64 = "ZFrameENC";
		private const string IV_64 = "ZFrameENC";

		#endregion

		public static string Encode(string data, byte[] key, byte[] iv)
		{
			DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();

			int i = cryptoProvider.KeySize;

			MemoryStream ms = new MemoryStream();

			CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write);

			StreamWriter sw = new StreamWriter(cst);

			sw.Write(data);

			sw.Flush();

			cst.FlushFinalBlock();

			sw.Flush();

			return Convert.ToBase64String(ms.GetBuffer(), 0, (int) ms.Length);
		}

		public static string Encode(string data)
		{
			byte[] byKey = Encoding.ASCII.GetBytes(KEY_64);

			byte[] byIV = Encoding.ASCII.GetBytes(IV_64);


			return Encode(data, byKey, byIV);
		}

		public static string Decode(string data, byte[] key, byte[] iv)
		{
			byte[] byEnc;

			try
			{
				byEnc = Convert.FromBase64String(data);
			}

			catch
			{
				return null;
			}

			DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();

			MemoryStream ms = new MemoryStream(byEnc);

			CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);

			StreamReader sr = new StreamReader(cst);

			return sr.ReadToEnd();
		}

		public static string Decode(string data)
		{
			byte[] byKey = Encoding.ASCII.GetBytes(KEY_64);

			byte[] byIV = Encoding.ASCII.GetBytes(IV_64);

			return Decode(data, byKey, byIV);
		}
	}
}