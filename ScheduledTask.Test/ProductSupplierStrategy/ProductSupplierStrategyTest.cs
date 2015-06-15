using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.DataAccessLayer;
using Tavisca.SupplierScheduledTask.BusinessLogic;
using Tavisca.SupplierScheduledTask.Notifications;

namespace ScheduledTask.Test.ProductSupplierStrategy
{
    /// <summary>
    /// test behaviour of product strategy classes when GetFailureLogs function
    /// returns valid or invalid objects
    /// failure logs function returns empty initialised object in case of sql exception or any internal error
    /// calling function should validate if supplier passed was enabled and if so, total rate (sucess + failure should be 100)
    /// In case rate is not 100 % even though supplier was enabled it should add string.empty as a failure rate for later handling
    /// of SuppliersToDisable.
    /// </summary>
    /// 
    [TestClass]
    public class ProductSupplierStrategyTest
    {
        private int minutes=Convert.ToInt32(Configuration.TimeDiffInMinutes);

        //when list contains all valid stats for enabled suppliers
        [TestMethod]
        public void GetFailureRateOfAirProductSuppliers_WithValidExecution_OfFailureLogsFunction()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), minutes)).Returns(new SupplierStatistics { FailureRate = 60, SuccessRate = 40, TotalRate = 100, IsEnabled = 1 ,TotalCallsCount=30}).Returns(new SupplierStatistics { FailureRate = 50, SuccessRate = 50, TotalRate = 100, IsEnabled = 1,TotalCallsCount=40 });           
            var suppliersTodisable = new AirProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "Mystifly" } });
            Assert.AreEqual(2, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
            foreach (var supplierToDisable in suppliersTodisable)
            {
               // Assert.IsTrue(!string.Equals(supplierToDisable.Value, string.Empty), " value should not be empty for invalid stats with enabled suppliers");
                Assert.AreNotEqual(string.Empty, supplierToDisable.Value, " value should not be empty for invalid stats with enabled suppliers");
                Assert.AreNotEqual(0, supplierToDisable.Key.TotalCallsCount, "totalcall count must not be 0");
            }
        }


        //when list contains empty objects Case: with handled sql exception
        [TestMethod]
        public void GetFailureRateOfAirProductSuppliers_WithInValid_ExecutionOfFailureLogsFunction()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), minutes)).Returns(new SupplierStatistics()).Returns(new SupplierStatistics());            
            var suppliersTodisable = new AirProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "WorldSpan" } });
            Assert.AreEqual(0, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
        }

        //when list contains all invalid stats for enabled suppliers
        [TestMethod]
        public void GetFailureRateOfAirProductSuppliers_WithInValid_ExecutionOfFailureLogsFunctionForEnabledSuppliers()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), minutes)).Returns(new SupplierStatistics { FailureRate = 0, SuccessRate = 0, TotalRate = 0,TotalCallsCount=0, IsEnabled = 1 }).Returns(new SupplierStatistics { FailureRate = 0, SuccessRate = 0, TotalRate = 0, IsEnabled = 1,TotalCallsCount=0 });
            var suppliersTodisable = new AirProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "WorldSpan" } });
            Assert.AreEqual(2, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
            foreach (var supplierToDisable in suppliersTodisable)
            {
              //  Assert.IsTrue(string.Equals(supplierToDisable.Value, string.Empty), " value should be empty for invalid stats with enabled suppliers");
                Assert.AreEqual(string.Empty, supplierToDisable.Value, " value should not be empty for invalid stats with enabled suppliers");
                Assert.AreEqual(0, supplierToDisable.Key.TotalCallsCount, "totalcall count must be 0");
            }
        
        }



        //when list contains all valid stats for enabled suppliers
        [TestMethod]
        public void GetFailureRateOfHotelProductSuppliers_WithValidExecution_OfFailureLogsFunction()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), minutes)).Returns(new SupplierStatistics { FailureRate = 60, SuccessRate = 40, TotalRate = 100, IsEnabled = 1,TotalCallsCount=30 }).Returns(new SupplierStatistics { FailureRate = 50, SuccessRate = 50, TotalRate = 100, IsEnabled = 1 ,TotalCallsCount=40});
            var suppliersTodisable = new HotelProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "Mystifly" } });
            Assert.AreEqual(2, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
            foreach (var supplierToDisable in suppliersTodisable)
            {
                // Assert.IsTrue(!string.Equals(supplierToDisable.Value, string.Empty), " value should not be empty for invalid stats with enabled suppliers");
                Assert.AreNotEqual(string.Empty, supplierToDisable.Value, " value should not be empty for invalid stats with enabled suppliers");
                Assert.AreNotEqual(0, supplierToDisable.Key.TotalCallsCount, "totalcall count must not be 0");
            }
        }


        //when list contains empty objects Case: with handled sql exception
        [TestMethod]
        public void GetFailureRateOfHotelProductSuppliers_WithInValid_ExecutionOfFailureLogsFunction()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), minutes)).Returns(new SupplierStatistics()).Returns(new SupplierStatistics());
            var suppliersTodisable = new HotelProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "WorldSpan" } });
            Assert.AreEqual(0, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
        }

        //when list contains all invalid stats for enabled suppliers
        [TestMethod]
        public void GetFailureRateOfHotelProductSuppliers_WithInValid_ExecutionOfFailureLogsFunction_ForEnabledSuppliers()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), minutes)).Returns(new SupplierStatistics { FailureRate = 0, SuccessRate = 0, TotalRate = 0, IsEnabled = 1 }).Returns(new SupplierStatistics { FailureRate = 0, SuccessRate = 0, TotalRate = 0, IsEnabled = 1 });
            var suppliersTodisable = new HotelProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "WorldSpan" } });
            Assert.AreEqual(2, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
            foreach (var supplierToDisable in suppliersTodisable)
            {
                //  Assert.IsTrue(string.Equals(supplierToDisable.Value, string.Empty), " value should be empty for invalid stats with enabled suppliers");
                Assert.AreEqual(string.Empty, supplierToDisable.Value, " value should not be empty for invalid stats with enabled suppliers");
                Assert.AreEqual(0, supplierToDisable.Key.TotalCallsCount, "totalcall count must be 0");
            }

        }

        //when list contains all valid stats for enabled suppliers
        [TestMethod]
        public void GetFailureRateOfCarProductSuppliers_WithValidExecution_OfFailureLogsFunction()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), minutes)).Returns(new SupplierStatistics { FailureRate = 60, SuccessRate = 40, TotalRate = 100, IsEnabled = 1,TotalCallsCount=35 }).Returns(new SupplierStatistics { FailureRate = 50, SuccessRate = 50, TotalRate = 100, IsEnabled = 1 ,TotalCallsCount=20});
            var suppliersTodisable = new CarProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "Mystifly" } });
            Assert.AreEqual(2, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
            foreach (var supplierToDisable in suppliersTodisable)
            {
                // Assert.IsTrue(!string.Equals(supplierToDisable.Value, string.Empty), " value should not be empty for invalid stats with enabled suppliers");
                Assert.AreNotEqual(string.Empty, supplierToDisable.Value, " value should not be empty for invalid stats with enabled suppliers");
                Assert.AreNotEqual(0, supplierToDisable.Key.TotalCallsCount, "totalcall count must not be 0");
            }
        }


        //when list contains empty objects Case: with handled sql exception
        [TestMethod]
        public void GetFailureRateOfCarProductSuppliers_WithInValid_ExecutionOfFailureLogsFunction()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), minutes)).Returns(new SupplierStatistics()).Returns(new SupplierStatistics());
            var suppliersTodisable = new CarProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "WorldSpan" } });
            Assert.AreEqual(0, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
        }

        //when list contains all invalid stats for enabled suppliers
        [TestMethod]
        public void GetFailureRateOfCarProductSuppliers_WithInValid_ExecutionOfFailureLogsFunction_ForEnabledSuppliers()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), minutes)).Returns(new SupplierStatistics { FailureRate = 0, SuccessRate = 0, TotalRate = 0, IsEnabled = 1 }).Returns(new SupplierStatistics { FailureRate = 0, SuccessRate = 0, TotalRate = 0, IsEnabled = 1 });
            var suppliersTodisable = new CarProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "WorldSpan" } });
            Assert.AreEqual(2, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
            foreach (var supplierToDisable in suppliersTodisable)
            {
                //  Assert.IsTrue(string.Equals(supplierToDisable.Value, string.Empty), " value should be empty for invalid stats with enabled suppliers");
                Assert.AreEqual(string.Empty, supplierToDisable.Value, " value should not be empty for invalid stats with enabled suppliers");
                Assert.AreEqual(0, supplierToDisable.Key.TotalCallsCount, "totalcall count must be 0");
            }

        }
    }
}
