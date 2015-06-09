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
            bool isDisabled;
            int? status = 1; 
            try
            {                 
                SupplierConfigUpdateManagerDBContext.UsingCommonContentDbRead(db =>
                    {                                            
                          status=  db.spDisableSupplierWhoHasCrossedThreshhold(supplierId,ref status);
                        //disable supplier and get status 
                                              
                    });
                 //if return value is 0 ; that indicates supplier is disabled. 
                if (status == 2) //for invalid parameters it will return value 2 (other than 0 or 1)
                    throw new Exception("Parameters passed to sp are invalid");
            }
            
            catch (Exception exception)
            {
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");
            }
            isDisabled = (status == 0);
            return isDisabled;

        }

        public bool EnableSupplier(int supplierId)
        {
            bool isEnabled ;
            int? status = 0;
            try
            {
                
                SupplierConfigUpdateManagerDBContext.UsingCommonContentDbRead(db =>
                {
                    status = db.spEnableSupplier(supplierId, ref status);
                    //disable supplier and get status 

                });
                if (status == 2) //for invalid parameters it will return value 2 (other than 0 or 1)
                    throw new Exception("Parameters passed to sp are invalid");
                 //if return value is 1 ; that indicates supplier is enabled.               
            }
            
            catch (Exception exception)
            {
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");
            }
            isEnabled = (status == 1);
            return isEnabled;
        }
    }
}
