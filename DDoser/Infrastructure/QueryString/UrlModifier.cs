using DDoser.Infrastructure.QueryString.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDoser.Infrastructure.QueryString
{
    public class UrlModifier
    {
        private static readonly List<IModifier> modifiers = new List<IModifier>
        {
            new RandomStringHelper(),
            new RandomEmailHelper(),
        };

        public static string ModifyUrl(string url)
        {
            foreach(var modifier in modifiers)
            {
               url = modifier.Modify(url);
            }
            return url;
        }
    }
}
