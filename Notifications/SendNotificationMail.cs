using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Entities;

namespace Notifications
{
    public class SendNotificationMail
    {
        public bool SendNotificationEmail(Dictionary<Supplier,float> suppliersToDisable)
        {
           
            var mailAttributes = BuildMailAttributes(suppliersToDisable);
            bool isSendMail = new Mail().SendMail(mailAttributes);
            return isSendMail;
        }

        private static MailAttributes BuildMailAttributes(Dictionary<Supplier, float> suppliersToDisable)
        {
            string directoryPath = Directory.GetCurrentDirectory();
            directoryPath = (directoryPath.EndsWith("\\bin\\Debug"))
                                ? directoryPath.Replace("\\bin\\Debug", "")
                                : directoryPath;
            string path = Path.Combine(directoryPath, @"MailBody.xml");
            var mailBody = XDocument.Load(path).ToString();
            var mailAttributes = new MailAttributes()
                {
                    From = Configuration.MailFrom,
                    To = Configuration.MailTo,
                    BCC = Configuration.MailCc,
                    Subject = Configuration.MailSubject,
                    TemplateName = Configuration.TemplateName,
                };
            int i = 1;
            mailBody = BuildMailBody(suppliersToDisable, i, mailBody);

            var attrubutes = new NameValueCollection {{"{[Environment]}", "Dev"}, {"{[RowInfo]}", mailBody}};
            mailAttributes.TemplateAttributes = attrubutes;
            return mailAttributes;
        }

        private static string BuildMailBody(Dictionary<Supplier, float> suppliersToDisable, int i, string mailBody)
        {
            var builder = new StringBuilder();
            foreach (var supplierToDisable in suppliersToDisable)
            {
                var isdisabled = (supplierToDisable.Key.DisableIfCrossesThreshhold == 1) ? "YES" : "NO";
                builder.Append(
                    @"<tr><td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" + i++ +
                    @"</td>");
                builder.Append(@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" +
                               supplierToDisable.Key.SupplierName + @"</td>");
                builder.Append(@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" +
                               supplierToDisable.Key.SupplierId + @"</td>");
                builder.Append(@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" +
                               supplierToDisable.Key.ProductType + @"</td>");
                builder.Append(@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" +
                               supplierToDisable.Key.ThreshholdValue + @"</td>");
                builder.Append(@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" +
                               supplierToDisable.Value + @"</td>");
                builder.Append(@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">" +
                               isdisabled + @"</td>");
                builder.Append(@"</tr>");
            }
            mailBody = mailBody.Replace("{[RowInfo]}", builder.ToString());
            return mailBody;
        }
    }
}
