using System.Collections.Generic;
using System.Globalization;
using Tavisca.Singularity;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.DataAccessLayer;

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

        public Dictionary<Supplier, string> GetFailureRateForProductSuppliers(List<Supplier> suppliersList)
        {
            const int minutes = 1000;

            var supplierAndFailureRateMapping = new Dictionary<Supplier, string>();
            foreach (Supplier supplier in suppliersList)
            {
                var supplierStats = _supplierRepository.GetFailureLogs(supplier, minutes);
                supplierAndFailureRateMapping.Add(supplier, (supplierStats.TotalRate != 100) ? string.Empty : supplierStats.FailureRate.ToString(CultureInfo.InvariantCulture));
            }

            return supplierAndFailureRateMapping;
        }

        #endregion

       
    }
}