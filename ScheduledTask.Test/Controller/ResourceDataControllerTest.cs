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
        [TestInitialize]
        public void CleanResourceFile_atTheBeginning()
        {           
            new ResourceDataController().RemoveAllEntriesInResourceFile();
        }
        [TestMethod]
        public void Verify_UpdateResourceFileLogic_WhenPassedProperData()
        {
            var list = new List<Supplier>
                {
                    new Supplier {SupplierId = 9,SupplierName = "Pegasus"},
                    new Supplier {SupplierId = 118,SupplierName = "JacTravel"}
                };
            new ResourceDataController().UpdateResourceFile(list);
        }

        [TestMethod]
        public void Verify_RemoveEnriesFromResourceFileLogic_WhenPassedProperData()
        {
            Verify_UpdateResourceFileLogic_WhenPassedProperData();
            var list = new List<string>
                {
                   "JacTravel_118",
                    "Pegasus_9"
                };
            new ResourceDataController().RemoveEntriesFromResourceFile(list);
        }
    }
}
