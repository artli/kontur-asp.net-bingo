using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MVC.Infrastructure
{
    public static class Security
	{

		#region SALT
		private static readonly Byte[] SALT = {
        0x2B,
        0x1,
        0x23,
        0x1A,
        0xA2,
        0xC2,
        0x84,
        0xB2,
        0x87,
        0x33,
        0xC3,
        0xB2, 
        0x25,
        0x15,
        0x2C,
        0x0F };
		#endregion

		public static Byte[] GenerateHash(String password)
		{
			var pwd = HashPassword(password, HashAlgorithm.Create("SHA512"));
			return pwd;
		}

		private static Byte[] HashPassword(string password, HashAlgorithm hash)
		{
			if (password != null && hash != null)
			{
				byte[] binaryPassword = Encoding.UTF8.GetBytes(password);
				var hash1 = hash.ComputeHash(binaryPassword);
				Byte[] valueToHash = new byte[hash1.Length + SALT.Length];
				hash1.CopyTo(valueToHash, 0);
				SALT.CopyTo(valueToHash, hash1.Length);
				return hash.ComputeHash(valueToHash);
			}
			return null;
		}
		public static Boolean VerifyPassword(String password, Byte[] hash)
		{
			var hashedPassword = GenerateHash(password);
			return hash.SequenceEqual(hashedPassword);
		}
	}
}