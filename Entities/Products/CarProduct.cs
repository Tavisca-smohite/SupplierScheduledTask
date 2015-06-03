using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Interfaces;

namespace Entities.Products
{
   public  class CarProduct :IProductOperation
    {
        public Dictionary<Supplier, float> GetFailureRateForSuppliers(List<Supplier> suppliers)
        {
            throw new NotImplementedException();
        }        
    }
}
