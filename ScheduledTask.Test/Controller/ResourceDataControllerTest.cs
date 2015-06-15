using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.BusinessLogic;

namespace ScheduledTask.Test
{
    [TestClass]
    public class ResourceDataControllerTest
    {
        private ResourceDataController _resourceDataController;
        [TestInitialize]
        public void CleanResourceFile_atTheBeginning()
        {
            _resourceDataController = new ResourceDataController();
            _resourceDataController.RemoveAllEntriesInResourceFile();
        }
        [TestMethod]
        public void Verify_UpdateResourceFileLogic_WhenPassedProperData()
        {
            var list = new List<Supplier>
                {
                    new Supplier {SupplierId = 9,SupplierName = "Pegasus"},
                    new Supplier {SupplierId = 118,SupplierName = "JacTravel"}
                };
            _resourceDataController.UpdateResourceFile(list);
        }

        [TestMethod]
        public void Verify_UpdateResourceFileLogic_WhenPassedProperData_OverwriteLogic()
        {
            
            var list = new List<Supplier>
                {
                    new Supplier {SupplierId = 9,SupplierName = "Pegasus"},
                    new Supplier {SupplierId = 118,SupplierName = "JacTravel"}
                };
            _resourceDataController.UpdateResourceFile(list);
            _resourceDataController.UpdateResourceFile(list);
            var resourceEntries=_resourceDataController.ReadResourceFile();
            Assert.AreEqual(2,resourceEntries.Count,"it should not update existing entries for same keys instead of adding multiple data");
        }
        [TestMethod]
        public void Verify_RemoveEnriesFromResourceFileLogic_WhenPassedProperData_RemoveAllEntries()
        {
            Verify_UpdateResourceFileLogic_WhenPassedProperData();
            var list = new List<string>
                {
                   "JacTravel_118",
                    "Pegasus_9"
                };
            _resourceDataController.RemoveEntriesFromResourceFile(list);
            var resourceEntries = _resourceDataController.ReadResourceFile();
            Assert.AreEqual(0, resourceEntries.Count, "it should delete all entries");
        }

        [TestMethod]
        public void Verify_RemoveEnriesFromResourceFileLogic_WhenPassedProperData_RemoveFewEntries()
        {
            Verify_UpdateResourceFileLogic_WhenPassedProperData();
            var list = new List<string>
                {
                   "JacTravel_118"
                    
                };
            _resourceDataController.RemoveEntriesFromResourceFile(list);
            var resourceEntries = _resourceDataController.ReadResourceFile();
            Assert.AreEqual(1, resourceEntries.Count);
        }
    }
}
