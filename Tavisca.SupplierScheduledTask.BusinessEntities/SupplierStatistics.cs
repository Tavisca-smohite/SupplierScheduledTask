using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.SupplierScheduledTask.BusinessEntities
{
    public  class SupplierStatistics
    {
        public double SuccessRate { get; set; }
        public double FailureRate { get; set; }
        public double TotalRate { get; set; }
        public int IsEnabled { get; set; }
        public int TotalCallsCount { get; set; }
        public int TotalSuccessfulCallsCount { get; set; } //total successful calls among total calls
        public int TotalFailureCallsCount { get; set; }  //total failure calls among total calls
    }
}
