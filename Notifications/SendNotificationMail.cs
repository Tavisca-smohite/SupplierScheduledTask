using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tavisca.SupplierScheduledTask.BusinessEntities;


namespace Tavisca.SupplierScheduledTask.Notifications
{
    public class SendNotificationMail
    {
        public bool SendNotificationEmail(Dictionary<Supplier,string> suppliersToDisable)
        {
           
            var mailAttributes = BuildMailAttributes(suppliersToDisable);
            bool isSendMail = new Mail().SendMail(mailAttributes);
            return isSendMail;
        }

        private static MailAttributes BuildMailAttributes(Dictionary<Supplier, string> suppliersToDisable)
        {
            string directoryPath = Directory.GetCurrentDirectory();
            directoryPath = (directoryPath.EndsWith("\\bin\\Debug"))
                                ? directoryPath.Replace("\\bin\\Debug", "")
                                : directoryPath;
            string path = directoryPath + Configuration.MailBodyData;
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

            var updatedListOfSuppliersToDisable = suppliersToDisable.Where(d => !string.Equals(d.Value, string.Empty)).ToDictionary(x => x.Key, x => x.Value);
            var listOfSuppliersWithFetchingFailure = suppliersToDisable.Where(d => string.Equals(d.Value, string.Empty)).ToDictionary(x => x.Key, x => x.Value);

            mailBody = BuildTableInMailBody(updatedListOfSuppliersToDisable, i, mailBody);

            var attrubutes = new NameValueCollection {{"{[Environment]}", Configuration.Environment}, {"{[RowInfo]}", mailBody}};
            mailAttributes.TemplateAttributes = attrubutes;
            return mailAttributes;
        }

        private static string BuildTableInMailBody(Dictionary<Supplier, string> suppliersToDisable, int i, string mailBody)
        {
            var builder = new StringBuilder();         
            const string rowStyle =@"<tr><td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">";
            foreach (var supplierToDisable in suppliersToDisable)
            {
                var needsToDisable = (supplierToDisable.Key.DisableIfCrossesThreshhold == 1) ? "YES" : "NO";
                var isDisabled = (supplierToDisable.Key.IsDisabled== true) ? "YES" : "NO";
                builder.Append(rowStyle + i++ +@"</td>");
                builder.Append(rowStyle + supplierToDisable.Key.SupplierName + @"</td>");
                builder.Append(rowStyle + supplierToDisable.Key.SupplierId + @"</td>");
                builder.Append(rowStyle + supplierToDisable.Key.ProductType + @"</td>");
                builder.Append(rowStyle + supplierToDisable.Key.ThreshholdValue + @"</td>");
                builder.Append(rowStyle + supplierToDisable.Value + @"</td>");
                builder.Append(rowStyle + needsToDisable + @"</td>");
                builder.Append(rowStyle + isDisabled + @"</td>");
                builder.Append(@"</tr>");
            }
            mailBody = mailBody.Replace("{[RowInfo]}", builder.ToString());
            return mailBody;
        }

        
    }
}
