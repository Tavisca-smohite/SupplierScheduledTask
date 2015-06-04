using System.Collections.Generic;
using Tavisca.Singularity;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.DataAccessLayer;
using Tavisca.SupplierScheduledTask.DataAccessLayer.Repository;

namespace Tavisca.SupplierScheduledTask.BusinessLogic.ProductSuppliersStrategy
{
    public class CarProductSupplierStrategy : IProductSupplier
    {
         public CarProductSupplierStrategy()
        {
            _supplierRepository = RuntimeContext.Resolver.Resolve<ISupplierLogRepository>("SupplierLogRepository");
        }

        private ISupplierLogRepository _supplierRepository;
        #region IProductSupplier Members

        public Dictionary<Supplier, float> GetFailureRateForProductSuppliers(List<Supplier> suppliersList)
        {
            const int minutes = 1000;

            var supplierAndFailureRateMapping = new Dictionary<Supplier, float>();
            foreach (Supplier supplier in suppliersList)
            {
                var supplierStats = _supplierRepository.GetFailureLogs(supplier, minutes);
                supplierAndFailureRateMapping.Add(supplier, supplierStats.FailureRate);
            }

            return supplierAndFailureRateMapping;
        }

        #endregion

       
    }
}