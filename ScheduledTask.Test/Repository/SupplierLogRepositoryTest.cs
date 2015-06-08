using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.DataAccessLayer;
using Moq;
using Tavisca.SupplierScheduledTask.BusinessLogic;

namespace ScheduledTask.Test
{
    [TestClass]
    public class SupplierLogRepositoryTest
    {
        [TestMethod]
        public void GetFailureLogs_Successful_WithValidInput()
        {
            var supplierLogRepository = new SupplierLogRepository();
            
            var rate = supplierLogRepository.GetFailureLogs(GetSupplierObject("HotelMultiAvail", 9, "Pegasus", "Hotel"), 1000);
            Assert.AreEqual(100, rate.TotalRate, "total success and failure Rate must be equal to 100%");
        }

        private Tavisca.SupplierScheduledTask.BusinessEntities.Supplier GetSupplierObject(string callType,int id,string name,string productType)
        {
            return new Supplier()
                {
                    CallType =callType,SupplierId = id,SupplierName = name,ProductType = productType
                };
        }


        [TestMethod]
        public void GetFailureLogs_Failed_WithInvalidInput()
        {

            var supplierLogRepository = new SupplierLogRepository();

            var rate = supplierLogRepository.GetFailureLogs(GetSupplierObject("AirMultiAvail", 9, "Pegasus", "Hotel"), 1000);
            Assert.AreEqual(0, rate.TotalRate, "total rate must be 0 for invalid inputs");
        }


        [TestMethod]
        public void GetFailureLogs_ThowsException_WithInvalidInput()
        {

            var supplierLogRepository = new SupplierLogRepository();

            var rate = supplierLogRepository.GetFailureLogs(new Supplier(), 1000);//throws sql exception
            Assert.AreEqual(0, rate.TotalRate, "total rate must be 0 for invalid inputs");
            Assert.AreEqual(0, rate.IsEnabled, "total rate must be 0 for invalid inputs");
            Assert.AreEqual(0, rate.SuccessRate, "total rate must be 0 for invalid inputs");
            Assert.AreEqual(0, rate.FailureRate, "total rate must be 0 for invalid inputs");
            
        }
    }
}
