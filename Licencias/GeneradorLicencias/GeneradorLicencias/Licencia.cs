using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorLicencias
{
    public class Licencia
    {
        public string Cliente { get; set; }
        public string Plan { get; set; }
        public int CantidadProveedores { get; set; }

        private readonly string password = "bc086251-1159-4f90-ba6b-7cba9e0502b5";

        public string GenerarLicencia()
        {
            string plainText = $"{Cliente}-{Plan}-{CantidadProveedores}";
            
            byte[] salt = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            int iterations = 10000;
            byte[] keyBytes;
            byte[] ivBytes;

            using (var aes = Aes.Create())
            {
                var keyGenerator = new Rfc2898DeriveBytes(password, salt, iterations);
                keyBytes = keyGenerator.GetBytes(aes.KeySize / 8);
                ivBytes = keyGenerator.GetBytes(aes.BlockSize / 8);

                using (var encryptor = aes.CreateEncryptor(keyBytes, ivBytes))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (var streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }
                        }

                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        public string DesencriptarLicencia(string licenciaEncriptada)
        {
            
            byte[] salt = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            int iterations = 10000;
            byte[] keyBytes;
            byte[] ivBytes;

            using (var aes = Aes.Create())
            {
                var keyGenerator = new Rfc2898DeriveBytes(password, salt, iterations);
                keyBytes = keyGenerator.GetBytes(aes.KeySize / 8);
                ivBytes = keyGenerator.GetBytes(aes.BlockSize / 8);

                using (var decryptor = aes.CreateDecryptor(keyBytes, ivBytes))
                {
                    using (var memoryStream = new MemoryStream(Convert.FromBase64String(licenciaEncriptada)))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (var streamReader = new StreamReader(cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}
