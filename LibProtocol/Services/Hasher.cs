using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibProtocol.Services
{
    public class Hasher
    {
        public String heshString(string message)
        {
            using (var algo = MD5.Create())
            {
                byte[] hesh = algo.ComputeHash(Encoding.Unicode.GetBytes(message));
                var sb = new StringBuilder();
                foreach (var h in hesh)
                {
                    sb.Append(h.ToString());
                }
                return sb.ToString();
            }
        }
    }
}
