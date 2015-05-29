using System;
using System.Collections.Generic;
using DataAccessLayer;
using Entities.Interfaces;

namespace Entities.Products
{
    public class HotelProduct : IProductOperation
    {
        /// <summary>
        /// for each supplier get success and failure rate
        /// </summary>
        /// <param name="suppliersList"></param>
        /// <returns></returns>
        public List<Supplier> GetFailureRateForSuppliers(List<Supplier> suppliersList)
        {
            foreach (var supplier in suppliersList)
            {
                //var obj = new SupplierDataManagerDataContext().spGetLogBasedOnCallType(supplier.CallType,supplier.SupplierId, 1000, supplier.SupplierName);
            }
            
            return null;
        }

        public bool DisableSupplier()
        {
            throw new NotImplementedException();
        }

        public bool CompareThreshhold()
        {
            throw new NotImplementedException();
        }
    }
}
