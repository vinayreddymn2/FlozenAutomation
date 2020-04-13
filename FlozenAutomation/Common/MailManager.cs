namespace FlozenAutomation.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net;
    using System.Net.Mail;
    using System.IO;
    using RestSharp;
    using Newtonsoft.Json;

    public static class MailManager
    {
        public static void SendTestReport(List<String> attachments)
        {
            SmtpClient SmtpServer = new SmtpClient(ConfigManager.SmtpServer);
            SmtpServer.Port = ConfigManager.SmtpPort;
            SmtpServer.Credentials = new NetworkCredential(ConfigManager.SenderMailUser, ConfigManager.SenderPassword);
            SmtpServer.EnableSsl = true;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ConfigManager.SenderMail);
            mail.To.Add(ConfigManager.MailTo);
            if (ConfigManager.MailCc != null && ConfigManager.MailCc.Length > 0)
                mail.CC.Add(ConfigManager.MailCc);
            
            mail.Subject = ConfigManager.AppName + " Automation Report " + DateTime.Now.ToString();
            mail.Body = "Please find attached Automation Execution Report generated";

            foreach (String fileName in attachments)
            {
                Attachment attachment = new Attachment(fileName);
                mail.Attachments.Add(attachment);
            }

            SmtpServer.Send(mail);
        }

        public static void SendLatestTestReport(long testRunNum)
        {
            if (ConfigManager.SendEmailReport)
            {
                var execLogsZip = Directory.GetFiles(ConfigManager.ReportsFolder + "\\" + testRunNum, "*.zip")
                                                .Select(f => new FileInfo(f))
                                                .OrderByDescending(fi => fi.LastWriteTime)
                                                .Take(1);

                var latestReports = Directory.GetFiles(ConfigManager.ReportsFolder + "\\" + testRunNum, "*.html")
                                                .Select(f => new FileInfo(f))
                                                .OrderByDescending(fi => fi.LastWriteTime)
                                                .Take(2);                                    

                if (latestReports != null && latestReports.Count() > 0)
                {
                    List<String> files = new List<String>();
                    foreach (FileInfo fi in latestReports)
                    {
                        files.Add(fi.FullName);
                    }
                    
                    if (ConfigManager.AttachExecLogsWithEmail)
                    {
                        files.Add(execLogsZip.FirstOrDefault().FullName);
                    }

                    SendTestReport(files);
                }
            } else
            {
                Console.WriteLine("Send Email Report is not opted");
            }
        }

        public static NadaMailBox GetNadaMailBox(string mailId)
        {
            var client = new RestClient("https://getnada.com");
            var request = new RestRequest("/api/v1/inboxes/{mailid}", Method.GET);
            request.AddUrlSegment("mailid", mailId);
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);
            NadaMailBox nadaMailBox = JsonConvert.DeserializeObject<NadaMailBox>(response.Content);

            return nadaMailBox;
        }

        public static NadaMailMessage GetNadaMailMessage(string messageId)
        {
            var client = new RestClient("https://getnada.com");
            var request = new RestRequest("/api/v1/messages/{id}", Method.GET);
            request.AddUrlSegment("id", messageId);
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);
            NadaMailMessage nadaMailMessage = JsonConvert.DeserializeObject<NadaMailMessage>(response.Content);

            return nadaMailMessage;
        }

        public static NadaMailMessage GetNadaMailMessageBySubjectStartWith(string mailId, string mailSubjectStartsWith)
        {
            NadaMailBox mailBox = GetNadaMailBox(mailId);
            List<NadaMailMessage> msgs = mailBox.msgs;

            NadaMailMessage mailMessageTemp = 
                msgs.Where(msg => msgs.Any(m => (msg.s.StartsWith(mailSubjectStartsWith)))).FirstOrDefault();

            NadaMailMessage mailMessage = GetNadaMailMessage(mailMessageTemp.uid);

            return mailMessage;
        }
    }
}
