using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Security.Cryptography;
using Android.Util;

namespace FSWCore
{
    class Hash
    {
        static string TAG = "Prova Oral - Hash";

        public static string gerarHash(string senha)
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding encoding = Encoding.UTF8;
                Byte[] resultado = hash.ComputeHash(encoding.GetBytes(senha));

                foreach (Byte byteResultado in resultado)
                {
                    stringBuilder.Append(byteResultado.ToString("x2"));
                }
            }

            return stringBuilder.ToString();
        }

        public static bool verificarHash(string hash1, string hash2)
        {
            return hash1.Equals(hash2);
        }
    }
}