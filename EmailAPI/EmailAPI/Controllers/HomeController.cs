using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace EmailAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : Controller
    {
        static string emailFromAddress = System.Configuration.ConfigurationManager.AppSettings["emailFromAddress"];
        static string password = System.Configuration.ConfigurationManager.AppSettings["password"];
        static string smtpAddress = System.Configuration.ConfigurationManager.AppSettings["smtpAddress"];
        static int portNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["portNumber"]);
        static string subject = System.Configuration.ConfigurationManager.AppSettings["subject"];
        static bool enableSSL = true;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        [HttpPost]
        public string SendEmail(EMailModel emailModel)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFromAddress);
                    mail.To.Add(emailFromAddress);
                    mail.Subject = subject + " from client: "+ emailModel.ClientName;
                    mail.Body = "ClientEmail: "+ emailModel.ClientEmail+"; Client Contact: "+emailModel.ClientTelephone+"; Client Requirement: "+emailModel.ClientRequirement;
                    mail.IsBodyHtml = true;
                    //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                        return "Sent";
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message+ "Failed 1";
            }
            return "Failed 2";
        }


        [HttpGet]
        public string SendEmailGet()
        {
            return "sent get";
        }
    }

    public class EMailModel
    {
		public string ClientName { get; set; }
		public string ClientEmail { get; set; }
		public string ClientTelephone { get; set; }
		public string ClientRequirement { get; set; }
    }
}
