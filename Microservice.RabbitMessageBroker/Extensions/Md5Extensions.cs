using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Microservice.RabbitMessageBroker.Extensions
{
    public static class Md5Extensions
    {
        public static string ToMd5Hash(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return (string)null;
            return Encoding.ASCII.GetBytes(str).ToMd5Hash();
        }

        public static string ToMd5Hash(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return (string)null;

            using var md5 = MD5.Create();
            return string.Join("", md5.ComputeHash(bytes).Select(x => x.ToString("X2")));
        }
    }
}
