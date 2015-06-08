using System.Collections.Generic;
using System.Globalization;
using Tavisca.Singularity;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.DataAccessLayer;

namespace Tavisca.SupplierScheduledTask.BusinessLogic
{
    public class CarProductSupplierStrategy : IProductSupplier
    {
         public CarProductSupplierStrategy()
        {
            _supplierRepository = RuntimeContext.Resolver.Resolve<ISupplierLogRepository>("SupplierLogRepository");
        }

         public CarProductSupplierStrategy(ISupplierLogRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
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
                if (supplierStats.IsEnabled == 1)
                {
                    var failureRate = (supplierStats.TotalRate < 100) ? string.Empty : supplierStats.FailureRate.ToString(CultureInfo.InvariantCulture);
                    supplier.TotalCallsCount = supplierStats.TotalCallsCount;
                    supplierAndFailureRateMapping.Add(supplier, failureRate);
                }    
            }

            return supplierAndFailureRateMapping;
        }

        #endregion

       
    }
}