using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Entities;
using Notifications;
using SupplierScheduledTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScheduledTask.Test
{
    [TestClass]
    public class TestFunctionality
    {
        [TestMethod]
        public void ParseSuppliersFromXml()
        {
            var suppliersList = new XmlParser().ParseSuppliers();           
            Assert.AreEqual(typeof(List<Supplier>),suppliersList.GetType());
            Assert.AreNotEqual(0,suppliersList.Count,"List cannot be empty");

        }

        [TestMethod]
        public void SendMail_Successful_WithValidTemplateMessage()
        {
            var toMailIds=new string[] {"pkumbhar@tavisca.com", "smohite@tavisca.com"};
            var mailAttributes = new MailAttributes()
                {
                    From = "pkumbhar@tavisca.com",
                    To = toMailIds,
                    BCC = toMailIds,
                    Subject = "High alert for supplier products",
                    TemplateName = "SuppliersShutDownEmailTemplate",
                };


            var dictionary = new Dictionary<Supplier, float>()
                {
                    {
                        new Supplier()
                            {
                                CallType = "HotelMultiAvail",
                                ThreshholdValue = 60,
                                DisableIfCrossesThreshhold = 1,
                                SupplierId = 9,
                                SupplierName = "Pegasus",
                                ProductType = "Hotel"
                            }, 70
                    },
                    {
                        new Supplier()
                            {
                                CallType = "HotelMultiAvail",
                                ThreshholdValue = 60,
                                DisableIfCrossesThreshhold = 1,
                                SupplierId = 9,
                                SupplierName = "Pegasus",
                                ProductType = "Hotel"
                            }, 75
                    }

                };

            var builder = new StringBuilder();


            string directoryPath = Directory.GetCurrentDirectory();
            directoryPath = (directoryPath.EndsWith("\\bin\\Debug"))
                                ? directoryPath.Replace("\\bin\\Debug", "")
                                : directoryPath;
            string path = Path.Combine(directoryPath, @"MailBody.xml");
            var mailBody = XDocument.Load(path).ToString();

            int i = 1;
            foreach (var values in dictionary)
            {
                var isdisabled = (values.Key.DisableIfCrossesThreshhold == 1) ? "YES" : "NO";
                builder.Append(@"<tr><td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" + i++ +@"</td>");                                
                builder.Append(@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" + values.Key.SupplierName + @"</td>");
                builder.Append(@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" + values.Key.SupplierId + @"</td>");
                builder.Append(@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" + values.Key.ProductType + @"</td>");
                builder.Append(@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" + values.Key.ThreshholdValue + @"</td>");
                builder.Append(@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" + values.Value + @"</td>");
                builder.Append(@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" + isdisabled + @"</td>");
                builder.Append(@"</tr>");
            }
            mailBody=mailBody.Replace("{[RowInfo]}", builder.ToString());

            var attrubutes = new NameValueCollection { {"{[Environment]}","Dev"},{ "{[RowInfo]}", mailBody } };
            mailAttributes.TemplateAttributes = attrubutes;

            var mailObj=new Mail();
            bool isSendMail = mailObj.SendMail(mailAttributes);
            Assert.IsTrue(isSendMail);
        }



        //public void NotifyViaMail()
        //{
        //    var dictionary = new Dictionary<Supplier, float>
        //        {
        //            {
        //                new Supplier
        //                    {
        //                        CallType = "HotelMultiAvail",ThreshholdValue = 60,DisableIfCrossesThreshhold = 1,SupplierId = 9,SupplierName = "Pegasus",ProductType = "Hotel"
                                
        //                    }, 70
        //            }
        //        };
        //    try
        //    {
        //        new Mail().NotifyAboutSuppliersShutDown(dictionary);
        //    }
        //   catch(Exception ex)
        //   {
        //     Assert.Fail(ex.Message);
        //   }


        //}


    }
}
