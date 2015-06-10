using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public bool SendNotificationEmail(List<string> enabledSuppliers,List<string> disabledSuppliers)
        {

            var mailAttributes = BuildMailAttributes(enabledSuppliers,disabledSuppliers);
            bool isSendMail = new Mail().SendMail(mailAttributes);
            return isSendMail;
        }

        

        

        #region helper methods to build mail body for disabled suppliers notification mail

        private MailAttributes BuildMailAttributes(Dictionary<Supplier, string> suppliersToDisable)
        {
            //string directoryPath = Directory.GetCurrentDirectory();
            //directoryPath = (directoryPath.EndsWith("\\bin\\Debug"))
            //                    ? directoryPath.Replace("\\bin\\Debug", "")
            //                    : directoryPath;
            string path =  Configuration.FailedSuppliersNotificationMailBodyData;
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
            mailBody = updatedListOfSuppliersToDisable.Any() ?
                BuildTableInMailBody(updatedListOfSuppliersToDisable, i, mailBody) :
                Regex.Replace(mailBody, @"<div id=""#1"">(.*?)(?=<div id=""#2"">)", string.Empty, RegexOptions.Compiled | RegexOptions.Singleline);

            mailBody = listOfSuppliersWithFetchingFailure.Any() ?
                AddSupplierInfoForWhomFetchingFailureOccured(listOfSuppliersWithFetchingFailure, mailBody) :
                Regex.Replace(mailBody, @"<div id=""#2"">(.*?)</div>", string.Empty, RegexOptions.Compiled | RegexOptions.Singleline);

            mailBody = mailBody.Replace("{[Environment]}", Configuration.Environment);

            var attributes = new NameValueCollection { { "{[SuppliersStatus]}", mailBody } };
            mailAttributes.TemplateAttributes = attributes;
            return mailAttributes;
        }

        private  string BuildTableInMailBody(Dictionary<Supplier, string> suppliersToDisable, int i, string mailBody)
        {
            var builder = new StringBuilder();         
            const string rowStyle =@"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">";
            foreach (var supplierToDisable in suppliersToDisable)
            {
                var needsToDisable = (supplierToDisable.Key.DisableIfCrossesThreshhold == 1) ? "YES" : "NO";
                var isDisabled = (supplierToDisable.Key.IsDisabled== true) ? "YES" : "NO";
                builder.Append(@"<tr>" + rowStyle + i++ + @"</td>");
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

        private  string AddSupplierInfoForWhomFetchingFailureOccured(Dictionary<Supplier, string> listOfSuppliersWithFetchingFailure,string mailBody)
        {
            var builder = new StringBuilder();
            const string listStyle = @"<li style="" vertical-align: middle; color: blue;font-size:medium ;font-family: cursive"">";
            foreach (var supplier in listOfSuppliersWithFetchingFailure)
            {                               
                builder.Append(listStyle + supplier.Key.SupplierName + @"</li>");              
               
            }
            mailBody = mailBody.Replace("{[FailedSupplierData]}", builder.ToString());
            return mailBody;
        }
        #endregion

        #region helper methods to build mail body for enabled suppliers notification mail
        private MailAttributes BuildMailAttributes(List<string> enabledSuppliers, List<string> disabledSuppliers)
        {
            //string directoryPath = Directory.GetCurrentDirectory();
            //directoryPath = (directoryPath.EndsWith("\\bin\\Debug"))
            //                    ? directoryPath.Replace("\\bin\\Debug", "")
            //                    : directoryPath;
            string path =  Configuration.EnabledSupliersNotificationMailBodyData;
            var mailBody = XDocument.Load(path).ToString();
            var mailAttributes = new MailAttributes()
            {
                From = Configuration.MailFrom,
                To = Configuration.MailTo,
                BCC = Configuration.MailCc,
                Subject = Configuration.MailSubject,
                TemplateName = Configuration.TemplateName,
            };
            mailBody =(enabledSuppliers != null && enabledSuppliers.Any())?
                BuildTableInMailBodyForEnabledSuppliers(enabledSuppliers, mailBody):
                Regex.Replace(mailBody, @"<div id=""#1"">(.*?)(?=<div id=""#2"">)", string.Empty, RegexOptions.Compiled | RegexOptions.Singleline);
            

            mailBody = (disabledSuppliers != null && disabledSuppliers.Any()) ?
               AddSupplierInfoWhichAreStillDisabled(disabledSuppliers, mailBody) :
               Regex.Replace(mailBody, @"<div id=""#2"">(.*?)</div>", string.Empty, RegexOptions.Compiled | RegexOptions.Singleline);

            mailBody = mailBody.Replace("{[Environment]}", Configuration.Environment);

            var attributes = new NameValueCollection { { "{[SuppliersStatus]}", mailBody } };
            mailAttributes.TemplateAttributes = attributes;
            return mailAttributes;
        }

        private string AddSupplierInfoWhichAreStillDisabled(List<string> disabledSuppliers, string mailBody)
        {
            var builder = new StringBuilder();
            const string listStyle = @"<li style="" vertical-align: middle; color: blue;font-size:medium ;font-family: cursive"">";
            foreach (var disabledSupplier in disabledSuppliers)
            {
                var supplierData = new List<string>();
                if (!string.IsNullOrEmpty(disabledSupplier))
                    supplierData.AddRange(disabledSupplier.Split('_'));

                builder.Append(listStyle + supplierData[0] + @"</li>");

            }
            mailBody = mailBody.Replace("{[DisabledSupplierData]}", builder.ToString());
            return mailBody;
        }

        private string BuildTableInMailBodyForEnabledSuppliers(List<string> enabledSuppliers, string mailBody)
        {
            
            const string rowStyle = @"<td style=""border: 1px solid black;color: blue;font-size:medium ;font-family: cursive"">";      
            var builder = new StringBuilder();
            int i = 1;
            foreach (string enabledSupplier in enabledSuppliers)
            {
                var supplierData = new List<string>();
                if (!string.IsNullOrEmpty(enabledSupplier))
                    supplierData.AddRange(enabledSupplier.Split('_'));
                
                builder.Append(@"<tr>" + rowStyle + i++ + @"</td>");
                builder.Append(rowStyle + supplierData[0] + @"</td>");
                builder.Append(rowStyle + supplierData[1] + @"</td>");
                builder.Append(@"</tr>");               
            }
            mailBody = mailBody.Replace("{[RowInfo]}", builder.ToString());
            return mailBody;
        }
        #endregion

    }
}
