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


        // behaviour of function when suppliers have crossed threshhold failure rate and total calls count
        [TestMethod]
        public void GetSuppliersTodisable_When_SuppliersHave_BothCrossedThreshholdAndTotalCount()
        {
            var productWiseSuppliersList = GetDictionary();
            var mockProductSupplier = new Mock<IProductSupplier>();
            mockProductSupplier.SetupSequence(m => m.GetFailureRateForProductSuppliers(It.IsAny<List<Supplier>>()))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateAndTotalCallsCount(1))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateAndTotalCallsCount(2))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateAndTotalCallsCount(3));
            var suppliersTodisable = new SupplierDataController(mockProductSupplier.Object).GetListOfSuppliersToDisable(productWiseSuppliersList);
            Assert.AreEqual(3, suppliersTodisable.Count(), "it should return all suppliers in above case");
        }


        // behaviour of function when suppliers have crossed threshhold failure rate and but total calls count is less than configured calls count
        [TestMethod]
        public void GetSuppliersTodisable_When_SuppliersHave_OnlyCrossedThreshholdAndNotTotalCount()
        {
            var productWiseSuppliersList = GetDictionary();
            var mockProductSupplier = new Mock<IProductSupplier>();
            mockProductSupplier.SetupSequence(m => m.GetFailureRateForProductSuppliers(It.IsAny<List<Supplier>>()))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateAndInvalidTotalCallsCount(1))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateAndInvalidTotalCallsCount(2))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateAndInvalidTotalCallsCount(3));
            var suppliersTodisable = new SupplierDataController(mockProductSupplier.Object).GetListOfSuppliersToDisable(productWiseSuppliersList);
            Assert.AreEqual(0, suppliersTodisable.Count(), "it should not return single supplier in above case");
        }


        // behaviour of function when suppliers have crossed threshhold failure rate or total calls count but not both at same time
        [TestMethod]
        public void GetSuppliersTodisable_When_SuppliersHave_CrossedThreshholdOrTotalCountButNotBoth_AtASameTime()
        {
            var productWiseSuppliersList = GetDictionary();
            var mockProductSupplier = new Mock<IProductSupplier>();
            mockProductSupplier.SetupSequence(m => m.GetFailureRateForProductSuppliers(It.IsAny<List<Supplier>>()))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateOrInvalidTotalCallsCount(1))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateOrInvalidTotalCallsCount(2))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateOrInvalidTotalCallsCount(3));
            var suppliersTodisable = new SupplierDataController(mockProductSupplier.Object).GetListOfSuppliersToDisable(productWiseSuppliersList);
            Assert.AreEqual(0, suppliersTodisable.Count(), "it should not return single supplier in above case");
        }


        // behaviour of function when suppliers have crossed threshhold failure rate or total calls 
        [TestMethod]
        public void GetSuppliersTodisable_When_SuppliersHave_CrossedThreshholdOrTotalCountButNotBoth()
        {
            var productWiseSuppliersList = GetDictionary(1);
            var mockProductSupplier = new Mock<IProductSupplier>();
            mockProductSupplier.SetupSequence(m => m.GetFailureRateForProductSuppliers(It.IsAny<List<Supplier>>()))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidAsWellAsInValidFailureRateOrTotalCallsCount(1))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidAsWellAsInValidFailureRateOrTotalCallsCount(2))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidAsWellAsInValidFailureRateOrTotalCallsCount(3))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidAsWellAsInValidFailureRateOrTotalCallsCount(4));
            var suppliersTodisable = new SupplierDataController(mockProductSupplier.Object).GetListOfSuppliersToDisable(productWiseSuppliersList);
            Assert.AreEqual(2, suppliersTodisable.Count(), "it should not return 2 suppliers in above case one with string.empty and one with valid arguments");
        }

        //when suppliers who has crossed threshhold are available
        //and disable supplier is mocked to return true for suppliers to disable
        [TestMethod]
        public void Test_InvokeLogic_WhenSuppliersTodisableAreAvailable()
        {            
            var mockProductSupplier = new Mock<IProductSupplier>();
            var mockUpdateFaresourcesConfig = new Mock<IUpdateFaresourcesConfig>();
            mockProductSupplier.SetupSequence(m => m.GetFailureRateForProductSuppliers(It.IsAny<List<Supplier>>()))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateAndTotalCallsCount(1))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateAndTotalCallsCount(2))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateAndTotalCallsCount(3));
            mockUpdateFaresourcesConfig.Setup(m => m.DisableSupplier(It.IsAny<int>())).Returns(true);
            new SupplierDataController(mockProductSupplier.Object,mockUpdateFaresourcesConfig.Object).Invoke();
        }

        //when suppliers who has crossed threshhold are available       
        [TestMethod]
        public void Test_InvokeLogic_WithoutmockingConfigurationBehaviour()
        {
            var mockProductSupplier = new Mock<IProductSupplier>();
           // var mockUpdateFaresourcesConfig = new Mock<IUpdateFaresourcesConfig>();
            mockProductSupplier.SetupSequence(m => m.GetFailureRateForProductSuppliers(It.IsAny<List<Supplier>>()))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateAndTotalCallsCount(1))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateAndTotalCallsCount(2))
                .Returns(StaticInputsForSupplierDataController.DictionaryWithValidFailureRateAndTotalCallsCount(3));            
            new SupplierDataController(mockProductSupplier.Object).Invoke();
        }
        private Dictionary<string, List<Supplier>> GetDictionary()
        {
            var productWiseSuppliersList = new Dictionary<string, List<Supplier>>
           {
               {"Hotel",new List<Supplier>()},
               {"Air",new List<Supplier>()},
               {"Car",new List<Supplier>()}
           };
            return productWiseSuppliersList;
        }

        private Dictionary<string, List<Supplier>> GetDictionary(int i)
        {
            var productWiseSuppliersList = new Dictionary<string, List<Supplier>>
           {
               {"Hotel",new List<Supplier>{new Supplier(),new Supplier()}},
               {"Air",new List<Supplier>()},
               {"Car",new List<Supplier>()}
           };
            return productWiseSuppliersList;
        }
    }
}
