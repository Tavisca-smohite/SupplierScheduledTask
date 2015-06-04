using System.Collections.Generic;
using Tavisca.SupplierScheduledTask.BusinessEntities;

namespace Tavisca.SupplierScheduledTask.BusinessLogic
{
    //TODO: Need to change interface name with proper meaning full name
    public interface IProductSupplier
    {
         Dictionary<Supplier,float> GetFailureRateForProductSuppliers(List<Supplier> suppliers);             
    }
}
