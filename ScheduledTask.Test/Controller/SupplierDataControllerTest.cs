﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.BusinessLogic.Controller;
using Tavisca.SupplierScheduledTask.BusinessLogic.Helper;
using Tavisca.SupplierScheduledTask.BusinessLogic.ProductSuppliersStrategy;

namespace ScheduledTask.Test
{
    [TestClass]
    public class SupplierDataControllerTest
    {

        [TestMethod]
        public  void GetLitOfSuppliersToDisable_Successful_withValidInputs()
        {
            Dictionary<string, List<Supplier>> productWiseSuppliersList = new SupplierDataHelper().GetProductWiseSuppliersList();
           var suppliersToDisable= new SupplierDataController().GetListOfSuppliersToDisable(productWiseSuppliersList);

        }

       
    }
}