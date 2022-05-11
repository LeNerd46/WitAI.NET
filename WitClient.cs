using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitAI.NET
{
    public class WitClient
    {
        internal static string Token { get; private set; }

        public WitClient(string token)
        {
            Token = token;
        }
    }
}
