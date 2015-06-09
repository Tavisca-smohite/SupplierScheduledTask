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
        public bool DisableSupplier(int supplierId)
        {
            var isDisabled = false;
            try
            {
                SupplierDataManagerDBContext.UsingCommonContentDbRead(db =>
                {
                    //TODO:disable supplier and get status 
                    //var status                        
                });
                // isDisabled = (statusFlag == 0) ? true : false; //if return value is 0 ; that indicates supplier is disabled.
            }

            catch (Exception exception)
            {
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");
            }
            return isDisabled;

        }

        public bool EnableSupplier(int supplierId)
        {
            throw new NotImplementedException();
        }
    }
}
