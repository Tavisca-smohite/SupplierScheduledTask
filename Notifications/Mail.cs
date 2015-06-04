using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using SupplierScheduledTask.Vexiere.NotificationService;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.TravelNxt.Shared.Entities.Infrastructure;

namespace Tavisca.SupplierScheduledTask.Notifications
{
    public class Mail : INotificationService
    {
        #region INotificationService 

        public bool SendMail(MailAttributes mail)
        {
            try
            {
                var mailMessage = new TemplateMessage
                    {
                        TemplateName = mail.TemplateName,
                        Subject = mail.Subject,
                        To = mail.To,
                        From = mail.From,
                        BCC = mail.BCC,
                        TemplateAttributes = ConvertToKeyValuePair(mail.TemplateAttributes).ToArray()
                    };

                Guid msgId = Guid.Empty;
                using (var client = new MailClient())
                {
                    msgId = client.SendMail(NotificationAppContext, mailMessage);
                }
                if (msgId != Guid.Empty)
                    return true;
            }
            catch (Exception exception)
            {
                //TODO: Add logging part
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");  
            }
            return false;
        }

        public Template GetTemplate(string templateName)
        {
            var emailTemplate = new Template();
            try
            {
                using (var client = new MailClient())
                {
                    emailTemplate = client.GetTemplate(NotificationAppContext, templateName);
                }
            }
            catch (Exception exception)
            {
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");  
            }
            return emailTemplate;
        }

        #endregion

        #region Private Methods

        private ApplicationContext NotificationAppContext
        {
            get { return new ApplicationContext(); }
        }

        private List<KeyValuePair> ConvertToKeyValuePair(NameValueCollection nameValueCollection)
        {
            var pairs = new List<KeyValuePair>();

            if (nameValueCollection != null && nameValueCollection.Count > 0)
            {
                for (int i = 0; i < nameValueCollection.Count; i++)
                {
                    var pair = new KeyValuePair
                        {Key = nameValueCollection.GetKey(i), Value = nameValueCollection[i]};
                    pairs.Add(pair);
                }
            }

            return pairs;
        }

        #endregion
    }
}