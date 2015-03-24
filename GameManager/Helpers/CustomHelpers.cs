using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace GameManager.Helpers
{
    public class CustomHelpers
    {
        public static string GetApiKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[16];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }
    }
}