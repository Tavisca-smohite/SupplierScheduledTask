using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Interfaces
{
    public interface IProductOperation
    {
         Dictionary<Supplier,float> GetFailureRateForSuppliers(List<Supplier> suppliers);
      
        Dictionary<Supplier,float> CompareThreshhold(Dictionary<Supplier,float> suppliers);
    }
}
