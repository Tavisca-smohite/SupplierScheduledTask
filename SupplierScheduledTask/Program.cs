using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierScheduledTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new XmlParser().ParseSuppliers();
        }
    }
}
