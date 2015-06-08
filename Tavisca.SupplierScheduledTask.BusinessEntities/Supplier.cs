using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.SupplierScheduledTask.BusinessEntities
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string ProductType { get; set; }
        public string CallType { get; set; }
        public float ThreshholdValue { get; set; }
        public int DisableIfCrossesThreshhold { get; set; }
        public int TotalCallsCount { get; set; } //total calls made with distinct session ids
        public bool IsDisabled { get; set; }
    }
}
