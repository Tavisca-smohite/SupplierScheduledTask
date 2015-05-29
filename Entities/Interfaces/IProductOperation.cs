using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Interfaces
{
    public interface IProductOperation
    {
         List<Supplier> GetFailureRateForSuppliers(List<Supplier> suppliers);
        bool DisableSupplier();
        bool CompareThreshhold();
    }
}
