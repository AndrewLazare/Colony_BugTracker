using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;

namespace COLONY_THE_BUGTRACKER
{
    public class EmailInformation
    {
        public List<string> Reciepents = new List<string>(); //list of sring of emails that requires all three of these
        public string Title = "";
        public string Body = "";
    }

    //EmailUtilities.SendEmail(new EmailInformation()
    //{
    //    Reciepents = listOfEmails,
    //    Title = "Test Tite",
    //    Body = "This is a test body"
    //})

    public static class EmailUtilities
    {
        public static bool SendEmail(EmailInformation info)
        {
            using (var msg = new MailMessage()) 
            {
                foreach (var email in info.Reciepents) //foreach looping through the emails.
                {
                    msg.To.Add(new MailAddress(email));//add to recip
                }
                msg.From = new MailAddress(ConfigurationManager.AppSettings.Get("EmailUsername"));// its grabbing the user name out of the configs
                msg.Subject = info.Title;
                msg.Body = info.Body.ToString();
                msg.IsBodyHtml = true;//you can use Html in it

                var client = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings.Get("SMTPServer"),
                    Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings.Get("EmailUsername"), ConfigurationManager.AppSettings.Get("EmailPassword")),
                    Port = Int32.Parse(ConfigurationManager.AppSettings.Get("SMTPPort")),
                    EnableSsl = true
                };//making connection to gmail.   getting user name, password, getting server and port.

                // You can use Port 25 if 587 is blocked 
                try
                {
                    client.Send(msg);
                    return true;
                }
                catch (SmtpException smtpEx)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}


//method will send it returs true,  if there is any tyoe of erroe it will return false