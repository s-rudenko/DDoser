using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDoser.Infrastructure.QueryString.Modifiers
{
    interface IModifier
    {
        public string Modify(string url);
    }
}
