using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace WitAI.NET
{
    public abstract class Base
    {
        internal HttpClient Client { get; }

        internal Base()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri("https://api.wit.ai")
            };

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", WitClient.Token);
        }
    }
}
