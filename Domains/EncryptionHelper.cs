﻿using System.Security.Cryptography;
using System.Text;

namespace CQRSWithDocker_Identity.Domains
{
    public static class EncryptionHelper
    {
        private static readonly string EncryptionKey = "X9IosOL5u3pjnyd+";//hviaMFR74l5wUB6tc1QLzb45uDk=";

        public static string EncryptString(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aes.GenerateIV();
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                        }


                        var iv = aes.IV;
                        var encrypted = ms.ToArray();


                        var result = new byte[iv.Length + encrypted.Length];
                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);


                        return Convert.ToBase64String(result);
                    }
                }
            }
        }


        public static string DecryptString(string cipherText)
        {
            try
            {
                var fullCipher = Convert.FromBase64String(cipherText);
                using (var aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                    var iv = new byte[aes.BlockSize / 8];
                    var cipher = new byte[fullCipher.Length - iv.Length];


                    Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                    Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);


                    aes.IV = iv;
                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        using (var ms = new MemoryStream(cipher))
                        {
                            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                            {
                                using (var sr = new StreamReader(cs))
                                {
                                    return sr.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var data = EncryptString(cipherText);
              //  var dat=DecryptString(data);
                return ex.Message;


            }
        }
    }

}