using System.Collections.Generic;
using System.Globalization;
using Tavisca.Singularity;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.DataAccessLayer;

namespace Tavisca.SupplierScheduledTask.BusinessLogic
{
    public class HotelProductSupplierStrategy : IProductSupplier
    {
         public HotelProductSupplierStrategy()
        {
            _supplierRepository = RuntimeContext.Resolver.Resolve<ISupplierLogRepository>("SupplierLogRepository");
        }

         public HotelProductSupplierStrategy(ISupplierLogRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        private ISupplierLogRepository _supplierRepository;
        #region IProductSupplier Members

        /// <summary>
        /// for each supplier get success and failure rate
        /// return dictinary with supplier and its failure rate
        /// </summary>
        /// <param name="suppliersList"></param>
        /// <returns></returns>
        public Dictionary<Supplier, string> GetFailureRateForProductSuppliers(List<Supplier> suppliersList)
        {
            const int minutes = 1000;

            var supplierAndFailureRateMapping = new Dictionary<Supplier, string>();

            foreach (var supplier in suppliersList)
            {
                //TODO: throw exception if sucess + failure rate is not 100
                var supplierStats = _supplierRepository.GetFailureLogs(supplier, minutes);
                if (supplierStats.IsEnabled == 1)
                {
                    var failureRate = (supplierStats.TotalRate<100)? string.Empty: supplierStats.FailureRate.ToString(CultureInfo.InvariantCulture);
                    supplier.TotalCallsCount = supplierStats.TotalCallsCount;
                    supplierAndFailureRateMapping.Add(supplier,failureRate);
                }
                
            }

            return supplierAndFailureRateMapping;
        }

        #endregion

    
    }
}