using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Interfaces;
using Entities.Products;
using Notifications;

namespace SupplierScheduledTask
{

    //TODO: Need to move this logic in Business 
    public class SupplierRelatedOperations
    {

        private static readonly Dictionary<string, IProductOperation> ProductOperation = new Dictionary<string, IProductOperation>();
        public SupplierRelatedOperations()
        {
            //TODO: Name of AirProduct, HotelProduct, CarProduct shoud be changed to proper class names.

            ProductOperation.Add("Air", new AirProduct());
            ProductOperation.Add("Hotel", new HotelProduct());
            ProductOperation.Add("Car", new CarProduct());
        }

        public void Invoke()
        {
            try
            {
                var productWiseSuppliersList = new XmlParser().GetProductWiseSuppliersList();
                var suppliersToDisable = GetListOfSuppliersToDisable(productWiseSuppliersList);
                DisableSuppliers(suppliersToDisable);
                bool isMailSend=new SendNotificationMail().SendNotificationEmail(suppliersToDisable);
                EnableSuppliers();
            }
            catch (Exception)
            {
                
                //TODO: ignore or log
            }
           
        }

        public Dictionary<Supplier,float> GetListOfSuppliersToDisable(Dictionary<string,List<Supplier>> productWiseSuppliersList )
        {
            var suppliersToDisable = new Dictionary<Supplier, float>();
            foreach (var productWiseSupplier in productWiseSuppliersList)
            {
                var suppliersWithFailureRate = ProductOperation[productWiseSupplier.Key].GetFailureRateForSuppliers(productWiseSupplier.Value);
                var supplierstoDisable = CompareThreshhold(suppliersWithFailureRate);
                suppliersToDisable = suppliersToDisable.Concat(supplierstoDisable).ToDictionary(x => x.Key, x => x.Value);

                //switch (productWiseSupplier.Key)
                //{
                //    case "Hotel":                        
                //     var suppliersWithFailureRate=   new HotelProduct().GetFailureRateForSuppliers(productWiseSupplier.Value);
                //        var hotelSupplierstoDisable = new HotelProduct().CompareThreshhold(suppliersWithFailureRate);
                //        suppliersToDisable = suppliersToDisable.Concat(hotelSupplierstoDisable).ToDictionary(x=>x.Key,x=>x.Value);
                //     break;

                //            case "Air":
                //        var airSuppliersWithFailureRate=   new AirProduct().GetFailureRateForSuppliers(productWiseSupplier.Value);
                //        var airSupplierstoDisable = new AirProduct().CompareThreshhold(airSuppliersWithFailureRate);
                //        suppliersToDisable = suppliersToDisable.Concat(airSupplierstoDisable).ToDictionary(x=>x.Key,x=>x.Value);

                //        break;

                //        case "Car":
                //        var carSuppliersWithFailureRate=   new CarProduct().GetFailureRateForSuppliers(productWiseSupplier.Value);
                //        var carSupplierstoDisable = new CarProduct().CompareThreshhold(carSuppliersWithFailureRate);
                //        suppliersToDisable = suppliersToDisable.Concat(carSupplierstoDisable).ToDictionary(x=>x.Key,x=>x.Value);
                //        break;
                //}
            }
            return suppliersToDisable;
        }

        private bool DisableSuppliers(Dictionary<Supplier,float> suppliersToDisable )
        {
            try
            {
                foreach (var supplierToDisable in suppliersToDisable)
                {
                    var supplier = supplierToDisable.Key;
                    if (supplier.DisableIfCrossesThreshhold == 1)
                    {
                        //TODO: call sp to disable supplier and save record of disabled supplier


                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }

        //TODO: If we are using methods within the same call then they shoud be private methods
        public void EnableSuppliers()
        {
            //TODO: enable supplier which has been disabled for more than half an hour
        }
        
        private Dictionary<Supplier, float> CompareThreshhold(Dictionary<Supplier, float> supplierAndFailureRateMapping)
        {
            var suppliersWhoCrossedThreshhold = new Dictionary<Supplier, float>();
            foreach (var mapping in supplierAndFailureRateMapping)
            {
                var supplier = mapping.Key;
                if (supplier.ThreshholdValue <= mapping.Value)
                {
                    suppliersWhoCrossedThreshhold.Add(supplier, mapping.Value);
                }

            }
            return suppliersWhoCrossedThreshhold;
        }
    }


 
}
