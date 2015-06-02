using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Mail;
using System.Text;
using Entities;

namespace Notifications
{
    public class Mail
    {

      public void NotifyAboutSuppliersShutDown(Dictionary<Supplier,float> suppliersToDisable )
      {
          var msg = new StringBuilder();
          var mailData=new MailDataParser().ParseMailData();
          
          msg.Append(string.Format("Suppliers who have crossed failure threshhold value on {0} environment are as follow:",mailData.Environment));
          msg.Append(Environment.NewLine);
          msg.Append("No.  SupplierName   SupplierID   ThreshholdVlue   FailureRate");
          msg.Append(Environment.NewLine);
          msg.Append("--------------------------------------------------------------");
          msg.Append(Environment.NewLine);
           var i = 0;
          foreach (var supplierToDisable in suppliersToDisable)
          {
              var supplier = supplierToDisable.Key;
              msg.Append(string.Format("{0}  {1}    {2}       {3}        {4}", i + 1, supplier.SupplierName, supplier.SupplierId, supplier.ThreshholdValue,supplierToDisable.Value));
              msg.Append(Environment.NewLine);
              i++;
          }
       
              
          //msg.Append(Environment.NewLine);
         // msg.Append("Start Time: " + TestRunContext.Current.StartTime);
          msg.Append(Environment.NewLine);
          msg.Append("Thanks and regards,");
          msg.Append(Environment.NewLine);
          msg.Append("Sheetal Mohite");
        //  msg.Append("End Time: " + System.DateTime.Now.ToString("G"));
          var from = new MailAddress(mailData.MailFrom, mailData.DisplayName);         

          var mail = new MailMessage()
          {
              From = from,
              Sender = from,
              Subject = mailData.MailSubject, 
              Body = msg.ToString(),
          };
          foreach (var mailAddress in mailData.MailTo.Split(','))
          {
              mail.To.Add(new MailAddress(mailAddress));
          }
          foreach (var mailAddress in mailData.MailCC.Split(','))
          {
              if (!string.IsNullOrEmpty(mailAddress))
                  mail.CC.Add(new MailAddress(mailAddress));
          }

          var smtp = new SmtpClient
          {
              Host = mailData.Host,
              Port = Convert.ToInt32(mailData.Port),
              Credentials = new NetworkCredential(Config.UserId, Config.Password),
              EnableSsl = true
          };
          Console.WriteLine("Sending email...");
          try
          {
              smtp.Send(mail);
          }
          catch (Exception exception)
          {
              
          }
      }       
    }
}
