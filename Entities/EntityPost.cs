using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitAI.NET.Entities
{
    internal class EntityPost
    {
        public string Name { get; set; }
        public string[] Roles { get; set; }
        public string[] Lookups { get; set; }
        public object[] Keywords { get; set; }

        public EntityPost(string name, string[] roles, string[] lookups = null, object[] keywords = null)
        {
            Name = name;
            Roles = roles;
            Lookups = lookups ?? Array.Empty<string>();
            Keywords = keywords ?? Array.Empty<object>();
        }
    }
}
