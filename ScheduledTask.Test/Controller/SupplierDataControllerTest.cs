using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.BusinessLogic.ProductSuppliersStrategy;

namespace ScheduledTask.Test
{
    [TestClass]
    public class SupplierDataControllerTest
    {

        [TestMethod]
        public  void GetFailureRateForProductSuppliers_Successful_WithValidInputs()
        {
            var supplierList = new AirProductSupplierStrategy().GetFailureRateForProductSuppliers(new List<Supplier>());
        }

        private List<Supplier> GetSupplierList()
        {
            var list = new List<Supplier>();
            list.Add(new Supplier
                {
                    CallType = "AirMultiAvail",
                    DisableIfCrossesThreshhold = 1,
                    IsDisabled = false,
                    ProductType = "Air",
                    SupplierId = 1,
                    SupplierName = "WorldSpan",
                    ThreshholdValue = 65
                    
                });
            return list;
        }
    }
}
