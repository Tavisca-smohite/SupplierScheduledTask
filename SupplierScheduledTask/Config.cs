using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierScheduledTask
{
    public class Config
    {
        public static string Environment
        {
            get { return ConfigurationManager.AppSettings["Environment"]; }
        }

        public static string Host
        {
            get { return ConfigurationManager.AppSettings["Host"]; }
        }

        public static int Port
        {
            get
            {
                int port = 587;
                int.TryParse(ConfigurationManager.AppSettings["Port"], out port);
                return port;
            }
        }

        public static string UserId
        {
            get { return ConfigurationManager.AppSettings["UserId"]; }
        }

        public static string Password
        {
            get { return ConfigurationManager.AppSettings["Password"]; }
        }

        public static string MailTo
        {
            get { return ConfigurationManager.AppSettings["MailTo"]; }
        }

        public static string MailCC
        {
            get { return ConfigurationManager.AppSettings["MailCC"]; }
        }

        public static string MailFrom
        {
            get { return ConfigurationManager.AppSettings["MailFrom"]; }
        }

        public static string DisplayName
        {
            get { return ConfigurationManager.AppSettings["DisplayName"]; }
        }

        public static string MailSubject
        {
            get { return ConfigurationManager.AppSettings["MailSubject"]; }
        }
    }
}
