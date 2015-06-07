using System.Collections.Generic;
using System.Globalization;
using Tavisca.Singularity;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.DataAccessLayer;

namespace Tavisca.SupplierScheduledTask.BusinessLogic.ProductSuppliersStrategy
{
    public class AirProductSupplierStrategy : IProductSupplier
    {
        public AirProductSupplierStrategy()
        {
            _supplierRepository = RuntimeContext.Resolver.Resolve<ISupplierLogRepository>("SupplierLogRepository");
        }

        public AirProductSupplierStrategy(ISupplierLogRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public static ISupplierLogRepository _supplierRepository;

        #region IProductSupplier Members

        public Dictionary<Supplier, string> GetFailureRateForProductSuppliers(List<Supplier> suppliersList)
        {
           
            const int minutes = 1000;
            var supplierAndFailureRateMapping = new Dictionary<Supplier, string>();

            foreach (Supplier supplier in suppliersList)
            {
                
                var supplierStats = _supplierRepository.GetFailureLogs(supplier, minutes);
                if(supplierStats.IsEnabled==1)
                    supplierAndFailureRateMapping.Add(supplier, (supplierStats.TotalRate != 100) ? string.Empty : supplierStats.FailureRate.ToString(CultureInfo.InvariantCulture));//add if total rate is 100 
            }

            return supplierAndFailureRateMapping;
        }
       

        #endregion

       
    }
}