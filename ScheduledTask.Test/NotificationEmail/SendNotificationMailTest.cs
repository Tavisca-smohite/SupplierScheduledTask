using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SupplierScheduledTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.BusinessLogic.Helper;
using Tavisca.SupplierScheduledTask.Notifications;
using ScheduledTask.Test.NotificationEmail;
using Moq;


namespace ScheduledTask.Test
{
    [TestClass]
    public class SendNotificationMailTest
    {
        [TestMethod]
        public void ParseSuppliersFromXml()
        {
            var suppliersList = new SupplierDataHelper().ParseSuppliers();

            XmlDocument doc = new XmlDocument();          
            Assert.AreEqual(typeof(List<Supplier>),suppliersList.GetType());
            Assert.AreNotEqual(0,suppliersList.Count,"List cannot be empty");

        }

        [TestMethod]
        public void SendMail_Successful_WhenBothTypesOfInputsAreProvided()
        {
            var isSendMail = new SendNotificationMail().SendNotificationEmail(StaticInputs.GetDictinoryWithBothTypesOfSuppliers());
            Assert.IsTrue(isSendMail);
        }

        [TestMethod]
        public void SendMail_Successful_WithOnlySuppliersWhoHaveCrossedThreshholdAreAvailable()
        {
            var isSendMail = new SendNotificationMail().SendNotificationEmail(StaticInputs.GetDictinoryWiththreshholdCrossedSuppliers());
            Assert.IsTrue(isSendMail);
        }

        [TestMethod]
        public void SendMail_Successful_SuppliersWithFetchingFailure()
        {
            var isSendMail = new SendNotificationMail().SendNotificationEmail(StaticInputs.GetDictinorySuppliersWithInternalFailureWhileFetchingLogs());
            Assert.IsTrue(isSendMail);
        }

    }
}
