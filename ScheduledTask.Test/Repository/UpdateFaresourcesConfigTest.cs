using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.DataAccessLayer;

namespace ScheduledTask.Test.Repository
{
    [TestClass]
    public class UpdateFaresourcesConfigTest
    {
        [TestMethod]
        public void UpdateFaresourcesConfig_DisableSupplier_Successful_WithValidInput()
        {
            var updateFaresourcesConfig = new UpdateFaresourcesConfig();

            var status = updateFaresourcesConfig.DisableSupplier(1);
            Assert.AreEqual(true, status, "it should return true for disabled suppliers");
        }
      


        [TestMethod]
        public void UpdateFaresourcesConfig_DisableSupplier_Failed_WithInvalidInput()
        {

            var updateFaresourcesConfig = new UpdateFaresourcesConfig();

            var status = updateFaresourcesConfig.DisableSupplier(502);
            Assert.AreNotEqual(true, status, "it should return false for invalid inputs");
        }


        [TestMethod]
        public void UpdateFaresourcesConfig_EnableSupplier_Successful_WithValidInput()
        {

            var updateFaresourcesConfig = new UpdateFaresourcesConfig();

            var status = updateFaresourcesConfig.EnableSupplier(96);
            Assert.AreEqual(true, status, "it should return true for enabled suppliers");

        }

        [TestMethod]
        public void UpdateFaresourcesConfig_EnableSupplier_Failed_WithInvalidInput()
        {

            var updateFaresourcesConfig = new UpdateFaresourcesConfig();

            var status = updateFaresourcesConfig.EnableSupplier(502);
            Assert.AreNotEqual(true, status, "it should return false for invalid inputs");
           
        }
    }
}
