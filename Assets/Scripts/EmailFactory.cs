using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class EmailFactory : MonoBehaviour
{
    string destinatarioDelMail = "moyanoj@uji.es";

    public void SendCustomEmail(string subject, string body)
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        SmtpServer.Timeout = 10000;
        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        SmtpServer.UseDefaultCredentials = false;
        SmtpServer.Port = 587;

        mail.From = new MailAddress("initcorporativemail@gmail.com");
        mail.To.Add(new MailAddress("moyanoj@uji.es"));
        mail.To.Add(new MailAddress("rebollo@lsi.uji.es"));

        mail.Subject = subject;
        mail.Body = body;

        SmtpServer.EnableSsl = true;
        SmtpServer.Credentials = new System.Net.NetworkCredential("initcorporativemail@gmail.com", "AldeaKonoha1996") as ICredentialsByHost;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        SmtpServer.Send(mail);
    }
}