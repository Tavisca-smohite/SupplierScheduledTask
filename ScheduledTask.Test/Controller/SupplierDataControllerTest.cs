using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.BusinessLogic;
using Tavisca.SupplierScheduledTask.DataAccessLayer;


namespace ScheduledTask.Test
{
    [TestClass]
    public class SupplierDataControllerTest
    {

        [TestMethod]
        public  void GetLitOfSuppliersToDisable_Successful_withValidInputs()
        {
            Dictionary<string, List<Supplier>> productWiseSuppliersList = new SupplierDataHelper().GetProductWiseSuppliersList();
            var suppliersToDisable = new SupplierDataController().GetListOfSuppliersToDisable(productWiseSuppliersList);
            Assert.IsNotNull(suppliersToDisable);
        }



        [TestMethod]
        public void Invoke_Successful_TaskSchedulerLogic()
        {
            new SupplierDataController().Invoke();
        }       

    }
}
