using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Notifications
{
    public class MailDataParser
    {


        public MailData ParseMailData()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            directoryPath = (directoryPath.EndsWith("\\bin\\Debug"))
                                ? directoryPath.Replace("\\bin\\Debug", "")
                                : directoryPath;
            string path = Path.Combine(directoryPath, @"Suppliers.xml");
            var MailDataXml = XDocument.Load(path);

            var mailData =
                MailDataXml.Element("MailData"); //.Where(arg => arg.Name.LocalName == "MailData").ToList();

            var mail = new MailData();
            if (mailData!=null)
            {
                var env = mailData.Element("Environment");
                var host=mailData.Element("Host");
                var port=mailData.Element("Port");
                var userId=mailData.Element("UserId");
                var pwd = mailData.Element("Password");
               var mailTo = mailData.Element("MailTo");
               var mailCC = mailData.Element("MailCC");
               var mailFrom = mailData.Element("MailFrom");
               var mailSubject = mailData.Element("MailSubject");
               var displayName = mailData.Element("DisplayName");

                if (env != null) mail.Environment = env.Value;
                if (host != null) mail.Host = host.Value;
                if (port != null) mail.Environment = port.Value;
                if (userId != null) mail.Environment = userId.Value;
                if (pwd != null) mail.Environment = pwd.Value;
                if (mailTo != null) mail.Environment = mailTo.Value;
                if (mailCC != null) mail.Environment = mailCC.Value;
                if (mailFrom != null) mail.Environment = mailFrom.Value;
                if (mailSubject != null) mail.Environment = mailSubject.Value;
                if (displayName != null) mail.Environment = displayName.Value;
            }
                     
            return mail;
        }


    }

    public class MailData
    {
        public string Environment{get;set;}
        public string Port{get;set;}
        public string Host{get;set;}
        public string UserId{get;set;}
        public string Password{get;set;}
        public string MailTo{get;set;}
        public string MailCC{get;set;}
        public string MailFrom{get;set;}
        public string MailSubject{get;set;}
        public string DisplayName{get;set;}
    }
}
