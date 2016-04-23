// main controller for site

using ppr_web.Models;
using System.Net.Mail;
using System.Web.Mvc;

namespace ppr_web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        // GET: home/contact
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        // POST: home/contact
        [HttpPost]
        public ActionResult Contact(ContactForm c)
        {
            // send email from returned contact form
            if (ModelState.IsValid)
            {
                MailMessage msg = new MailMessage();
                SmtpClient client = new SmtpClient();
                //client.Host = "smtp.gmail.com";
                client.Host = "smtp-mail.outlook.com";
                client.UseDefaultCredentials = false;
                client.Port = 25;
                client.Timeout = 20000;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential("ward.keyth2@outlook.com", "blackburn9823");

                msg.From = new MailAddress("ward.keyth2@outlook.com");
                msg.To.Add("ward.keyth@gmail.com");
                msg.Subject = "PPR contact form";
                msg.Body = "From: " + c.FirstName + " Comment: " + c.Comment;
                client.Send(msg);
                msg.Dispose();
                // return View("Success");
                return View("Search");

            }
            return View(c);
        }

        // GET: home/search
        [HttpGet]
        public ActionResult Search()
        {
            SearchDB sdb = new SearchDB();
            return View(sdb);
        }

        // POST: home/search
        [HttpPost]
        public ActionResult Search(SearchDB searchFor)
        {
            return View(searchFor);
        }

        // GET: home/combined
        [HttpGet]
        public ActionResult Combined()
        {
            CombinedSearch cs = new CombinedSearch();
            return View(cs);
        }

        // POST: home/combined
        [HttpPost]
        public ActionResult Combined(CombinedSearch cs)
        {
            return View(cs);
        }

        // POST: home/blankeditorrow
        // return a blank line for combined search page add button
        public ViewResult BlankEditorRow()
        {
            return View("~/Views/Shared/EditorTemplates/LineEditor.cshtml", new Line());
        }
    }
}