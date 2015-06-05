using System;
using System.Collections.Generic;
using System.Linq;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.TravelNxt.Shared.Entities.Infrastructure;

namespace Tavisca.SupplierScheduledTask.DataAccessLayer
{
    public class SupplierLogRepository : ISupplierLogRepository
    {
        #region ISupplierLogRepository Members

        public SupplierStatistics GetFailureLogs(Supplier supplier, int minutes)
        {
            var supplierStatistics=new SupplierStatistics();
            var getFailureStatResult = new List<spGetLogBasedOnCallTypeResult>();                    
            try
            {
                SupplierDataManagerDBContext.UsingCommonContentDbRead(db =>
                    {
                        getFailureStatResult =
                            db.spGetLogBasedOnCallType(supplier.CallType, supplier.SupplierId, minutes,
                                                       supplier.SupplierName).ToList();
                    });                                             
            }               

            catch (Exception exception)
            {
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");
            }

            supplierStatistics = ParseLogBasedOnCallTypeResult(getFailureStatResult);
            return supplierStatistics;
        }

        private static SupplierStatistics ParseLogBasedOnCallTypeResult(List<spGetLogBasedOnCallTypeResult> getFailureStatResult)
        {
            
            var supplierStats = new SupplierStatistics();
            float perFailureRate = 0, perSuccessRate = 0;
            int isEnabled=0;
            if (getFailureStatResult != null && getFailureStatResult.Count > 0)
            {

                foreach (var result in getFailureStatResult)
                {

                    perFailureRate = (result.PerFailure != null) ? float.Parse(result.PerFailure) : 0;
                    perSuccessRate = (result.PerSuccess != null) ? float.Parse(result.PerSuccess) : 0;
                    isEnabled = result.IsEnabled;
                }
            }
            supplierStats.FailureRate = perFailureRate;
            supplierStats.SuccessRate = perSuccessRate;
            supplierStats.TotalRate = perFailureRate + perSuccessRate;
            supplierStats.IsEnabled = isEnabled;


            return supplierStats;
        }

        #endregion
    }
}