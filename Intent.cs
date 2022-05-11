using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitAI.NET.Intents
{
    public class Intent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Confidence { get; set; }
        public bool IsCustom { get; set; }
    }
}
