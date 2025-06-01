namespace ControlGastos.Models
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class Seguridad
    {
        public static string EncriptarMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}