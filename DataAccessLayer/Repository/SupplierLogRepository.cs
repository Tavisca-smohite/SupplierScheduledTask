using System;
using System.Collections.Generic;
using System.Linq;
using Tavisca.SupplierScheduledTask.BusinessEntities;

namespace Tavisca.SupplierScheduledTask.DataAccessLayer.Repository
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


                supplierStatistics = ParseLogBasedOnCallTypeResult(getFailureStatResult);

                //if (getFailureStatResult[0] != null && getFailureStatResult[0].PerFailure != null)
                    //    failureRate = float.Parse(getFailureStatResult[0].PerFailure);
                    //if (getFailureStatResult[0] != null && getFailureStatResult[0].PerSuccess != null)
                    //    successRate = float.Parse(getFailureStatResult[0].PerSuccess);                
            }
            catch (Exception exception)
            {               

            }

            return supplierStatistics;
        }

        private static SupplierStatistics ParseLogBasedOnCallTypeResult(List<spGetLogBasedOnCallTypeResult> getFailureStatResult)
        {
            
            var supplierStats = new SupplierStatistics();
            float perFailureRate = 0, perSuccessRate = 0;
            if (getFailureStatResult != null && getFailureStatResult.Count > 0)
            {
                
                foreach (var result in getFailureStatResult)
                {
                   
                    perFailureRate=(result.PerFailure!=null)?float.Parse(result.PerFailure):0;
                    perSuccessRate = (result.PerSuccess != null) ? float.Parse(result.PerSuccess) :0;                    
                }
            }
            supplierStats.FailureRate = perFailureRate;
            supplierStats.SuccessRate = perSuccessRate;
            supplierStats.TotalRate = perFailureRate + perSuccessRate;


            return supplierStats;
        }

        #endregion
    }
}