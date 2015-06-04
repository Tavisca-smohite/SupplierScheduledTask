using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.DataAccessLayer.Repository;

namespace ScheduledTask.Test
{
    [TestClass]
    public class SupplierLogRepositoryTest
    {
        [TestMethod]
        public void GetFailureLogs_Successful_WithValidInput()
        {
           // var supplierLogRepository=new SupplierLogRepository();
           // float successRate;
           // var failureRate = supplierLogRepository.GetFailureLogs(GetSupplierObject("HotelMultiAvail",9,"Pegasus","Hotel"), 1000, out successRate);
           //Assert.AreEqual(100,successRate+failureRate,"total success and failure Rate must be equal to 100%");
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
            //var supplierLogRepository = new SupplierLogRepository();
            //float successRate;
            //var failureRate = supplierLogRepository.GetFailureLogs(GetSupplierObject("AirMultiAvail",9,"Pegasus","Hotel"), 1000, out successRate);
            //Assert.AreEqual(0, successRate + failureRate, "total rate must be 0 for invalid inputs");
        }
    }
}
