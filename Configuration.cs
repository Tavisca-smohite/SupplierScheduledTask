using System;


    public static class Configuration
    {
        public static string[] MailTo
        {
            get
            {
                string key = ConfigurationManager.AppSettings["MailTo"];
                var mailTo = new List<string>();
                if (string.IsNullOrEmpty(key))
                    return mailTo.ToArray();

                mailTo.AddRange(key.Split('|'));
                return mailTo.ToArray();
            }
        }

        public static string[] MailCc
        {
            get
            {
                string key = ConfigurationManager.AppSettings["MailCC"];

                var mailCC = new List<string>();
                if (string.IsNullOrEmpty(key))
                    return mailCC.ToArray();

                mailCC.AddRange(key.Split('|'));
                return mailCC.ToArray();
            }
        }

        public static string MailFrom
        {
            get { return ConfigurationManager.AppSettings["MailFrom"]; }
        }


        public static string MailSubject
        {
            get { return ConfigurationManager.AppSettings["MailSubject"]; }
        }

        public static string TemplateName
        {
            get { return ConfigurationManager.AppSettings["TemplateName"]; }
        }
    }

