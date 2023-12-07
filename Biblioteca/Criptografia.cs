using System.Text;
using System.Security.Cryptography;

namespace LocFarma.Biblioteca
{
    public class Criptografia
    {
        public static string CriptografarSenhaSHA256(string senha)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Converte a senha para um array de bytes
                byte[] bytes = Encoding.UTF8.GetBytes(senha);

                // Calcula o hash SHA-256
                byte[] hash = sha256.ComputeHash(bytes);

                // Converte o hash de volta para uma string hexadecimal
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    stringBuilder.Append(hash[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}
