﻿using System;
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
        public void SendMail_Successful_WithValidTemplateMessage()
        {
            var isSendMail=new SendNotificationMail().SendNotificationEmail(GetDictinory());
            Assert.IsTrue(isSendMail);
        }

        private Dictionary<Supplier,float> GetDictinory()
        {
            var dictionary = new Dictionary<Supplier, float>()
                {
                    {
                        new Supplier()
                            {
                                SupplierId = 118,
                                SupplierName = "JacTravel",
                                IsDisabled = true,
                                ProductType = "Hotel",
                                DisableIfCrossesThreshhold = 1,
                                ThreshholdValue = 50
                            }, 50
                    }
                };
            return dictionary;
        }


    }
}