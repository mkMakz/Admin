using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.lib.Modules
{
    public class Provider
    {
        public List<int> Prefix = new List<int>();
        public string LigoURL { get; set; }
        public string NameCompany { get; set; }
        public double Procent { get; set; }        
    }
}
