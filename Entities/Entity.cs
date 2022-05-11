using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitAI.NET.Entities
{
    public class Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsCustom { get; set; }

        public string[] Roles { get; set; }
        public string[] Lookups { get; set; }
        public object[] Keywords { get; set; }
    }
}