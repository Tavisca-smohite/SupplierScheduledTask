using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Products;

namespace SupplierScheduledTask
{
    public class SupplierRelatedOperations
    {
        public Dictionary<Supplier,float> GetListOfSuppliersToDisablecs(Dictionary<string,List<Supplier>> productWiseSuppliersList )
        {
            var suppliersToDisable = new Dictionary<Supplier, float>();
            foreach (var productWiseSupplier in productWiseSuppliersList)
            {
                switch (productWiseSupplier.Key)
                {
                    case "Hotel":
                     
                     var suppliersWithFailureRate=   new HotelProduct().GetFailureRateForSuppliers(productWiseSupplier.Value);
                        var hotelSupplierstoDisable = new HotelProduct().CompareThreshhold(suppliersWithFailureRate);
                        suppliersToDisable = suppliersToDisable.Concat(hotelSupplierstoDisable).ToDictionary(x=>x.Key,x=>x.Value);
                     break;

                            case "Air":
                        var airSuppliersWithFailureRate=   new AirProduct().GetFailureRateForSuppliers(productWiseSupplier.Value);
                        var airSupplierstoDisable = new AirProduct().CompareThreshhold(airSuppliersWithFailureRate);
                        suppliersToDisable = suppliersToDisable.Concat(airSupplierstoDisable).ToDictionary(x=>x.Key,x=>x.Value);

                        break;

                        case "Car":
                        var carSuppliersWithFailureRate=   new CarProduct().GetFailureRateForSuppliers(productWiseSupplier.Value);
                        var carSupplierstoDisable = new CarProduct().CompareThreshhold(carSuppliersWithFailureRate);
                        suppliersToDisable = suppliersToDisable.Concat(carSupplierstoDisable).ToDictionary(x=>x.Key,x=>x.Value);
                        break;
                }
            }
            return suppliersToDisable;
        }
    }
}
