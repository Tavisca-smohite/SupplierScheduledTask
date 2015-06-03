using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Interfaces
{
    //TODO: Need to change interface name with proper meaning full name
    public interface IProductOperation
    {
         Dictionary<Supplier,float> GetFailureRateForSuppliers(List<Supplier> suppliers);             
    }
}
