using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Supplier 
    {     
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string ProductType { get; set; }
        public string CallType { get; set; }
        public int ThreshholdValue { get; set; }
        public int FailureRate { get; set; }
    }

    
}
