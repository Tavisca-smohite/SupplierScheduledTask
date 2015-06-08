using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupplierScheduledTask.Vexiere.NotificationService;
using Tavisca.SupplierScheduledTask.BusinessEntities;

namespace Tavisca.SupplierScheduledTask.Notifications
{
    public interface INotificationService
    {
        bool SendMail(MailAttributes mail);

        //Template GetTemplate(string templateName);
    }
}
