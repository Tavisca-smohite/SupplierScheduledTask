using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.SupplierScheduledTask.BusinessEntities
{
    public  class SupplierStatistics
    {
        public float SuccessRate { get; set; }
        public float FailureRate { get; set; }
        public float TotalRate { get; set; }
        public int IsEnabled { get; set; }
        public int TotalCallsCount { get; set; }
    }
}
