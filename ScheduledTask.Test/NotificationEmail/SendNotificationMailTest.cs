using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SupplierScheduledTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.BusinessLogic;
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
        //tests to verify behaviour of  mail sent to notify about disabled suppliers
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

        //behaviour of function when exception is thrown .
        [TestMethod]
        public void SendMail_WithInvalidInputs_ThrowsException_ShouldReturnFalse()
        {
            var isSendMail = new Mail().SendMail(null);
            Assert.IsFalse(isSendMail);
        }


        //tests to verify behaviour of  mail sent to notify about enabled suppliers
        [TestMethod]
        public void SendMail_Successful_WhenBothTypesOfInputsAreProvided_EnabledSuppliers()
        {         
            var isSendMail = new SendNotificationMail().SendNotificationEmail(StaticInputs.GetEnabledSuppliers(),StaticInputs.GetDisabledSuppliers());
            Assert.IsTrue(isSendMail);
        }


        [TestMethod]
        public void SendMail_Successful_WhenoOnlyEnabledSuppliersHasPassedInInputs_EnabledSuppliers()
        {
            var isSendMail = new SendNotificationMail().SendNotificationEmail(new List<string>(), StaticInputs.GetDisabledSuppliers());
            Assert.IsTrue(isSendMail);
        }


        [TestMethod]
        public void SendMail_Successful_WhenoOnlydisabledSuppliersHasPassedInInputs_EnabledSuppliers()
        {
            var isSendMail = new SendNotificationMail().SendNotificationEmail(StaticInputs.GetEnabledSuppliers(),new List<string>());
            Assert.IsTrue(isSendMail);
        }


    }
}
