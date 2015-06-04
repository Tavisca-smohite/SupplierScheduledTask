using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.SupplierScheduledTask.BusinessEntities;

namespace Tavisca.SupplierScheduledTask.DataAccessLayer
{
    public interface ISupplierLogRepository
    {
        SupplierStatistics GetFailureLogs(Supplier supplier, int minutes);
    }
}
