// main controller for site

using ppr_web.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ppr_web.Controllers
{
    public class HomeController : Controller
    {
        // GET: home/index
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: home/about
        [HttpGet]
        public ActionResult About()
        {
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContactForm model)
        {
            // send email from returned contact form
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("ward.keyth@outlook.com"));
                message.Subject = "PPR project email from contact page";
                message.Body = string.Format(body, model.FirstName+" "+model.LastName, model.Email, model.Comment);
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }

        // if email sent show page
        // GET: home/sent
        [HttpGet]
        public ActionResult Sent()
        {
            return View();
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

        // GET: home/advanced
        [HttpGet]
        public ActionResult Advanced()
        {
            AdvancedSearch ads = new AdvancedSearch();
            return View(ads);
        }

        // POST: home/advanced
        [HttpPost]
        public ActionResult Advanced(AdvancedSearch ads)
        {
            ads.GetLists();
            return View(ads);
        }
    }
}