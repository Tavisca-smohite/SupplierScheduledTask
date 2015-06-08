using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.BusinessLogic.Controller;
using Tavisca.SupplierScheduledTask.BusinessLogic.Helper;
using Tavisca.SupplierScheduledTask.DataAccessLayer;
using Tavisca.SupplierScheduledTask.BusinessLogic.ProductSuppliersStrategy;

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
        //when list contains all valid stats for enabled suppliers
        [TestMethod]
        public void GetFailureRateOfAirProductSuppliers_WithValid_ExecutionOfFailureLogsFunction()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), 1000)).Returns(new SupplierStatistics { FailureRate = 60, SuccessRate = 40, TotalRate = 100, IsEnabled = 1 }).Returns(new SupplierStatistics { FailureRate = 50, SuccessRate = 50, TotalRate = 100, IsEnabled = 1 });           
            var suppliersTodisable = new AirProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "Mystifly" } });
            Assert.AreEqual(2, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
            foreach (var supplierToDisable in suppliersTodisable)
            {
                Assert.IsTrue(!string.Equals(supplierToDisable.Value, string.Empty), " value should not be empty for invalid stats with enabled suppliers");
               // Assert.AreEqual(supplierToDisable.Value.Equals());
            }
        }


        //when list contains empty objects Case: with handled sql exception
        [TestMethod]
        public void GetFailureRateOfAirProductSuppliers_WithInValid_ExecutionOfFailureLogsFunction()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), 1000)).Returns(new SupplierStatistics()).Returns(new SupplierStatistics());            
            var suppliersTodisable = new AirProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "WorldSpan" } });
            Assert.AreEqual(0, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
        }

        //when list contains all invalid stats for enabled suppliers
        [TestMethod]
        public void GetFailureRateOfAirProductSuppliers_WithInValid_ExecutionOfFailureLogsFunctionForEnabledSuppliers()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), 1000)).Returns(new SupplierStatistics { FailureRate = 0, SuccessRate = 0, TotalRate = 0, IsEnabled = 1 }).Returns(new SupplierStatistics { FailureRate = 0, SuccessRate = 0, TotalRate = 0, IsEnabled = 1 });
            var suppliersTodisable = new AirProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "WorldSpan" } });
            Assert.AreEqual(2, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
            foreach (var supplierToDisable in suppliersTodisable)
            {
                Assert.IsTrue(string.Equals(supplierToDisable.Value, string.Empty), " value should be empty for invalid stats with enabled suppliers");
            }
        
        }



        //when list contains all valid stats for enabled suppliers
        [TestMethod]
        public void GetFailureRateOfHotelProductSuppliers_WithValid_ExecutionOfFailureLogsFunction()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), 1000)).Returns(new SupplierStatistics { FailureRate = 60, SuccessRate = 40, TotalRate = 100, IsEnabled = 1 }).Returns(new SupplierStatistics { FailureRate = 50, SuccessRate = 50, TotalRate = 100, IsEnabled = 1 });
            var suppliersTodisable = new HotelProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "Mystifly" } });
            Assert.AreEqual(2, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
            foreach (var supplierToDisable in suppliersTodisable)
            {
                Assert.IsTrue(!string.Equals(supplierToDisable.Value, string.Empty), " value should not be empty for invalid stats with enabled suppliers");
            }
        }


        //when list contains empty objects Case: with handled sql exception
        [TestMethod]
        public void GetFailureRateOfHotelProductSuppliers_WithInValid_ExecutionOfFailureLogsFunction()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), 1000)).Returns(new SupplierStatistics()).Returns(new SupplierStatistics());
            var suppliersTodisable = new HotelProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "WorldSpan" } });
            Assert.AreEqual(0, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
        }

        //when list contains all invalid stats for enabled suppliers
        [TestMethod]
        public void GetFailureRateOfHotelProductSuppliers_WithInValid_ExecutionOfFailureLogsFunction_ForEnabledSuppliers()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), 1000)).Returns(new SupplierStatistics { FailureRate = 0, SuccessRate = 0, TotalRate = 0, IsEnabled = 1 }).Returns(new SupplierStatistics { FailureRate = 0, SuccessRate = 0, TotalRate = 0, IsEnabled = 1 });
            var suppliersTodisable = new HotelProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "WorldSpan" } });
            Assert.AreEqual(2, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
            foreach (var supplierToDisable in suppliersTodisable)
            {
                Assert.IsTrue(string.Equals(supplierToDisable.Value, string.Empty), " value should be empty for invalid stats with enabled suppliers");
            }

        }

        //when list contains all valid stats for enabled suppliers
        [TestMethod]
        public void GetFailureRateOfCarProductSuppliers_WithValid_ExecutionOfFailureLogsFunction()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), 1000)).Returns(new SupplierStatistics { FailureRate = 60, SuccessRate = 40, TotalRate = 100, IsEnabled = 1 }).Returns(new SupplierStatistics { FailureRate = 50, SuccessRate = 50, TotalRate = 100, IsEnabled = 1 });
            var suppliersTodisable = new CarProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "Mystifly" } });
            Assert.AreEqual(2, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
            foreach (var supplierToDisable in suppliersTodisable)
            {
                Assert.IsTrue(!string.Equals(supplierToDisable.Value, string.Empty), " value should not be empty for invalid stats with enabled suppliers");
            }
        }


        //when list contains empty objects Case: with handled sql exception
        [TestMethod]
        public void GetFailureRateOfCarProductSuppliers_WithInValid_ExecutionOfFailureLogsFunction()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), 1000)).Returns(new SupplierStatistics()).Returns(new SupplierStatistics());
            var suppliersTodisable = new CarProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "WorldSpan" } });
            Assert.AreEqual(0, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
        }

        //when list contains all invalid stats for enabled suppliers
        [TestMethod]
        public void GetFailureRateOfCarProductSuppliers_WithInValid_ExecutionOfFailureLogsFunction_ForEnabledSuppliers()
        {
            Mock<ISupplierLogRepository> mockSupplierLogRepository = new Mock<ISupplierLogRepository>();
            mockSupplierLogRepository.SetupSequence(m => m.GetFailureLogs(It.IsAny<Supplier>(), 1000)).Returns(new SupplierStatistics { FailureRate = 0, SuccessRate = 0, TotalRate = 0, IsEnabled = 1 }).Returns(new SupplierStatistics { FailureRate = 0, SuccessRate = 0, TotalRate = 0, IsEnabled = 1 });
            var suppliersTodisable = new CarProductSupplierStrategy(mockSupplierLogRepository.Object).GetFailureRateForProductSuppliers(new List<Supplier> { new Supplier(), new Supplier { SupplierName = "WorldSpan" } });
            Assert.AreEqual(2, suppliersTodisable.Count(), "only suppliers which are enabled and have totalrate=100 should be added to dictionary");
            foreach (var supplierToDisable in suppliersTodisable)
            {
                Assert.IsTrue(string.Equals(supplierToDisable.Value, string.Empty), " value should be empty for invalid stats with enabled suppliers");
            }

        }
    }
}
