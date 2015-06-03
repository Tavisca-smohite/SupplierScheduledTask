using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Notifications
{
    public class MailAttributes
    {
        public string From { get; set; }

        public string [] To { get; set; }

        public string[] BCC { get; set; }

        public NameValueCollection TemplateAttributes { get; set; }

        public string Subject { get; set; }

        public string TemplateName { get; set; }

    }
}
