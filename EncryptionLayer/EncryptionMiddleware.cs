using CQRSWithDocker_Identity.Data;
using CQRSWithDocker_Identity.Domains;
using System.Security.Cryptography;
using System.Text;

namespace CQRSWithDocker_Identity.EncryptionLayer
{
    public class EncryptionMiddleware
    {
       
        private static readonly string EncryptionKey = "X9IosOL5u3pjnyd+hviaMFR74l5wUB6tc1QLzb45uDk=";
       
        //public EncryptionMiddleware(RequestDelegate next)
        //{
        //    _next = next;
       
        //}
        //private static CryptoStream EncryptStream(Stream responseStream)
        //{
        //    Aes aes = GetEncryptionAlgorithm();
        //    ToBase64Transform base64Transform = new ToBase64Transform();
        //    CryptoStream base64EncodedStream = new CryptoStream(responseStream, base64Transform, CryptoStreamMode.Write);
        //    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        //    CryptoStream cryptoStream = new CryptoStream(base64EncodedStream, encryptor, CryptoStreamMode.Write);
        //    return cryptoStream;
        //}
        //static byte[] Generate256BitKey()
        //{
        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.KeySize = 256; // Set key size to 256 bits
        //        aes.GenerateKey();
        //        return aes.Key;
        //    }
        //}
        //static byte[] Generate128BitIV()
        //{
        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.BlockSize = 256; // Set block size to 128 bits
        //        aes.GenerateIV();
        //        return aes.IV;
        //    }
        //}

        //private static Aes GetEncryptionAlgorithm()
        //{

        //    Aes aes = Aes.Create();
        //    byte[] byteArr = { 5, 2, 8, 9, 0, 7 };
        //    //var product = new Domains.Loggers("GetUserList", "", "");
        //  //  _context.Logger.AddAsync(product);
        //    byte[] key = Encoding.UTF8.GetBytes(EncryptionKey);// Generate128BitKey();
        //    aes.Key = key;
        //    aes.IV = Generate128BitIV();

        //    return aes;
        //}
        //private static Stream DecryptStream(Stream cipherStream)
        //{
        //    Aes aes = GetEncryptionAlgorithm();

        //    FromBase64Transform base64Transform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces);
        //    CryptoStream base64DecodedStream = new CryptoStream(cipherStream, base64Transform, CryptoStreamMode.Read);
        //    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        //    CryptoStream decryptedStream = new CryptoStream(base64DecodedStream, decryptor, CryptoStreamMode.Read);
        //    return decryptedStream;
        //}
        //private static string DecryptString(string cipherText)
        //{
        //    Aes aes = GetEncryptionAlgorithm();
        //    byte[] buffer = Convert.FromBase64String(cipherText);

        //    using MemoryStream memoryStream = new MemoryStream(buffer);
        //    using ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        //    using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        //    using StreamReader streamReader = new StreamReader(cryptoStream);
        //    return streamReader.ReadToEnd();
        //}
        //public async Task Invoke(HttpContext httpContext)
        //{
        //    httpContext.Response.Body = EncryptStream(httpContext.Response.Body);
        //    httpContext.Request.Body = DecryptStream(httpContext.Request.Body);
        //    if (httpContext.Request.QueryString.HasValue)
        //    {
        //        string decryptedString = DecryptString(httpContext.Request.QueryString.Value.Substring(1));
        //        httpContext.Request.QueryString = new QueryString($"?{decryptedString}");
        //    }
        //    await _next(httpContext);
        //    await httpContext.Request.Body.DisposeAsync();
        //    await httpContext.Response.Body.DisposeAsync();
        //}



        private readonly RequestDelegate _next;


        public EncryptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();


            if (context.Request.Body.CanSeek)
            {
                context.Request.Body.Position = 0;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    var body = await reader.ReadToEndAsync();

                    if (!string.IsNullOrEmpty(body))
                    {
                        var decryptedBody = EncryptionHelper.DecryptString(body);
                        var bytes = Encoding.UTF8.GetBytes(decryptedBody);
                        context.Request.Body = new MemoryStream(bytes);
                    }
                }
                context.Request.Body.Position = 0;
            }


            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context);


                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                var encryptedResponse = EncryptionHelper.EncryptString(responseBodyText);


                context.Response.Body = originalBodyStream;
                await context.Response.WriteAsJsonAsync(encryptedResponse);
            }
        }





    }
}
