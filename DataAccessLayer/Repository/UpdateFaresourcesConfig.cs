using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.TravelNxt.Shared.Entities.Infrastructure;

namespace Tavisca.SupplierScheduledTask.DataAccessLayer
{
    public class UpdateFaresourcesConfig : IUpdateFaresourcesConfig
    {
        public bool DisableSupplier(int? supplierId)
        {
            int? status = 0; 
            try
            {                 
                SupplierConfigUpdateManagerDBContext.UsingCommonContentDbRead(db =>
                    {                                            
                          status=  db.spDisableSupplierWhoHasCrossedThreshhold(supplierId,ref status);
                        //disable supplier and get status 
                                              
                    });
                 //if return value is 1 ; that indicates supplier is disabled. 
                if (status == 0) //for invalid parameters it will return value 0
                    throw new Exception("Parameters passed to sp are invalid");
            }
            
            catch (Exception exception)
            {
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");
            }
            bool isDisabled = (status == 1);
            return isDisabled;

        }

        public bool EnableSupplier(int supplierId)
        {
            int? status = 0;
            try
            {
                
                SupplierConfigUpdateManagerDBContext.UsingCommonContentDbRead(db =>
                {
                    status = db.spEnableSupplier(supplierId, ref status);
                    //disable supplier and get status 

                });
                if (status == 0) //for invalid parameters it will return value 0
                    throw new Exception("Parameters passed to sp are invalid");
                 //if return value is 1 ; that indicates supplier is enabled.               
            }
            
            catch (Exception exception)
            {
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");
            }
            bool isEnabled = (status == 1);
            return isEnabled;
        }
    }
}
