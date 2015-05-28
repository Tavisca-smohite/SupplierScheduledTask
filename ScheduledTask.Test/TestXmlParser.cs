using System;
using System.Collections.Generic;
using Entities;
using SupplierScheduledTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScheduledTask.Test
{
    [TestClass]
    public class TestXmlParser
    {
        [TestMethod]
        public void ParseSuppliersFromXml()
        {
            var suppliersList = new XmlParser().ParseSuppliers();           
            Assert.AreEqual(typeof(List<Supplier>),suppliersList.GetType());
            Assert.AreNotEqual(0,suppliersList.Count,"List cannot be empty");

        }
    }
}
