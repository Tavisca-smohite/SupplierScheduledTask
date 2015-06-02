using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using Entities.Interfaces;

namespace Entities.Products
{
    public class HotelProduct : IProductOperation
    {
        /// <summary>
        /// for each supplier get success and failure rate
        /// return dictinary with supplier and its failure rate
        /// </summary>
        /// <param name="suppliersList"></param>
        /// <returns></returns>
        public Dictionary<Supplier,float> GetFailureRateForSuppliers(List<Supplier> suppliersList)
        {
            //foreach (var supplier in suppliersList)
            //{
            //    var result = new SupplierDataManagerDataContext().spGetLogBasedOnCallType2(supplier.CallType,supplier.SupplierId, 1000, supplier.SupplierName);
            //    var supplierData = result.ToList();
            //}
            const int minutes = 1000;
            
            var getFailureStatResult = new List<spGetLogBasedOnCallTypeResult>();
            var supplierAndFailureRateMapping = new Dictionary<Supplier, float>();
            foreach (var supplier in suppliersList)
            {
                Supplier tempSupplier = supplier;
                SupplierDataManagerDBContext.UsingCommonContentDbRead(db =>
                    {
                        getFailureStatResult =
                            db.spGetLogBasedOnCallType(tempSupplier.CallType, tempSupplier.SupplierId, minutes,
                                                        tempSupplier.SupplierName).ToList();
                    });
                if (getFailureStatResult != null && getFailureStatResult.Count > 0)
                {
                    if(getFailureStatResult[0] !=null && getFailureStatResult[0].PerFailure!=null)
                    supplierAndFailureRateMapping.Add(supplier,float.Parse(getFailureStatResult[0].PerFailure));
                }
                
            }

            return supplierAndFailureRateMapping;
        }

        public Dictionary<Supplier,float> CompareThreshhold(Dictionary<Supplier,float> supplierAndFailureRateMapping )
        {
           var suppliersWhoCrossedThreshhold=new Dictionary<Supplier, float>();
            foreach (var mapping in supplierAndFailureRateMapping)
            {
                var supplier = mapping.Key;
                if(supplier.ThreshholdValue<=mapping.Value)
                {
                    suppliersWhoCrossedThreshhold.Add(supplier,mapping.Value);
                }

            }
            return suppliersWhoCrossedThreshhold;
        }
    }
}